output "artifact_registry_repository" {
  description = "Full path of the Artifact Registry repository"
  value       = google_artifact_registry_repository.common.name
}

output "builds_bucket_name" {
  description = "Name of the shared mobile builds GCS bucket"
  value       = google_storage_bucket.common_builds.name
}

output "appetize_api_token_secret_id" {
  description = "Secret Manager ID for the Appetize API token"
  value       = google_secret_manager_secret.common_appetize_api_token.secret_id
}
