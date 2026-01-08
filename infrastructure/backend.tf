# Backend configuration for storing Terraform state in Azure Storage
# Configuration is provided via tfvars files
#
# Usage:
#   terraform init -backend-config="resource_group_name=<value>" -backend-config="storage_account_name=<value>" -backend-config="container_name=<value>" -backend-config="key=<value>"
#   Or set via environment variables before init:
#   export TF_CLI_ARGS_init="-backend-config='resource_group_name=<value>' -backend-config='storage_account_name=<value>' -backend-config='container_name=<value>' -backend-config='key=<value>'"

terraform {
  backend "azurerm" {
    # Backend configuration must be provided during terraform init
    # Values come from environment-specific tfvars:
    #   - backend_resource_group_name  -> resource_group_name
    #   - backend_storage_account_name -> storage_account_name
    #   - backend_container_name       -> container_name
    #   - backend_key                  -> key
  }
}
