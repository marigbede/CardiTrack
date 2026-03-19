# Artifact Registry
# Central Docker image repository shared across all environments.


resource "google_project_service" "common_artifactregistry" {
  service            = "artifactregistry.googleapis.com"
  disable_on_destroy = false
}

resource "google_artifact_registry_repository" "common" {
  location      = var.region
  repository_id = "${var.project_name}-common"
  format        = "DOCKER"
  description   = "Central Docker image registry for CardiTrack services"
  depends_on    = [google_project_service.common_artifactregistry]

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
    condition {
      tag_state = "ANY"
    }
  }
}

resource "google_artifact_registry_repository_iam_member" "common_ci_writer" {
  location   = var.region
  repository = google_artifact_registry_repository.common.name
  role       = "roles/artifactregistry.writer"
  member     = "serviceAccount:carditrack-deploy@${var.project_id}.iam.gserviceaccount.com"
}

resource "google_artifact_registry_repository_iam_member" "common_cloud_run_reader" {
  location   = var.region
  repository = google_artifact_registry_repository.common.name
  role       = "roles/artifactregistry.reader"
  member     = "serviceAccount:${data.google_project.current.number}-compute@developer.gserviceaccount.com"
}
