# Monitoring
# Manages Application Insights

resource "azurerm_application_insights" "main" {
  name                = var.app_insights_name
  location            = var.monitoring_location
  resource_group_name = var.monitoring_resource_group_name
  application_type    = "web"
  retention_in_days   = var.monitoring_retention_in_days

  tags = var.monitoring_tags
}

# Variables
variable "app_insights_name" {
  description = "Name of Application Insights"
  type        = string
}

variable "monitoring_location" {
  description = "Azure region"
  type        = string
}

variable "monitoring_resource_group_name" {
  description = "Resource group name"
  type        = string
}

variable "monitoring_retention_in_days" {
  description = "Retention in days"
  type        = number
  default     = 30
}

variable "monitoring_tags" {
  description = "Tags"
  type        = map(string)
  default     = {}
}

