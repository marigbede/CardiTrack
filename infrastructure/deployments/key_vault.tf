# Key Vault
# Manages Azure Key Vault

data "azurerm_client_config" "current" {}

resource "azurerm_key_vault" "main" {
  name                = var.key_vault_name
  location            = var.key_vault_location
  resource_group_name = var.key_vault_resource_group_name
  tenant_id           = data.azurerm_client_config.current.tenant_id
  sku_name            = var.key_vault_sku_name

  soft_delete_retention_days = var.key_vault_soft_delete_retention_days
  purge_protection_enabled   = var.key_vault_purge_protection_enabled

  enabled_for_disk_encryption = var.key_vault_enabled_for_disk_encryption
  enabled_for_deployment      = var.key_vault_enabled_for_deployment

  dynamic "network_acls" {
    for_each = var.key_vault_enable_network_acls ? [1] : []
    content {
      default_action = "Deny"
      bypass         = "AzureServices"
    }
  }

  tags = var.key_vault_tags
}

# Variables
variable "key_vault_name" {
  description = "Name of the Key Vault"
  type        = string
}

variable "key_vault_location" {
  description = "Azure region"
  type        = string
}

variable "key_vault_resource_group_name" {
  description = "Resource group name"
  type        = string
}

variable "key_vault_sku_name" {
  description = "Key Vault SKU (standard or premium)"
  type        = string
  default     = "standard"
}

variable "key_vault_soft_delete_retention_days" {
  description = "Soft delete retention in days"
  type        = number
  default     = 7
}

variable "key_vault_purge_protection_enabled" {
  description = "Enable purge protection"
  type        = bool
  default     = false
}

variable "key_vault_enabled_for_disk_encryption" {
  description = "Enable for disk encryption"
  type        = bool
  default     = false
}

variable "key_vault_enabled_for_deployment" {
  description = "Enable for deployment"
  type        = bool
  default     = false
}

variable "key_vault_enable_network_acls" {
  description = "Enable network ACLs"
  type        = bool
  default     = false
}

variable "key_vault_tags" {
  description = "Tags"
  type        = map(string)
  default     = {}
}

