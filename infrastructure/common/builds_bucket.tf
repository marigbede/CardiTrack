# Builds Bucket
# Central GCS bucket for mobile build artifacts, shared across all environments.


resource "google_storage_bucket" "common_builds" {
  name          = "${var.project_name}-common-builds"
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

resource "google_storage_bucket_iam_member" "common_builds_ci_writer" {
  bucket = google_storage_bucket.common_builds.name
  role   = "roles/storage.objectCreator"
  member = "serviceAccount:carditrack-deploy@${var.project_id}.iam.gserviceaccount.com"
}

resource "google_storage_bucket_iam_member" "common_builds_ci_reader" {
  bucket = google_storage_bucket.common_builds.name
  role   = "roles/storage.objectViewer"
  member = "serviceAccount:carditrack-deploy@${var.project_id}.iam.gserviceaccount.com"
}
