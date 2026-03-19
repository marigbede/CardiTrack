# Resources moved to infrastructure/common/artifact_registry.tf
#
# These removed blocks drop the resources from the dev/prod state without
# destroying them. Apply once after the common environment has been deployed.

removed {
  from = google_project_service.artifactregistry
  lifecycle { destroy = false }
}

removed {
  from = google_artifact_registry_repository.images
  lifecycle { destroy = false }
}

removed {
  from = google_artifact_registry_repository_iam_member.ci_writer
  lifecycle { destroy = false }
}

removed {
  from = google_artifact_registry_repository_iam_member.cloud_run_reader
  lifecycle { destroy = false }
}
