# Cloud Monitoring & Audit Logging
# Enables Cloud Monitoring and creates audit log sink for HIPAA compliance

# Dedicated audit log bucket (HIPAA compliance)
resource "google_storage_bucket" "audit" {
  count         = var.enable_hipaa_compliance ? 1 : 0
  name          = var.audit_bucket_name
  location      = "US"
  storage_class = "COLDLINE"
  force_destroy = false

  uniform_bucket_level_access = true

  retention_policy {
    retention_period = var.audit_retention_days * 24 * 60 * 60
  }

  labels     = var.monitoring_labels
  depends_on = [google_project_service.storage]
}

# Log sink routing audit logs to GCS
resource "google_logging_project_sink" "audit" {
  count       = var.enable_hipaa_compliance ? 1 : 0
  name        = var.log_sink_name
  destination = "storage.googleapis.com/${google_storage_bucket.audit[0].name}"
  filter      = "resource.type=\"cloudsql_database\" OR resource.type=\"cloud_run_revision\""

  unique_writer_identity = true
}

# Grant the log sink writer permission to write to the audit bucket
resource "google_storage_bucket_iam_member" "audit_writer" {
  count  = var.enable_hipaa_compliance ? 1 : 0
  bucket = google_storage_bucket.audit[0].name
  role   = "roles/storage.objectCreator"
  member = google_logging_project_sink.audit[0].writer_identity
}

# Variables
variable "log_sink_name" {
  description = "Name of the Cloud Logging sink for audit logs"
  type        = string
}

variable "audit_bucket_name" {
  description = "Name of the GCS bucket for audit logs"
  type        = string
}

variable "enable_hipaa_compliance" {
  description = "Enable HIPAA compliance features (audit log sink)"
  type        = bool
  default     = false
}

variable "audit_retention_days" {
  description = "Audit log retention in days"
  type        = number
  default     = 90
}

variable "monitoring_labels" {
  description = "Labels for monitoring resources"
  type        = map(string)
  default     = {}
}
