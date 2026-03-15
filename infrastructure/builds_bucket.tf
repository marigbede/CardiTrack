# Builds Bucket
# Central GCS bucket for mobile build artifacts, shared across all environments.
# Managed by the dev Terraform state only (count = 0 in prod).
# Bucket name matches _env.yml: carditrack-builds

resource "google_storage_bucket" "builds" {
  count         = var.environment == "dev" ? 1 : 0
  name          = "carditrack-builds"
  location      = "EU"
  storage_class = "STANDARD"
  force_destroy = false

  uniform_bucket_level_access = true
  public_access_prevention    = "enforced"

  versioning {
    enabled = false
  }

  labels = {
    environment = "shared"
    project     = "carditrack"
    managed_by  = "terraform"
    cost_center = "engineering"
  }

  depends_on = [module.deployments]
}

# CI/CD service account — upload build artifacts
resource "google_storage_bucket_iam_member" "builds_ci_writer" {
  count  = var.environment == "dev" ? 1 : 0
  bucket = google_storage_bucket.builds[0].name
  role   = "roles/storage.objectCreator"
  member = "serviceAccount:carditrack-deploy@${var.project_id}.iam.gserviceaccount.com"
}

# CI/CD service account — read artifacts (for prod download step)
resource "google_storage_bucket_iam_member" "builds_ci_reader" {
  count  = var.environment == "dev" ? 1 : 0
  bucket = google_storage_bucket.builds[0].name
  role   = "roles/storage.objectViewer"
  member = "serviceAccount:carditrack-deploy@${var.project_id}.iam.gserviceaccount.com"
}
