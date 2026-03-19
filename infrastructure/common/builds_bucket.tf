# Builds Bucket
# Central GCS bucket for mobile build artifacts, shared across all environments.
# Bucket name matches _env.yml: carditrack-builds

# One-time import (run before first apply if bucket already exists):
#   terraform import google_storage_bucket.builds carditrack-builds

resource "google_storage_bucket" "builds" {
  name          = "${var.project_name}-builds"
  location      = "EU"
  storage_class = "STANDARD"
  force_destroy = false

  uniform_bucket_level_access = true
  public_access_prevention    = "enforced"

  versioning {
    enabled = false
  }

  lifecycle_rule {
    action { type = "Delete" }
    condition { age = 10 }
  }
}

# CI/CD service account — upload build artifacts
resource "google_storage_bucket_iam_member" "builds_ci_writer" {
  bucket = google_storage_bucket.builds.name
  role   = "roles/storage.objectCreator"
  member = "serviceAccount:carditrack-deploy@${var.project_id}.iam.gserviceaccount.com"
}

# CI/CD service account — read artifacts (prod download step)
resource "google_storage_bucket_iam_member" "builds_ci_reader" {
  bucket = google_storage_bucket.builds.name
  role   = "roles/storage.objectViewer"
  member = "serviceAccount:carditrack-deploy@${var.project_id}.iam.gserviceaccount.com"
}
