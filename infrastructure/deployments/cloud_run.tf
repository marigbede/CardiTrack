# Cloud Run Services
# Manages API and Web services on Google Cloud Run

resource "google_cloud_run_v2_service" "api" {
  name     = var.api_service_name
  location = var.cloud_run_location
  ingress  = local.has_any_domain ? "INGRESS_TRAFFIC_INTERNAL_LOAD_BALANCER" : "INGRESS_TRAFFIC_ALL"
  client   = "terraform"

  template {
    containers {
      image = var.api_container_image

      dynamic "env" {
        for_each = var.api_env_vars
        iterator = item
        content {
          name  = item.key
          value = item.value
        }
      }

      dynamic "env" {
        for_each = var.api_secret_env_vars
        iterator = item
        content {
          name = item.key
          value_source {
            secret_key_ref {
              secret  = item.value
              version = "latest"
            }
          }
        }
      }

      resources {
        limits = {
          cpu    = var.cloud_run_cpu
          memory = var.cloud_run_memory
        }
      }
    }

    scaling {
      min_instance_count = var.cloud_run_min_instances
      max_instance_count = var.cloud_run_max_instances
    }
  }

  labels = var.cloud_run_labels
  depends_on = [
    google_project_service.run,
    google_secret_manager_secret_version.app_secrets,
    google_secret_manager_secret_version.db_connection_string,
  ]
}

resource "google_cloud_run_v2_service" "web" {
  name     = var.web_service_name
  location = var.cloud_run_location
  ingress  = local.has_any_domain ? "INGRESS_TRAFFIC_INTERNAL_LOAD_BALANCER" : "INGRESS_TRAFFIC_ALL"
  client   = "terraform"

  template {
    containers {
      image = var.web_container_image

      dynamic "env" {
        for_each = var.web_env_vars
        iterator = item
        content {
          name  = item.key
          value = item.value
        }
      }

      dynamic "env" {
        for_each = var.web_secret_env_vars
        iterator = item
        content {
          name = item.key
          value_source {
            secret_key_ref {
              secret  = item.value
              version = "latest"
            }
          }
        }
      }

      resources {
        limits = {
          cpu    = var.cloud_run_cpu
          memory = var.cloud_run_memory
        }
      }
    }

    scaling {
      min_instance_count = var.cloud_run_min_instances
      max_instance_count = var.cloud_run_max_instances
    }
  }

  labels = var.cloud_run_labels
  depends_on = [
    google_project_service.run,
    google_secret_manager_secret_version.app_secrets,
    google_secret_manager_secret_version.db_connection_string,
  ]
}

# Allow unauthenticated access (traffic enters via GCLB + Cloud Armor)
resource "google_cloud_run_v2_service_iam_member" "api_public" {
  name     = google_cloud_run_v2_service.api.name
  location = google_cloud_run_v2_service.api.location
  role     = "roles/run.invoker"
  member   = "allUsers"
}

resource "google_cloud_run_v2_service_iam_member" "web_public" {
  name     = google_cloud_run_v2_service.web.name
  location = google_cloud_run_v2_service.web.location
  role     = "roles/run.invoker"
  member   = "allUsers"
}

# Variables
variable "api_service_name" {
  description = "Name of the API Cloud Run service"
  type        = string
}

variable "web_service_name" {
  description = "Name of the Web Cloud Run service"
  type        = string
}

variable "cloud_run_location" {
  description = "GCP region for Cloud Run services"
  type        = string
}

variable "api_container_image" {
  description = "Container image for the API service"
  type        = string
  default     = "us-docker.pkg.dev/cloudrun/container/hello"
}

variable "web_container_image" {
  description = "Container image for the Web service"
  type        = string
  default     = "us-docker.pkg.dev/cloudrun/container/hello"
}

variable "api_env_vars" {
  description = "Environment variables for API service"
  type        = map(string)
  default     = {}
}

variable "api_secret_env_vars" {
  description = "Secret Manager-backed env vars for API service (key=env var name, value=secret ID)"
  type        = map(string)
  default     = {}
}

variable "web_env_vars" {
  description = "Environment variables for Web service"
  type        = map(string)
  default     = {}
}

variable "web_secret_env_vars" {
  description = "Secret Manager-backed env vars for Web service (key=env var name, value=secret ID)"
  type        = map(string)
  default     = {}
}

variable "cloud_run_cpu" {
  description = "CPU allocation for Cloud Run services"
  type        = string
  default     = "1"
}

variable "cloud_run_memory" {
  description = "Memory allocation for Cloud Run services"
  type        = string
  default     = "512Mi"
}

variable "cloud_run_min_instances" {
  description = "Minimum number of Cloud Run instances"
  type        = number
  default     = 0
}

variable "cloud_run_max_instances" {
  description = "Maximum number of Cloud Run instances"
  type        = number
  default     = 10
}

variable "cloud_run_labels" {
  description = "Labels for Cloud Run services"
  type        = map(string)
  default     = {}
}

variable "api_custom_domain" {
  description = "Custom domain for the API service (e.g. api.carditrack.com)"
  type        = string
  default     = ""
}

variable "web_custom_domain" {
  description = "Custom domain for the Web service (e.g. app.carditrack.com)"
  type        = string
  default     = ""
}
