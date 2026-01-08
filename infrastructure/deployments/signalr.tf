# SignalR Service
# Manages Azure SignalR Service for real-time communication

resource "azurerm_signalr_service" "main" {
  count               = var.signalr_capacity > 0 ? 1 : 0
  name                = var.signalr_name
  location            = var.signalr_location
  resource_group_name = var.signalr_resource_group_name

  sku {
    name     = var.signalr_sku_name
    capacity = var.signalr_capacity
  }

  cors {
    allowed_origins = var.signalr_allowed_origins
  }

  tags = var.signalr_tags
}

# Variables
variable "signalr_name" {
  description = "Name of SignalR Service"
  type        = string
}

variable "signalr_location" {
  description = "Azure region"
  type        = string
}

variable "signalr_resource_group_name" {
  description = "Resource group name"
  type        = string
}

variable "signalr_sku_name" {
  description = "SignalR SKU"
  type        = string
  default     = "Standard_S1"
}

variable "signalr_capacity" {
  description = "SignalR capacity"
  type        = number
  default     = 1
}

variable "signalr_allowed_origins" {
  description = "Allowed CORS origins"
  type        = list(string)
  default     = []
}

variable "signalr_tags" {
  description = "Tags"
  type        = map(string)
  default     = {}
}

