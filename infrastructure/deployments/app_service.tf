# App Service
# Manages App Service Plan and Web Apps (API + Web)

resource "azurerm_service_plan" "main" {
  name                = var.app_service_plan_name
  location            = var.app_service_location
  resource_group_name = var.app_service_resource_group_name
  os_type             = "Linux"
  sku_name            = var.app_service_sku_name

  tags = var.app_service_tags
}

resource "azurerm_linux_web_app" "api" {
  name                = var.api_app_name
  location            = var.app_service_location
  resource_group_name = var.app_service_resource_group_name
  service_plan_id     = azurerm_service_plan.main.id

  site_config {
    application_stack {
      dotnet_version = "10.0"
    }

    always_on              = var.app_service_always_on
    http2_enabled          = var.app_service_http2_enabled
    ftps_state             = var.app_service_ftps_state
    minimum_tls_version    = "1.2"
    health_check_path      = var.app_service_health_check_path
  }

  app_settings = var.api_app_settings
  https_only   = var.app_service_https_only

  tags = var.app_service_tags
}

resource "azurerm_linux_web_app" "web" {
  name                = var.web_app_name
  location            = var.app_service_location
  resource_group_name = var.app_service_resource_group_name
  service_plan_id     = azurerm_service_plan.main.id

  site_config {
    application_stack {
      dotnet_version = "10.0"
    }

    always_on              = var.app_service_always_on
    http2_enabled          = var.app_service_http2_enabled
    ftps_state             = var.app_service_ftps_state
    minimum_tls_version    = "1.2"
    health_check_path      = var.app_service_health_check_path
  }

  app_settings = var.web_app_settings
  https_only   = var.app_service_https_only

  tags = var.app_service_tags
}

# Variables
variable "app_service_plan_name" {
  description = "Name of the App Service Plan"
  type        = string
}

variable "api_app_name" {
  description = "Name of the API App Service"
  type        = string
}

variable "web_app_name" {
  description = "Name of the Web App Service"
  type        = string
}

variable "app_service_location" {
  description = "Azure region"
  type        = string
}

variable "app_service_resource_group_name" {
  description = "Resource group name"
  type        = string
}

variable "app_service_sku_name" {
  description = "App Service Plan SKU"
  type        = string
  default     = "B1"
}

variable "app_service_always_on" {
  description = "Keep app always on"
  type        = bool
  default     = false
}

variable "app_service_http2_enabled" {
  description = "Enable HTTP/2"
  type        = bool
  default     = false
}

variable "app_service_ftps_state" {
  description = "FTPS state"
  type        = string
  default     = "AllAllowed"
}

variable "app_service_health_check_path" {
  description = "Health check path"
  type        = string
  default     = null
}

variable "app_service_https_only" {
  description = "Force HTTPS only"
  type        = bool
  default     = false
}

variable "api_app_settings" {
  description = "App settings for API app"
  type        = map(string)
  default     = {}
}

variable "web_app_settings" {
  description = "App settings for Web app"
  type        = map(string)
  default     = {}
}

variable "app_service_tags" {
  description = "Tags"
  type        = map(string)
  default     = {}
}

