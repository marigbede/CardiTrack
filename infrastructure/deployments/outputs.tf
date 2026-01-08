# Deployments Module Outputs
# Exposes outputs from all deployment resources

# Resource Group Outputs
output "resource_group_name" {
  description = "Name of the resource group"
  value       = azurerm_resource_group.main.name
}

output "resource_group_location" {
  description = "Location of the resource group"
  value       = azurerm_resource_group.main.location
}

output "resource_group_id" {
  description = "ID of the resource group"
  value       = azurerm_resource_group.main.id
}

# Storage Account Outputs
output "storage_account_id" {
  description = "ID of the storage account"
  value       = azurerm_storage_account.main.id
}

output "storage_account_name" {
  description = "Name of the storage account"
  value       = azurerm_storage_account.main.name
}

output "storage_primary_blob_endpoint" {
  description = "Primary blob endpoint"
  value       = azurerm_storage_account.main.primary_blob_endpoint
}

output "storage_primary_access_key" {
  description = "Primary access key"
  value       = azurerm_storage_account.main.primary_access_key
  sensitive   = true
}

# Application Insights Outputs
output "app_insights_id" {
  description = "ID of Application Insights"
  value       = azurerm_application_insights.main.id
}

output "app_insights_instrumentation_key" {
  description = "Instrumentation key"
  value       = azurerm_application_insights.main.instrumentation_key
  sensitive   = true
}

output "app_insights_connection_string" {
  description = "Connection string"
  value       = azurerm_application_insights.main.connection_string
  sensitive   = true
}

# App Service Outputs
output "app_service_plan_id" {
  description = "ID of the App Service Plan"
  value       = azurerm_service_plan.main.id
}

output "api_app_name" {
  description = "Name of the API App"
  value       = azurerm_linux_web_app.api.name
}

output "api_app_url" {
  description = "URL of the API App"
  value       = "https://${azurerm_linux_web_app.api.default_hostname}"
}

output "api_app_hostname" {
  description = "Default hostname of the API App"
  value       = azurerm_linux_web_app.api.default_hostname
}

output "web_app_name" {
  description = "Name of the Web App"
  value       = azurerm_linux_web_app.web.name
}

output "web_app_url" {
  description = "URL of the Web App"
  value       = "https://${azurerm_linux_web_app.web.default_hostname}"
}

output "web_app_hostname" {
  description = "Default hostname of the Web App"
  value       = azurerm_linux_web_app.web.default_hostname
}

# SQL Server and Database Outputs
output "sql_server_id" {
  description = "ID of the SQL Server"
  value       = azurerm_mssql_server.main.id
}

output "sql_server_fqdn" {
  description = "FQDN of the SQL Server"
  value       = azurerm_mssql_server.main.fully_qualified_domain_name
}

output "sql_database_id" {
  description = "ID of the SQL Database"
  value       = azurerm_mssql_database.main.id
}

output "sql_database_name" {
  description = "Name of the SQL Database"
  value       = azurerm_mssql_database.main.name
}

# Key Vault Outputs
output "key_vault_id" {
  description = "ID of the Key Vault"
  value       = azurerm_key_vault.main.id
}

output "key_vault_name" {
  description = "Name of the Key Vault"
  value       = azurerm_key_vault.main.name
}

output "key_vault_uri" {
  description = "URI of the Key Vault"
  value       = azurerm_key_vault.main.vault_uri
}

# SignalR Service Outputs (optional)
output "signalr_id" {
  description = "ID of SignalR Service"
  value       = var.signalr_capacity > 0 ? azurerm_signalr_service.main[0].id : null
}

output "signalr_hostname" {
  description = "Hostname of SignalR Service"
  value       = var.signalr_capacity > 0 ? azurerm_signalr_service.main[0].hostname : null
}

output "signalr_primary_connection_string" {
  description = "Primary connection string"
  value       = var.signalr_capacity > 0 ? azurerm_signalr_service.main[0].primary_connection_string : null
  sensitive   = true
}
