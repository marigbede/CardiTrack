variable "project_id" {
  description = "GCP project ID"
  type        = string
}

variable "region" {
  description = "GCP region"
  type        = string
  default     = "europe-west2"
}

variable "project_name" {
  description = "Project name used for resource naming"
  type        = string
  default     = "carditrack"
}
