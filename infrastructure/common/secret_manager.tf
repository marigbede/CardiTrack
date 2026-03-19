# Secret Manager — Shared / Common Secrets
# Secrets here are not environment-specific.
# Values are set by operators via:
#   gcloud secrets versions add <id> --data-file=-

resource "google_project_service" "secretmanager" {
  service            = "secretmanager.googleapis.com"
  disable_on_destroy = false
}

locals {
  appetize_secrets = {
    "appetize-api-token"          = "REPLACE_ME"
    "appetize-android-public-key" = "REPLACE_ME"
    "appetize-ios-public-key"     = "REPLACE_ME"
  }
}

resource "google_secret_manager_secret" "appetize" {
  for_each  = local.appetize_secrets
  secret_id = "${var.project_name}-${each.key}"

  replication {
    auto {}
  }

  depends_on = [google_project_service.secretmanager]
}

resource "google_secret_manager_secret_version" "appetize" {
  for_each    = local.appetize_secrets
  secret      = google_secret_manager_secret.appetize[each.key].id
  secret_data = each.value

  lifecycle {
    ignore_changes = [secret_data]
  }
}

# CI/CD service account — read Appetize credentials at deploy time
resource "google_secret_manager_secret_iam_member" "appetize_deploy_accessor" {
  for_each  = local.appetize_secrets
  secret_id = google_secret_manager_secret.appetize[each.key].id
  role      = "roles/secretmanager.secretAccessor"
  member    = "serviceAccount:carditrack-deploy@${var.project_id}.iam.gserviceaccount.com"
}
