# Secret Manager — Shared / Common Secrets
# Values are set by operators via:
#   echo -n "your_token" | gcloud secrets versions add carditrack-appetize-api-token --data-file=-


resource "google_project_service" "common_secretmanager" {
  service            = "secretmanager.googleapis.com"
  disable_on_destroy = false
}

resource "google_secret_manager_secret" "common_appetize_api_token" {
  secret_id = "${var.project_name}-common-appetize-api-token"

  replication {
    auto {}
  }

  depends_on = [google_project_service.common_secretmanager]
}

resource "google_secret_manager_secret_version" "common_appetize_api_token" {
  secret      = google_secret_manager_secret.common_appetize_api_token.id
  secret_data = "REPLACE_ME"

  lifecycle {
    ignore_changes = [secret_data]
  }
}

resource "google_secret_manager_secret_iam_member" "common_appetize_api_token_accessor" {
  secret_id = google_secret_manager_secret.common_appetize_api_token.id
  role      = "roles/secretmanager.secretAccessor"
  member    = "serviceAccount:carditrack-deploy@${var.project_id}.iam.gserviceaccount.com"
}
