# Resources moved to infrastructure/common/builds_bucket.tf
#
# These removed blocks drop the resources from the dev/prod state without
# destroying them. Apply once after the common environment has been deployed.

removed {
  from = google_storage_bucket.builds
  lifecycle { destroy = false }
}

removed {
  from = google_storage_bucket_iam_member.builds_ci_writer
  lifecycle { destroy = false }
}

removed {
  from = google_storage_bucket_iam_member.builds_ci_reader
  lifecycle { destroy = false }
}
