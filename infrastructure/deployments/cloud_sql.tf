# Cloud SQL (PostgreSQL)
# Manages Cloud SQL PostgreSQL instance and database

# Variables
variable "cloud_sql_instance_name" {
  description = "Name of the Cloud SQL instance"
  type        = string
}

variable "cloud_sql_database_name" {
  description = "Name of the Cloud SQL database"
  type        = string
}

variable "cloud_sql_region" {
  description = "GCP region for Cloud SQL"
  type        = string
}

variable "db_admin_username" {
  description = "Database administrator username"
  type        = string
}

variable "cloud_sql_tier" {
  description = "Cloud SQL machine tier"
  type        = string
  default     = "db-f1-micro"
}

variable "cloud_sql_edition" {
  description = "Cloud SQL edition (ENTERPRISE or ENTERPRISE_PLUS)"
  type        = string
  default     = "ENTERPRISE"
}

variable "cloud_sql_disk_size_gb" {
  description = "Cloud SQL disk size in GB"
  type        = number
  default     = 10
}

variable "cloud_sql_ha_enabled" {
  description = "Enable high availability (REGIONAL availability type)"
  type        = bool
  default     = false
}

variable "cloud_sql_public_ip_enabled" {
  description = "Enable public IP for Cloud SQL"
  type        = bool
  default     = true
}

variable "cloud_sql_deletion_protection" {
  description = "Enable deletion protection for Cloud SQL instance"
  type        = bool
  default     = false
}

variable "cloud_sql_enable_audit" {
  description = "Enable database audit logging flags"
  type        = bool
  default     = false
}

variable "cloud_sql_labels" {
  description = "Labels for Cloud SQL resources"
  type        = map(string)
  default     = {}
}

# Resources
resource "google_sql_database_instance" "main" {
  name             = var.cloud_sql_instance_name
  database_version = "POSTGRES_16"
  region           = var.cloud_sql_region

  settings {
    tier              = var.cloud_sql_tier
    edition           = var.cloud_sql_edition
    availability_type = var.cloud_sql_ha_enabled ? "REGIONAL" : "ZONAL"
    disk_size         = var.cloud_sql_disk_size_gb
    disk_type         = "PD_SSD"
    disk_autoresize   = true

    backup_configuration {
      enabled    = true
      start_time = "03:00"

      backup_retention_settings {
        retained_backups = 7
      }
    }

    ip_configuration {
      ipv4_enabled    = var.cloud_sql_public_ip_enabled
      private_network = google_compute_network.main.self_link
      ssl_mode        = "ENCRYPTED_ONLY"
    }

    database_flags {
      name  = "log_connections"
      value = var.cloud_sql_enable_audit ? "on" : "off"
    }

    database_flags {
      name  = "log_disconnections"
      value = var.cloud_sql_enable_audit ? "on" : "off"
    }

    user_labels = var.cloud_sql_labels
  }

  deletion_protection = var.cloud_sql_deletion_protection

  depends_on = [google_project_service.sql, google_service_networking_connection.private_services]
}

resource "google_sql_database" "main" {
  name     = var.cloud_sql_database_name
  instance = google_sql_database_instance.main.name
}

resource "google_sql_user" "main" {
  name     = var.db_admin_username
  instance = google_sql_database_instance.main.name
  password = random_password.db_password.result
}

# ── IAM — allow the default Compute SA to use the Cloud SQL Auth Proxy ────────
# Required for Cloud Run services (API, Web, Migrator) to connect via Unix socket.
resource "google_project_iam_member" "cloudsql_client" {
  project = var.project_id
  role    = "roles/cloudsql.client"
  member  = "serviceAccount:${data.google_project.current.number}-compute@developer.gserviceaccount.com"
}
