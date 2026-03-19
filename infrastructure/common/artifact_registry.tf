# Artifact Registry
# Central Docker image repository shared across all environments.
# Repo name matches _env.yml: REGION-docker.pkg.dev/PROJECT/carditrack

# One-time import (run before first apply if registry already exists):
#   terraform import google_artifact_registry_repository.images \
#     projects/carditrack-490120/locations/europe-west2/repositories/carditrack

resource "google_project_service" "artifactregistry" {
  service            = "artifactregistry.googleapis.com"
  disable_on_destroy = false
}

resource "google_artifact_registry_repository" "images" {
  location      = var.region
  repository_id = var.project_name
  format        = "DOCKER"
  description   = "Central Docker image registry for CardiTrack services"
  depends_on    = [google_project_service.artifactregistry]

  vulnerability_scanning_config {
    enablement_config = "DISABLED"
  }

  cleanup_policy_dry_run = false

  cleanup_policies {
    id     = "keep-last-50"
    action = "KEEP"
    most_recent_versions {
      keep_count = 50
    }
  }

  cleanup_policies {
    id     = "delete-old"
    action = "DELETE"
  }
}

# CI/CD service account — push images
resource "google_artifact_registry_repository_iam_member" "ci_writer" {
  location   = var.region
  repository = google_artifact_registry_repository.images.name
  role       = "roles/artifactregistry.writer"
  member     = "serviceAccount:carditrack-deploy@${var.project_id}.iam.gserviceaccount.com"
}

# Cloud Run compute SA — pull images
resource "google_artifact_registry_repository_iam_member" "cloud_run_reader" {
  location   = var.region
  repository = google_artifact_registry_repository.images.name
  role       = "roles/artifactregistry.reader"
  member     = "serviceAccount:${data.google_project.current.number}-compute@developer.gserviceaccount.com"
}
