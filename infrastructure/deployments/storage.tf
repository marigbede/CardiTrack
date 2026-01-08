# Storage Account
# Manages Azure Storage Account

resource "azurerm_storage_account" "main" {
  name                     = var.storage_account_name
  location                 = var.storage_location
  resource_group_name      = var.storage_resource_group_name
  account_tier             = "Standard"
  account_replication_type = var.storage_replication_type

  min_tls_version                   = "TLS1_2"
  https_traffic_only_enabled        = true
  infrastructure_encryption_enabled = var.storage_infrastructure_encryption_enabled

  tags = var.storage_tags
}

# Variables
variable "storage_account_name" {
  description = "Name of the storage account"
  type        = string
}

variable "storage_location" {
  description = "Azure region"
  type        = string
}

variable "storage_resource_group_name" {
  description = "Resource group name"
  type        = string
}

variable "storage_replication_type" {
  description = "Storage replication type (LRS, GRS, etc.)"
  type        = string
  default     = "LRS"
}

variable "storage_infrastructure_encryption_enabled" {
  description = "Enable infrastructure encryption"
  type        = bool
  default     = false
}

variable "storage_tags" {
  description = "Tags"
  type        = map(string)
  default     = {}
}

