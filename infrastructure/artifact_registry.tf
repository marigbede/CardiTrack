# Artifact Registry
# Central Docker image repository shared across all environments.

data "google_project" "current" {}

# Managed by the dev Terraform state only (count = 0 in prod).
# Repo name matches _env.yml: REGION-docker.pkg.dev/PROJECT/carditrack

resource "google_project_service" "artifactregistry" {
  count              = var.environment == "dev" ? 1 : 0
  service            = "artifactregistry.googleapis.com"
  disable_on_destroy = false
}

resource "google_artifact_registry_repository" "images" {
  count         = var.environment == "dev" ? 1 : 0
  location      = var.region
  repository_id = var.project_name
  format        = "DOCKER"
  description   = "Central Docker image registry for CardiTrack services"
  depends_on    = [google_project_service.artifactregistry]
}

# CI/CD service account — push images
resource "google_artifact_registry_repository_iam_member" "ci_writer" {
  count      = var.environment == "dev" ? 1 : 0
  location   = var.region
  repository = google_artifact_registry_repository.images[0].name
  role       = "roles/artifactregistry.writer"
  member     = "serviceAccount:carditrack-deploy@${var.project_id}.iam.gserviceaccount.com"
}

# Cloud Run compute SA — pull images
resource "google_artifact_registry_repository_iam_member" "cloud_run_reader" {
  count      = var.environment == "dev" ? 1 : 0
  location   = var.region
  repository = google_artifact_registry_repository.images[0].name
  role       = "roles/artifactregistry.reader"
  member     = "serviceAccount:${data.google_project.current.number}-compute@developer.gserviceaccount.com"
}
