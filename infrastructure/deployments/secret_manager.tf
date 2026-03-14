# Secret Manager
# Manages Google Cloud Secret Manager secrets
# Replaces Azure Key Vault

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

# Variables
variable "db_password_secret_id" {
  description = "Secret Manager secret ID for database password"
  type        = string
}

variable "secret_labels" {
  description = "Labels for Secret Manager resources"
  type        = map(string)
  default     = {}
}
