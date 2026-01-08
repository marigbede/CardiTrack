# Azure SQL
# Manages SQL Server and Database

resource "azurerm_mssql_server" "main" {
  name                          = var.sql_server_name
  location                      = var.sql_location
  resource_group_name           = var.sql_resource_group_name
  version                       = "12.0"
  administrator_login           = var.sql_admin_username
  administrator_login_password  = var.sql_admin_password
  minimum_tls_version           = "1.2"
  public_network_access_enabled = var.sql_public_network_access_enabled

  dynamic "azuread_administrator" {
    for_each = var.sql_aad_admin_login != null && var.sql_aad_admin_object_id != null ? [1] : []
    content {
      login_username = var.sql_aad_admin_login
      object_id      = var.sql_aad_admin_object_id
    }
  }

  tags = var.sql_tags
}

resource "azurerm_mssql_database" "main" {
  name           = var.sql_database_name
  server_id      = azurerm_mssql_server.main.id
  sku_name       = var.sql_database_sku_name
  max_size_gb    = var.sql_database_max_size_gb
  zone_redundant = var.sql_zone_redundant

  tags = var.sql_tags
}

# Firewall rule to allow Azure services
resource "azurerm_mssql_firewall_rule" "allow_azure_services" {
  name             = "AllowAzureServices"
  server_id        = azurerm_mssql_server.main.id
  start_ip_address = "0.0.0.0"
  end_ip_address   = "0.0.0.0"
}

# Extended auditing policy (optional, for HIPAA compliance)
resource "azurerm_mssql_database_extended_auditing_policy" "main" {
  count = var.sql_enable_auditing ? 1 : 0

  database_id                = azurerm_mssql_database.main.id
  storage_endpoint           = var.sql_audit_storage_endpoint
  storage_account_access_key = var.sql_audit_storage_access_key
  retention_in_days          = var.sql_audit_retention_days
}

# Variables
variable "sql_server_name" {
  description = "Name of the SQL Server"
  type        = string
}

variable "sql_database_name" {
  description = "Name of the SQL Database"
  type        = string
}

variable "sql_location" {
  description = "Azure region"
  type        = string
}

variable "sql_resource_group_name" {
  description = "Resource group name"
  type        = string
}

variable "sql_admin_username" {
  description = "SQL Server administrator username"
  type        = string
  sensitive   = true
}

variable "sql_admin_password" {
  description = "SQL Server administrator password"
  type        = string
  sensitive   = true
}

variable "sql_aad_admin_login" {
  description = "Azure AD admin login name"
  type        = string
  default     = null
}

variable "sql_aad_admin_object_id" {
  description = "Azure AD admin object ID"
  type        = string
  default     = null
}

variable "sql_database_sku_name" {
  description = "SQL Database SKU"
  type        = string
  default     = "Basic"
}

variable "sql_database_max_size_gb" {
  description = "SQL Database max size in GB"
  type        = number
  default     = 2
}

variable "sql_zone_redundant" {
  description = "Enable zone redundancy"
  type        = bool
  default     = false
}

variable "sql_public_network_access_enabled" {
  description = "Enable public network access"
  type        = bool
  default     = true
}

variable "sql_enable_auditing" {
  description = "Enable database auditing"
  type        = bool
  default     = false
}

variable "sql_audit_storage_endpoint" {
  description = "Storage endpoint for audit logs"
  type        = string
  default     = null
}

variable "sql_audit_storage_access_key" {
  description = "Storage access key for audit logs"
  type        = string
  sensitive   = true
  default     = null
}

variable "sql_audit_retention_days" {
  description = "Audit log retention in days"
  type        = number
  default     = 90
}

variable "sql_tags" {
  description = "Tags"
  type        = map(string)
  default     = {}
}

