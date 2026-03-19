output "artifact_registry_repository" {
  description = "Full path of the Artifact Registry repository"
  value       = google_artifact_registry_repository.images.name
}

output "builds_bucket_name" {
  description = "Name of the shared mobile builds GCS bucket"
  value       = google_storage_bucket.builds.name
}

output "appetize_secret_ids" {
  description = "Secret Manager IDs for Appetize credentials"
  value       = { for k, v in google_secret_manager_secret.appetize : k => v.secret_id }
}
