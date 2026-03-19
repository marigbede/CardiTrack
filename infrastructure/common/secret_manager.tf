# Secret Manager — Shared / Common Secrets
# Secrets here are not environment-specific.
# Values are set by operators via:
#   echo -n "your_token" | gcloud secrets versions add carditrack-appetize-api-token --data-file=-

resource "google_project_service" "secretmanager" {
  service            = "secretmanager.googleapis.com"
  disable_on_destroy = false
}

resource "google_secret_manager_secret" "appetize_api_token" {
  secret_id = "${var.project_name}-appetize-api-token"

  replication {
    auto {}
  }

  depends_on = [google_project_service.secretmanager]
}

resource "google_secret_manager_secret_version" "appetize_api_token" {
  secret      = google_secret_manager_secret.appetize_api_token.id
  secret_data = "REPLACE_ME"

  lifecycle {
    ignore_changes = [secret_data]
  }
}

# CI/CD service account — read token at deploy time
resource "google_secret_manager_secret_iam_member" "appetize_api_token_accessor" {
  secret_id = google_secret_manager_secret.appetize_api_token.id
  role      = "roles/secretmanager.secretAccessor"
  member    = "serviceAccount:carditrack-deploy@${var.project_id}.iam.gserviceaccount.com"
}
