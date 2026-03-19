# CardiTrack Common Infrastructure
# Resources here are shared across all environments (dev, prod).
# This root has its own state: carditrack-tf-state-common / carditrack/common

locals {
  labels = {
    environment = "common"
    project     = var.project_name
    managed_by  = "terraform"
    cost_center = "engineering"
  }
}

data "google_project" "current" {}
