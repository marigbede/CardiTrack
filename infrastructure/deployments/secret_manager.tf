# Secret Manager
# Manages Google Cloud Secret Manager secrets

locals {
  db_connection_string = join(";", [
    "Host=${google_sql_database_instance.main.public_ip_address}",
    "Port=5432",
    "Database=${google_sql_database.main.name}",
    "Username=${var.db_admin_username}",
    "Password=${random_password.db_password.result}",
    "SSL Mode=Require",
  ])

  # Secrets whose values Terraform sets with a placeholder.
  # Operators overwrite via: gcloud secrets versions add <id> --data-file=-
  # lifecycle.ignore_changes ensures subsequent applies never revert operator values.
  placeholder_secrets = {
    "auth0-domain"        = "REPLACE_ME"
    "auth0-audience"      = "REPLACE_ME"
    "auth0-client-id"     = "REPLACE_ME"
    "auth0-client-secret" = "REPLACE_ME"
    "encryption-key"      = "REPLACE_ME"
  }
}

# ── DB password ──────────────────────────────────────────────────────────────

resource "random_password" "db_password" {
  length           = 32
  special          = true
  override_special = "!#$%&*()-_=+[]{}<>:?"
}

resource "google_secret_manager_secret" "db_password" {
  secret_id = var.db_password_secret_id
  replication {
    auto {}
  }
  labels     = var.secret_labels
  depends_on = [google_project_service.secretmanager]
}

resource "google_secret_manager_secret_version" "db_password" {
  secret      = google_secret_manager_secret.db_password.id
  secret_data = random_password.db_password.result
}

# ── DB connection string (Terraform-owned value) ─────────────────────────────

resource "google_secret_manager_secret" "db_connection_string" {
  secret_id = "${var.secret_id_prefix}-db-connection-string"
  replication {
    auto {}
  }
  labels     = var.secret_labels
  depends_on = [google_project_service.secretmanager]
}

resource "google_secret_manager_secret_version" "db_connection_string" {
  secret      = google_secret_manager_secret.db_connection_string.id
  secret_data = local.db_connection_string
}

# ── Auth0 + Encryption (placeholder values, operator-overwritten) ─────────────

resource "google_secret_manager_secret" "app_secrets" {
  for_each  = local.placeholder_secrets
  secret_id = "${var.secret_id_prefix}-${each.key}"
  replication {
    auto {}
  }
  labels     = var.secret_labels
  depends_on = [google_project_service.secretmanager]
}

resource "google_secret_manager_secret_version" "app_secrets" {
  for_each    = local.placeholder_secrets
  secret      = google_secret_manager_secret.app_secrets[each.key].id
  secret_data = each.value

  lifecycle {
    ignore_changes = [secret_data]
  }
}

# ── IAM — Cloud Run default compute SA can read all app secrets ───────────────

resource "google_secret_manager_secret_iam_member" "db_conn_accessor" {
  secret_id = google_secret_manager_secret.db_connection_string.id
  role      = "roles/secretmanager.secretAccessor"
  member    = "serviceAccount:${data.google_project.current.number}-compute@developer.gserviceaccount.com"
}

resource "google_secret_manager_secret_iam_member" "app_secrets_accessor" {
  for_each  = local.placeholder_secrets
  secret_id = google_secret_manager_secret.app_secrets[each.key].id
  role      = "roles/secretmanager.secretAccessor"
  member    = "serviceAccount:${data.google_project.current.number}-compute@developer.gserviceaccount.com"
}

# ── Variables ─────────────────────────────────────────────────────────────────

variable "db_password_secret_id" {
  description = "Secret Manager secret ID for database password"
  type        = string
}

variable "secret_id_prefix" {
  description = "Prefix for app secret IDs (e.g. carditrack-dev)"
  type        = string
}

variable "secret_labels" {
  description = "Labels for Secret Manager resources"
  type        = map(string)
  default     = {}
}
