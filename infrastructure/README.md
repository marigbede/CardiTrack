# CardiTrack Terraform Infrastructure

This directory contains Terraform configurations for provisioning Azure infrastructure for the CardiTrack platform.

## Structure

```
terraform/
├── environments/
│   ├── dev/              # Development environment
│   └── production/       # Production environment
├── modules/              # Reusable Terraform modules (future)
├── backend.tf            # Remote state configuration
├── providers.tf          # Azure provider configuration
└── versions.tf           # Terraform version constraints
```

## Prerequisites

1. **Install Terraform**
   - Download from https://www.terraform.io/downloads
   - Version: >= 1.0.0

2. **Azure CLI**
   - Install: https://docs.microsoft.com/en-us/cli/azure/install-azure-cli
   - Login: `az login`

3. **Azure Subscription**
   - Ensure you have owner or contributor access

## Getting Started

### 1. Configure Azure Authentication

```bash
# Login to Azure
az login

# Set the subscription (if you have multiple)
az account set --subscription "YOUR_SUBSCRIPTION_ID"

# Verify
az account show
```

### 2. Initialize Terraform Backend (Optional)

For production use, configure remote state storage:

```bash
# Create a storage account for Terraform state
az group create --name carditrack-tfstate-rg --location eastus
az storage account create --name carditracktfstate --resource-group carditrack-tfstate-rg --location eastus --sku Standard_LRS
az storage container create --name tfstate --account-name carditracktfstate

# Uncomment the backend configuration in backend.tf
```

### 3. Deploy Development Environment

```bash
cd environments/dev

# Copy the example variables file
cp terraform.tfvars.example terraform.tfvars

# Edit terraform.tfvars with your values
# IMPORTANT: Set strong passwords and never commit this file to git

# Initialize Terraform
terraform init

# Review the plan
terraform plan

# Apply the configuration
terraform apply
```

### 4. Deploy Production Environment

```bash
cd environments/production

# Copy the example variables file
cp terraform.tfvars.example terraform.tfvars

# Edit terraform.tfvars with your values
# IMPORTANT: Set very strong passwords and configure Azure AD admin

# Initialize Terraform
terraform init

# Review the plan
terraform plan

# Apply the configuration
terraform apply
```

## Environment Differences

### Development
- **App Service Plan**: B1 (Basic)
- **SQL Database**: Basic tier
- **Storage**: Locally redundant (LRS)
- **Key Vault**: Standard, no purge protection
- **Always On**: Disabled (cost savings)
- **Backups**: Limited retention

### Production
- **App Service Plan**: P1v2 (Premium)
- **SQL Database**: S2 (Standard) with 250GB
- **Storage**: Geo-redundant (GRS)
- **Key Vault**: Premium with HSM, purge protection enabled
- **Always On**: Enabled
- **Backups**: 90-day retention (HIPAA compliant)
- **SignalR**: Standard tier
- **Monitoring**: Enhanced with Application Insights

## HIPAA Compliance Features

The production environment includes:

- ✅ **Encryption at rest**: TDE on SQL Database, storage encryption
- ✅ **Encryption in transit**: TLS 1.2+ enforced
- ✅ **Audit logging**: SQL Database auditing enabled (90 days)
- ✅ **Key Vault**: Premium with HSM-backed keys
- ✅ **Purge protection**: Enabled on Key Vault
- ✅ **Data retention**: 90-day retention policies
- ✅ **Network security**: Key Vault network ACLs

## Outputs

After deployment, Terraform will output:

- **API URL**: Endpoint for the API application
- **Web URL**: Endpoint for the web dashboard
- **SQL Server FQDN**: Database connection string
- **Key Vault URI**: Secrets management endpoint
- **Application Insights Keys**: For monitoring (sensitive)

View outputs:
```bash
terraform output
```

## Destroy Environment

**WARNING**: This will delete all resources. Only use for dev/test environments.

```bash
terraform destroy
```

## Cost Estimates

### Development Environment
- App Service Plan (B1): ~$13/month
- SQL Database (Basic): ~$5/month
- Storage Account: ~$2/month
- Key Vault: ~$1/month
- Application Insights: ~$2/month
- **Total**: ~$25-30/month

### Production Environment
- App Service Plan (P1v2): ~$146/month
- SQL Database (S2): ~$75/month
- Storage Account (GRS): ~$10/month
- Key Vault (Premium): ~$5/month
- Application Insights: ~$10/month
- SignalR (Standard): ~$50/month
- **Total**: ~$300-350/month

## Troubleshooting

### Authentication Issues
```bash
# Re-login to Azure
az login
az account show
```

### State Lock Issues
```bash
# If using remote backend and state is locked
terraform force-unlock <LOCK_ID>
```

### Resource Name Conflicts
- Azure resource names must be globally unique
- Modify names in main.tf if needed

## Security Best Practices

1. **Never commit terraform.tfvars** to source control
2. Use **Azure Key Vault** for secrets in production
3. Enable **Multi-Factor Authentication** on Azure accounts
4. Rotate **SQL passwords** regularly
5. Review **Azure security recommendations** regularly
6. Enable **Azure Defender** for production resources

## Next Steps

After infrastructure is deployed:

1. Configure **CI/CD pipelines** (see `.github/workflows/`)
2. Deploy **application code** to App Services
3. Run **database migrations** with EF Core
4. Configure **custom domains** and SSL certificates
5. Set up **monitoring alerts** in Application Insights
6. Configure **device API credentials** (Fitbit, etc.)

## Support

For issues or questions, refer to:
- [Terraform Azure Provider Docs](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs)
- [Azure Documentation](https://docs.microsoft.com/en-us/azure/)
- Project documentation in `/docs/`
