# Root Variables for CardiTrack Infrastructure
# These are provided via environment-specific tfvars files

# Backend Configuration Variables
variable "backend_resource_group_name" {
  description = "Resource group name for Terraform state storage"
  type        = string
}

variable "backend_storage_account_name" {
  description = "Storage account name for Terraform state"
  type        = string
}

variable "backend_container_name" {
  description = "Container name for Terraform state"
  type        = string
}

variable "backend_key" {
  description = "State file key/path"
  type        = string
}

variable "environment" {
  description = "Environment name (dev, staging, prod)"
  type        = string
  validation {
    condition     = contains(["dev", "staging", "prod"], var.environment)
    error_message = "Environment must be dev, staging, or prod."
  }
}

variable "location" {
  description = "Azure region for resources"
  type        = string
  default     = "eastus"
}

variable "project_name" {
  description = "Project name for resource naming"
  type        = string
  default     = "carditrack"
}

# SQL Server Configuration
variable "sql_admin_username" {
  description = "SQL Server administrator username"
  type        = string
  sensitive   = true
}

variable "sql_admin_password" {
  description = "SQL Server administrator password (must meet complexity requirements)"
  type        = string
  sensitive   = true
  validation {
    condition     = length(var.sql_admin_password) >= 12
    error_message = "SQL admin password must be at least 12 characters long."
  }
}

variable "sql_aad_admin_login" {
  description = "Azure AD admin login name for SQL Server (production only)"
  type        = string
  default     = null
}

variable "sql_aad_admin_object_id" {
  description = "Azure AD admin object ID for SQL Server (production only)"
  type        = string
  default     = null
}

# App Service Configuration
variable "app_service_sku" {
  description = "App Service Plan SKU (B1 for dev, P1v2 for prod)"
  type        = string
  default     = "B1"
}

variable "app_service_always_on" {
  description = "Keep App Service always on"
  type        = bool
  default     = false
}

# SQL Database Configuration
variable "sql_database_sku" {
  description = "SQL Database SKU (Basic for dev, S2 for prod)"
  type        = string
  default     = "Basic"
}

variable "sql_database_max_size_gb" {
  description = "SQL Database max size in GB"
  type        = number
  default     = 2
}

# Storage Configuration
variable "storage_replication_type" {
  description = "Storage account replication type (LRS for dev, GRS for prod)"
  type        = string
  default     = "LRS"
}

variable "storage_enable_infrastructure_encryption" {
  description = "Enable infrastructure encryption for storage"
  type        = bool
  default     = false
}

# Key Vault Configuration
variable "key_vault_sku" {
  description = "Key Vault SKU (standard for dev, premium for prod)"
  type        = string
  default     = "standard"
}

variable "key_vault_soft_delete_retention_days" {
  description = "Key Vault soft delete retention in days"
  type        = number
  default     = 7
}

variable "key_vault_purge_protection_enabled" {
  description = "Enable Key Vault purge protection"
  type        = bool
  default     = false
}

# Application Insights Configuration
variable "app_insights_retention_days" {
  description = "Application Insights retention in days"
  type        = number
  default     = 30
}

# SignalR Configuration
variable "enable_signalr" {
  description = "Enable SignalR Service (production only)"
  type        = bool
  default     = false
}

variable "signalr_sku_name" {
  description = "SignalR Service SKU"
  type        = string
  default     = "Standard_S1"
}

# HIPAA Compliance Configuration
variable "enable_hipaa_compliance" {
  description = "Enable HIPAA compliance features (auditing, encryption, retention)"
  type        = bool
  default     = false
}

variable "sql_audit_retention_days" {
  description = "SQL audit log retention in days (90 for HIPAA)"
  type        = number
  default     = 90
}

# Tags
variable "additional_tags" {
  description = "Additional tags to apply to all resources"
  type        = map(string)
  default     = {}
}
