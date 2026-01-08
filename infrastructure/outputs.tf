# Root Outputs for CardiTrack Infrastructure

output "resource_group_name" {
  description = "Name of the resource group"
  value       = module.deployments.resource_group_name
}

output "resource_group_location" {
  description = "Location of the resource group"
  value       = module.deployments.resource_group_location
}

output "api_app_url" {
  description = "URL of the API App Service"
  value       = module.deployments.api_app_url
}

output "api_app_name" {
  description = "Name of the API App Service"
  value       = module.deployments.api_app_name
}

output "web_app_url" {
  description = "URL of the Web App Service"
  value       = module.deployments.web_app_url
}

output "web_app_name" {
  description = "Name of the Web App Service"
  value       = module.deployments.web_app_name
}

output "sql_server_fqdn" {
  description = "Fully qualified domain name of the SQL Server"
  value       = module.deployments.sql_server_fqdn
}

output "sql_server_name" {
  description = "Name of the SQL Server"
  value       = module.deployments.sql_server_name
}

output "sql_database_name" {
  description = "Name of the SQL Database"
  value       = module.deployments.sql_database_name
}

output "storage_account_name" {
  description = "Name of the storage account"
  value       = module.deployments.storage_account_name
}

output "storage_account_primary_blob_endpoint" {
  description = "Primary blob endpoint of the storage account"
  value       = module.deployments.storage_primary_blob_endpoint
}

output "key_vault_uri" {
  description = "URI of the Key Vault"
  value       = module.deployments.key_vault_uri
}

output "key_vault_name" {
  description = "Name of the Key Vault"
  value       = module.deployments.key_vault_name
}

output "application_insights_instrumentation_key" {
  description = "Application Insights instrumentation key"
  value       = module.deployments.app_insights_instrumentation_key
  sensitive   = true
}

output "application_insights_connection_string" {
  description = "Application Insights connection string"
  value       = module.deployments.app_insights_connection_string
  sensitive   = true
}

output "signalr_hostname" {
  description = "SignalR Service hostname (if enabled)"
  value       = module.deployments.signalr_hostname
}

output "signalr_primary_connection_string" {
  description = "SignalR Service primary connection string (if enabled)"
  value       = module.deployments.signalr_primary_connection_string
  sensitive   = true
}
