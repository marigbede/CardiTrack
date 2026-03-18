# Cloud Storage
# Manages GCS bucket for application storage

# Variables
variable "storage_bucket_name" {
  description = "Name of the GCS bucket"
  type        = string
}

variable "storage_location" {
  description = "GCS bucket location (US, EU, ASIA, or specific region)"
  type        = string
  default     = "EU"
}

variable "storage_class" {
  description = "GCS storage class (STANDARD, NEARLINE, COLDLINE, ARCHIVE)"
  type        = string
  default     = "STANDARD"
}

variable "storage_force_destroy" {
  description = "Allow bucket deletion even if non-empty"
  type        = bool
  default     = false
}

variable "storage_labels" {
  description = "Labels for storage resources"
  type        = map(string)
  default     = {}
}

# Resources
resource "google_storage_bucket" "main" {
  name          = var.storage_bucket_name
  location      = var.storage_location
  storage_class = var.storage_class
  force_destroy = var.storage_force_destroy

  uniform_bucket_level_access = true
  public_access_prevention    = "enforced"

  versioning {
    enabled = true
  }

  labels     = var.storage_labels
  depends_on = [google_project_service.storage]
}
