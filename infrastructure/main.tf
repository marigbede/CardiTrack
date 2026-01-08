# CardiTrack Infrastructure - Main Orchestration
# This file orchestrates all deployment modules

locals {
  environment = var.environment
  location    = var.location

  common_tags = merge(
    {
      Environment = title(var.environment)
      Project     = "CardiTrack"
      ManagedBy   = "Terraform"
    },
    var.environment == "prod" ? { CostCenter = "Engineering" } : {},
    var.additional_tags
  )

  # Resource naming
  resource_group_name  = "${var.project_name}-${local.environment}-rg"
  app_service_plan_name = "${var.project_name}-${local.environment}-asp"
  api_app_name         = "${var.project_name}-${local.environment}-api"
  web_app_name         = "${var.project_name}-${local.environment}-web"
  sql_server_name      = "${var.project_name}-${local.environment}-sql"
  sql_database_name    = "${var.project_name}-${local.environment}-db"
  storage_account_name = "${var.project_name}${local.environment}st"
  key_vault_name       = "${var.project_name}-${local.environment}-kv"
  app_insights_name    = "${var.project_name}-${local.environment}-ai"
  signalr_name         = "${var.project_name}-${local.environment}-signalr"
}

# All deployments are managed through a single module
module "deployments" {
  source = "./deployments"

  # Resource Group
  resource_group_name = local.resource_group_name
  location            = local.location
  tags                = local.common_tags

  # Storage Account
  storage_account_name                      = local.storage_account_name
  storage_location                          = local.location
  storage_resource_group_name               = local.resource_group_name
  storage_replication_type                  = var.storage_replication_type
  storage_infrastructure_encryption_enabled = var.storage_enable_infrastructure_encryption
  storage_tags                              = local.common_tags

  # Application Insights
  app_insights_name              = local.app_insights_name
  monitoring_location            = local.location
  monitoring_resource_group_name = local.resource_group_name
  monitoring_retention_in_days   = var.app_insights_retention_days
  monitoring_tags                = local.common_tags

  # App Service Plan and Web Apps
  app_service_plan_name           = local.app_service_plan_name
  api_app_name                    = local.api_app_name
  web_app_name                    = local.web_app_name
  app_service_location            = local.location
  app_service_resource_group_name = local.resource_group_name
  app_service_sku_name            = var.app_service_sku
  app_service_always_on           = var.app_service_always_on
  app_service_http2_enabled       = var.environment == "prod"
  app_service_ftps_state          = var.environment == "prod" ? "Disabled" : "AllAllowed"
  app_service_health_check_path   = var.environment == "prod" ? "/health" : null
  app_service_https_only          = var.environment == "prod"

  api_app_settings = merge(
    {
      "ASPNETCORE_ENVIRONMENT" = title(var.environment)
    },
    var.environment == "prod" ? {
      "APPLICATIONINSIGHTS_CONNECTION_STRING"   = module.deployments.app_insights_connection_string
      "ApplicationInsights__InstrumentationKey" = module.deployments.app_insights_instrumentation_key
    } : {}
  )

  web_app_settings = merge(
    {
      "ASPNETCORE_ENVIRONMENT" = title(var.environment)
    },
    var.environment == "prod" ? {
      "APPLICATIONINSIGHTS_CONNECTION_STRING"   = module.deployments.app_insights_connection_string
      "ApplicationInsights__InstrumentationKey" = module.deployments.app_insights_instrumentation_key
    } : {}
  )

  app_service_tags = local.common_tags

  # Azure SQL Server and Database
  sql_server_name              = local.sql_server_name
  sql_database_name            = local.sql_database_name
  sql_location                 = local.location
  sql_resource_group_name      = local.resource_group_name
  sql_admin_username           = var.sql_admin_username
  sql_admin_password           = var.sql_admin_password
  sql_aad_admin_login          = var.sql_aad_admin_login
  sql_aad_admin_object_id      = var.sql_aad_admin_object_id
  sql_database_sku_name        = var.sql_database_sku
  sql_database_max_size_gb     = var.sql_database_max_size_gb
  sql_enable_auditing          = var.enable_hipaa_compliance
  sql_audit_storage_endpoint   = var.enable_hipaa_compliance ? module.deployments.storage_primary_blob_endpoint : null
  sql_audit_storage_access_key = var.enable_hipaa_compliance ? module.deployments.storage_primary_access_key : null
  sql_audit_retention_days     = var.sql_audit_retention_days
  sql_tags                     = local.common_tags

  # Key Vault
  key_vault_name                        = local.key_vault_name
  key_vault_location                    = local.location
  key_vault_resource_group_name         = local.resource_group_name
  key_vault_sku_name                    = var.key_vault_sku
  key_vault_soft_delete_retention_days  = var.key_vault_soft_delete_retention_days
  key_vault_purge_protection_enabled    = var.key_vault_purge_protection_enabled
  key_vault_enabled_for_disk_encryption = var.enable_hipaa_compliance
  key_vault_enabled_for_deployment      = var.enable_hipaa_compliance
  key_vault_enable_network_acls         = var.environment == "prod"
  key_vault_tags                        = local.common_tags

  # SignalR Service (Optional - Production only)
  signalr_name                = local.signalr_name
  signalr_location            = local.location
  signalr_resource_group_name = local.resource_group_name
  signalr_sku_name            = var.signalr_sku_name
  signalr_capacity            = var.enable_signalr ? 1 : 0
  signalr_allowed_origins     = var.enable_signalr ? ["https://${module.deployments.web_app_hostname}"] : []
  signalr_tags                = local.common_tags
}
