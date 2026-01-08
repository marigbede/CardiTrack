# CardiTrack Solution Structure

> **Multi-Device Elderly Health Monitoring Platform**
>
> A device-agnostic health monitoring system that supports Fitbit, Apple Watch, Garmin, Samsung, and other wearable devices with AI-powered pattern analysis for preventive elderly care.

---

## Directory Structure

```
CardiTrack/
│
├── src/
│   ├── Core/
│   │   ├── CardiTrack.Domain/
│   │   │   ├── Entities/
│   │   │   │   ├── User.cs
│   │   │   │   ├── Elder.cs
│   │   │   │   ├── FamilyMember.cs
│   │   │   │   ├── Device.cs                           # Wearable device entity
│   │   │   │   ├── DeviceConnection.cs                 # Device OAuth tokens
│   │   │   │   ├── ActivityLog.cs                      # Device-agnostic activity data
│   │   │   │   ├── PatternBaseline.cs
│   │   │   │   ├── Alert.cs
│   │   │   │   ├── AuditLog.cs
│   │   │   │   └── Subscription.cs
│   │   │   ├── Enums/
│   │   │   │   ├── FamilyRole.cs
│   │   │   │   ├── AlertSeverity.cs
│   │   │   │   ├── AlertType.cs
│   │   │   │   ├── SubscriptionTier.cs
│   │   │   │   ├── DeviceType.cs                       # Fitbit, AppleWatch, Garmin, etc.
│   │   │   │   ├── DeviceConnectionStatus.cs           # Connected, Disconnected, TokenExpired
│   │   │   │   └── DataSource.cs                       # Track which device provided data
│   │   │   ├── ValueObjects/
│   │   │   │   ├── DeviceTokens.cs                     # Generic token storage
│   │   │   │   ├── HeartRateData.cs
│   │   │   │   ├── SleepData.cs
│   │   │   │   ├── ActivityData.cs
│   │   │   │   ├── OxygenSaturationData.cs             # SpO2
│   │   │   │   ├── BloodPressureData.cs                # For future devices
│   │   │   │   └── TemperatureData.cs                  # For future devices
│   │   │   ├── Interfaces/
│   │   │   │   └── IEntity.cs
│   │   │   └── CardiTrack.Domain.csproj
│   │   │
│   │   └── CardiTrack.Application/
│   │       ├── Interfaces/
│   │       │   ├── Services/
│   │       │   │   ├── Devices/
│   │       │   │   │   ├── IDeviceService.cs           # Generic device interface
│   │       │   │   │   ├── IDeviceOAuthService.cs      # OAuth abstraction
│   │       │   │   │   ├── IDeviceDataSyncService.cs   # Sync abstraction
│   │       │   │   │   └── IDeviceCapabilitiesService.cs # Feature detection
│   │       │   │   ├── IAlertService.cs
│   │       │   │   ├── IPatternAnalysisService.cs
│   │       │   │   ├── IEmailService.cs
│   │       │   │   ├── ISmsService.cs
│   │       │   │   ├── IEncryptionService.cs
│   │       │   │   └── IAuditService.cs
│   │       │   ├── Repositories/
│   │       │   │   ├── IUserRepository.cs
│   │       │   │   ├── IElderRepository.cs
│   │       │   │   ├── IDeviceRepository.cs
│   │       │   │   ├── IDeviceConnectionRepository.cs
│   │       │   │   ├── IActivityLogRepository.cs
│   │       │   │   ├── IPatternBaselineRepository.cs
│   │       │   │   ├── IAlertRepository.cs
│   │       │   │   └── IAuditLogRepository.cs
│   │       │   └── IUnitOfWork.cs
│   │       ├── DTOs/
│   │       │   ├── Requests/
│   │       │   │   ├── CreateElderRequest.cs
│   │       │   │   ├── ConnectDeviceRequest.cs         # Generic device connection
│   │       │   │   ├── UpdateAlertSettingsRequest.cs
│   │       │   │   └── RegisterUserRequest.cs
│   │       │   ├── Responses/
│   │       │   │   ├── ElderDashboardResponse.cs
│   │       │   │   ├── HealthSummaryResponse.cs
│   │       │   │   ├── AlertResponse.cs
│   │       │   │   ├── DeviceConnectionResponse.cs     # Generic
│   │       │   │   └── SupportedDevicesResponse.cs     # List available devices
│   │       │   └── External/
│   │       │       ├── Fitbit/
│   │       │       │   ├── FitbitActivityResponse.cs
│   │       │       │   ├── FitbitHeartRateResponse.cs
│   │       │       │   ├── FitbitSleepResponse.cs
│   │       │       │   └── FitbitTokenResponse.cs
│   │       │       ├── AppleHealth/                    # Apple Watch support
│   │       │       │   ├── AppleHealthActivityResponse.cs
│   │       │       │   ├── AppleHealthHeartRateResponse.cs
│   │       │       │   └── AppleHealthSleepResponse.cs
│   │       │       ├── Garmin/                         # Garmin support
│   │       │       │   ├── GarminActivityResponse.cs
│   │       │       │   ├── GarminHeartRateResponse.cs
│   │       │       │   └── GarminSleepResponse.cs
│   │       │       ├── Samsung/                        # Samsung Health
│   │       │       │   ├── SamsungHealthActivityResponse.cs
│   │       │       │   └── SamsungHealthHeartRateResponse.cs
│   │       │       └── Withings/                       # Withings devices
│   │       │           ├── WithingsActivityResponse.cs
│   │       │           └── WithingsBloodPressureResponse.cs
│   │       ├── Services/
│   │       │   ├── ElderService.cs
│   │       │   ├── FamilyMemberService.cs
│   │       │   ├── DashboardService.cs
│   │       │   ├── SubscriptionService.cs
│   │       │   └── DeviceManagementService.cs          # Manage device connections
│   │       ├── Validators/
│   │       │   ├── CreateElderRequestValidator.cs
│   │       │   ├── RegisterUserRequestValidator.cs
│   │       │   ├── ConnectDeviceRequestValidator.cs
│   │       │   └── UpdateAlertSettingsValidator.cs
│   │       ├── Exceptions/
│   │       │   ├── DeviceConnectionException.cs        # Generic device exception
│   │       │   ├── AlertException.cs
│   │       │   ├── UnauthorizedAccessException.cs
│   │       │   └── PatternAnalysisException.cs
│   │       ├── Mappings/
│   │       │   ├── AutoMapperProfile.cs
│   │       │   └── DeviceDataMapperProfile.cs          # Map device-specific to generic
│   │       └── CardiTrack.Application.csproj
│   │
│   ├── Infrastructure/
│   │   ├── CardiTrack.Infrastructure/
│   │   │   ├── Persistence/
│   │   │   │   ├── CardiTrackDbContext.cs
│   │   │   │   ├── Configurations/
│   │   │   │   │   ├── UserConfiguration.cs
│   │   │   │   │   ├── ElderConfiguration.cs
│   │   │   │   │   ├── DeviceConfiguration.cs
│   │   │   │   │   ├── DeviceConnectionConfiguration.cs
│   │   │   │   │   ├── ActivityLogConfiguration.cs
│   │   │   │   │   ├── PatternBaselineConfiguration.cs
│   │   │   │   │   └── AlertConfiguration.cs
│   │   │   │   ├── Repositories/
│   │   │   │   │   ├── BaseRepository.cs
│   │   │   │   │   ├── UserRepository.cs
│   │   │   │   │   ├── ElderRepository.cs
│   │   │   │   │   ├── DeviceRepository.cs
│   │   │   │   │   ├── DeviceConnectionRepository.cs
│   │   │   │   │   ├── ActivityLogRepository.cs
│   │   │   │   │   ├── PatternBaselineRepository.cs
│   │   │   │   │   ├── AlertRepository.cs
│   │   │   │   │   └── AuditLogRepository.cs
│   │   │   │   ├── Migrations/
│   │   │   │   │   └── (EF Core migrations)
│   │   │   │   └── UnitOfWork.cs
│   │   │   ├── ExternalServices/
│   │   │   │   ├── Devices/
│   │   │   │   │   ├── Abstractions/
│   │   │   │   │   │   ├── IDeviceApiClient.cs         # Interface for all devices
│   │   │   │   │   │   ├── IDeviceAuthHandler.cs       # OAuth abstraction
│   │   │   │   │   │   └── IDeviceDataAdapter.cs       # Normalize device data
│   │   │   │   │   ├── Fitbit/
│   │   │   │   │   │   ├── FitbitService.cs
│   │   │   │   │   │   ├── FitbitApiClient.cs
│   │   │   │   │   │   ├── FitbitOAuthHandler.cs
│   │   │   │   │   │   └── FitbitDataAdapter.cs        # Convert Fitbit → Generic
│   │   │   │   │   ├── AppleHealth/
│   │   │   │   │   │   ├── AppleHealthService.cs
│   │   │   │   │   │   ├── AppleHealthApiClient.cs
│   │   │   │   │   │   ├── AppleHealthOAuthHandler.cs
│   │   │   │   │   │   └── AppleHealthDataAdapter.cs
│   │   │   │   │   ├── Garmin/
│   │   │   │   │   │   ├── GarminService.cs
│   │   │   │   │   │   ├── GarminApiClient.cs
│   │   │   │   │   │   ├── GarminOAuthHandler.cs
│   │   │   │   │   │   └── GarminDataAdapter.cs
│   │   │   │   │   ├── Samsung/
│   │   │   │   │   │   ├── SamsungHealthService.cs
│   │   │   │   │   │   ├── SamsungHealthApiClient.cs
│   │   │   │   │   │   ├── SamsungHealthOAuthHandler.cs
│   │   │   │   │   │   └── SamsungHealthDataAdapter.cs
│   │   │   │   │   ├── Withings/
│   │   │   │   │   │   ├── WithingsService.cs
│   │   │   │   │   │   ├── WithingsApiClient.cs
│   │   │   │   │   │   ├── WithingsOAuthHandler.cs
│   │   │   │   │   │   └── WithingsDataAdapter.cs
│   │   │   │   │   ├── Oura/                           # Oura Ring
│   │   │   │   │   │   ├── OuraService.cs
│   │   │   │   │   │   ├── OuraApiClient.cs
│   │   │   │   │   │   ├── OuraOAuthHandler.cs
│   │   │   │   │   │   └── OuraDataAdapter.cs
│   │   │   │   │   ├── Whoop/                          # Whoop band
│   │   │   │   │   │   ├── WhoopService.cs
│   │   │   │   │   │   ├── WhoopApiClient.cs
│   │   │   │   │   │   ├── WhoopOAuthHandler.cs
│   │   │   │   │   │   └── WhoopDataAdapter.cs
│   │   │   │   │   ├── Factory/
│   │   │   │   │   │   ├── DeviceServiceFactory.cs     # Create device service by type
│   │   │   │   │   │   └── DeviceDataAdapterFactory.cs # Get adapter by device type
│   │   │   │   │   └── Common/
│   │   │   │   │       ├── DeviceDataNormalizer.cs     # Normalize all device data
│   │   │   │   │       └── DeviceCapabilitiesRegistry.cs # Track device capabilities
│   │   │   │   ├── Notifications/
│   │   │   │   │   ├── TwilioSmsService.cs
│   │   │   │   │   └── SendGridEmailService.cs
│   │   │   │   └── MachineLearning/
│   │   │   │       ├── PatternAnalysisService.cs
│   │   │   │       ├── AnomalyDetectionService.cs
│   │   │   │       ├── BaselineCalculationService.cs
│   │   │   │       └── Models/
│   │   │   │           ├── AnomalyDetectionModel.cs
│   │   │   │           └── PatternPredictionModel.cs
│   │   │   ├── Security/
│   │   │   │   ├── EncryptionService.cs
│   │   │   │   ├── AuditService.cs
│   │   │   │   └── TokenService.cs
│   │   │   ├── BackgroundJobs/
│   │   │   │   ├── DeviceSyncJob.cs                    # Generic device sync
│   │   │   │   ├── PatternAnalysisJob.cs
│   │   │   │   ├── TokenRefreshJob.cs                  # Refresh all device tokens
│   │   │   │   ├── BaselineRecalculationJob.cs
│   │   │   │   └── AlertProcessingJob.cs
│   │   │   ├── Caching/
│   │   │   │   └── CacheService.cs
│   │   │   └── CardiTrack.Infrastructure.csproj
│   │   │
│   │   └── CardiTrack.Shared/
│   │       ├── Constants/
│   │       │   ├── AlertConstants.cs
│   │       │   ├── DeviceConstants.cs                  # Device types, capabilities
│   │       │   └── HipaaConstants.cs
│   │       ├── Helpers/
│   │       │   ├── DateTimeHelper.cs
│   │       │   ├── ValidationHelper.cs
│   │       │   ├── HealthMetricsHelper.cs
│   │       │   └── DeviceHelper.cs                     # Device utilities
│   │       ├── Extensions/
│   │       │   ├── DateTimeExtensions.cs
│   │       │   ├── StringExtensions.cs
│   │       │   └── EnumExtensions.cs
│   │       └── CardiTrack.Shared.csproj
│   │
│   ├── Presentation/
│   │   ├── CardiTrack.API/
│   │   │   ├── Controllers/
│   │   │   │   ├── AuthController.cs
│   │   │   │   ├── EldersController.cs
│   │   │   │   ├── FamilyMembersController.cs
│   │   │   │   ├── DashboardController.cs
│   │   │   │   ├── AlertsController.cs
│   │   │   │   ├── DevicesController.cs               # Device management
│   │   │   │   ├── SubscriptionsController.cs
│   │   │   │   └── Webhooks/                          # Device webhooks
│   │   │   │       ├── FitbitWebhookController.cs
│   │   │   │       ├── GarminWebhookController.cs
│   │   │   │       └── WithingsWebhookController.cs
│   │   │   ├── Middleware/
│   │   │   │   ├── ErrorHandlingMiddleware.cs
│   │   │   │   ├── AuditLoggingMiddleware.cs
│   │   │   │   ├── RateLimitingMiddleware.cs
│   │   │   │   └── HipaaComplianceMiddleware.cs
│   │   │   ├── Filters/
│   │   │   │   ├── ValidateModelAttribute.cs
│   │   │   │   └── AuthorizeElderAccessAttribute.cs
│   │   │   ├── Extensions/
│   │   │   │   ├── ServiceCollectionExtensions.cs
│   │   │   │   └── ApplicationBuilderExtensions.cs
│   │   │   ├── Program.cs
│   │   │   ├── appsettings.json
│   │   │   ├── appsettings.Development.json
│   │   │   ├── appsettings.Production.json
│   │   │   └── CardiTrack.API.csproj
│   │   │
│   │   ├── CardiTrack.Web/
│   │   │   ├── Pages/
│   │   │   │   ├── Index.razor
│   │   │   │   ├── Dashboard.razor
│   │   │   │   ├── ElderProfile.razor
│   │   │   │   ├── Alerts.razor
│   │   │   │   ├── Settings.razor
│   │   │   │   ├── Devices/                           # Device management pages
│   │   │   │   │   ├── ConnectDevice.razor
│   │   │   │   │   ├── DeviceList.razor
│   │   │   │   │   └── DeviceSettings.razor
│   │   │   │   └── Account/
│   │   │   │       ├── Login.razor
│   │   │   │       ├── Register.razor
│   │   │   │       └── Profile.razor
│   │   │   ├── Shared/
│   │   │   │   ├── MainLayout.razor
│   │   │   │   ├── NavMenu.razor
│   │   │   │   ├── AlertCard.razor
│   │   │   │   ├── HealthMetricCard.razor
│   │   │   │   ├── DeviceCard.razor                   # Show connected devices
│   │   │   │   └── LoadingSpinner.razor
│   │   │   ├── Components/
│   │   │   │   ├── ActivityChart.razor
│   │   │   │   ├── HeartRateChart.razor
│   │   │   │   ├── SleepQualityChart.razor
│   │   │   │   ├── TrendIndicator.razor
│   │   │   │   └── DeviceSelector.razor               # Choose device to connect
│   │   │   ├── Services/
│   │   │   │   ├── ApiClient.cs
│   │   │   │   ├── AuthStateProvider.cs
│   │   │   │   └── SignalRService.cs
│   │   │   ├── wwwroot/
│   │   │   │   ├── css/
│   │   │   │   ├── js/
│   │   │   │   └── images/
│   │   │   │       └── devices/                       # Device logos/icons
│   │   │   │           ├── fitbit.svg
│   │   │   │           ├── apple-watch.svg
│   │   │   │           ├── garmin.svg
│   │   │   │           ├── samsung.svg
│   │   │   │           ├── withings.svg
│   │   │   │           ├── oura.svg
│   │   │   │           └── whoop.svg
│   │   │   ├── Program.cs
│   │   │   ├── App.razor
│   │   │   ├── _Imports.razor
│   │   │   ├── appsettings.json
│   │   │   └── CardiTrack.Web.csproj
│   │   │
│   │   └── CardiTrack.Mobile/
│   │       ├── Platforms/
│   │       │   ├── Android/
│   │       │   ├── iOS/
│   │       │   │   └── HealthKitService.cs            # iOS HealthKit integration
│   │       │   └── Windows/
│   │       ├── Views/
│   │       │   ├── DashboardPage.xaml
│   │       │   ├── ElderListPage.xaml
│   │       │   ├── AlertsPage.xaml
│   │       │   ├── SettingsPage.xaml
│   │       │   ├── DevicesPage.xaml
│   │       │   └── LoginPage.xaml
│   │       ├── ViewModels/
│   │       │   ├── BaseViewModel.cs
│   │       │   ├── DashboardViewModel.cs
│   │       │   ├── ElderListViewModel.cs
│   │       │   ├── AlertsViewModel.cs
│   │       │   ├── DevicesViewModel.cs
│   │       │   └── SettingsViewModel.cs
│   │       ├── Services/
│   │       │   ├── MobileApiClient.cs
│   │       │   ├── NotificationService.cs
│   │       │   └── SecureStorageService.cs
│   │       ├── MauiProgram.cs
│   │       ├── App.xaml
│   │       ├── AppShell.xaml
│   │       └── CardiTrack.Mobile.csproj
│   │
│   └── Functions/
│       └── CardiTrack.Functions/
│           ├── DeviceSyncFunction.cs                   # Generic device sync
│           ├── PatternAnalysisFunction.cs
│           ├── AlertProcessingFunction.cs
│           ├── TokenRefreshFunction.cs
│           ├── WebhookHandlers/                        # Handle device webhooks
│           │   ├── FitbitWebhookFunction.cs
│           │   ├── GarminWebhookFunction.cs
│           │   └── WithingsWebhookFunction.cs
│           ├── host.json
│           ├── local.settings.json
│           └── CardiTrack.Functions.csproj
│
├── infrastructure/
│   ├── terraform/
│   │   ├── environments/
│   │   │   ├── dev/
│   │   │   │   ├── main.tf
│   │   │   │   ├── variables.tf
│   │   │   │   ├── outputs.tf
│   │   │   │   └── terraform.tfvars
│   │   │   ├── staging/
│   │   │   │   ├── main.tf
│   │   │   │   ├── variables.tf
│   │   │   │   ├── outputs.tf
│   │   │   │   └── terraform.tfvars
│   │   │   └── production/
│   │   │       ├── main.tf
│   │   │       ├── variables.tf
│   │   │       ├── outputs.tf
│   │   │       └── terraform.tfvars
│   │   ├── modules/
│   │   │   ├── app-service/
│   │   │   │   ├── main.tf
│   │   │   │   ├── variables.tf
│   │   │   │   └── outputs.tf
│   │   │   ├── sql-database/
│   │   │   │   ├── main.tf
│   │   │   │   ├── variables.tf
│   │   │   │   └── outputs.tf
│   │   │   ├── function-app/
│   │   │   │   ├── main.tf
│   │   │   │   ├── variables.tf
│   │   │   │   └── outputs.tf
│   │   │   ├── storage/
│   │   │   │   ├── main.tf
│   │   │   │   ├── variables.tf
│   │   │   │   └── outputs.tf
│   │   │   ├── key-vault/
│   │   │   │   ├── main.tf
│   │   │   │   ├── variables.tf
│   │   │   │   └── outputs.tf
│   │   │   ├── application-insights/
│   │   │   │   ├── main.tf
│   │   │   │   ├── variables.tf
│   │   │   │   └── outputs.tf
│   │   │   ├── signalr/
│   │   │   │   ├── main.tf
│   │   │   │   ├── variables.tf
│   │   │   │   └── outputs.tf
│   │   │   └── networking/
│   │   │       ├── main.tf
│   │   │       ├── variables.tf
│   │   │       └── outputs.tf
│   │   ├── backend.tf
│   │   ├── providers.tf
│   │   ├── versions.tf
│   │   └── README.md
│   │
│   ├── scripts/
│   │   ├── init-terraform.sh
│   │   ├── plan-infrastructure.sh
│   │   ├── apply-infrastructure.sh
│   │   └── destroy-infrastructure.sh
│   │
│   └── azure-pipelines/
│       ├── infrastructure-pipeline.yml
│       └── terraform-validate.yml
│
├── tests/
│   ├── CardiTrack.UnitTests/
│   │   ├── Domain/
│   │   ├── Application/
│   │   │   ├── Services/
│   │   │   │   └── DeviceServices/                    # Test device services
│   │   │   └── Validators/
│   │   ├── Infrastructure/
│   │   │   ├── Services/
│   │   │   ├── Devices/                               # Test device adapters
│   │   │   │   ├── FitbitAdapterTests.cs
│   │   │   │   ├── AppleHealthAdapterTests.cs
│   │   │   │   └── GarminAdapterTests.cs
│   │   │   └── Repositories/
│   │   └── CardiTrack.UnitTests.csproj
│   │
│   ├── CardiTrack.IntegrationTests/
│   │   ├── API/
│   │   │   ├── Controllers/
│   │   │   └── Middleware/
│   │   ├── Infrastructure/
│   │   │   ├── Devices/                               # Test device integrations
│   │   │   │   ├── FitbitIntegrationTests.cs
│   │   │   │   ├── GarminIntegrationTests.cs
│   │   │   │   └── AppleHealthIntegrationTests.cs
│   │   │   └── Persistence/
│   │   ├── TestFixtures/
│   │   └── CardiTrack.IntegrationTests.csproj
│   │
│   └── CardiTrack.E2ETests/
│       ├── Scenarios/
│       │   ├── OnboardingTests.cs
│       │   ├── DashboardTests.cs
│       │   ├── DeviceConnectionTests.cs
│       │   └── AlertFlowTests.cs
│       └── CardiTrack.E2ETests.csproj
│
├── docs/
│   ├── architecture/
│   │   ├── system-architecture.md
│   │   ├── database-schema.md
│   │   ├── device-integration-guide.md
│   │   ├── multi-device-strategy.md
│   │   └── api-specification.md
│   ├── devices/                                        # Device-specific docs
│   │   ├── fitbit-integration.md
│   │   ├── apple-health-integration.md
│   │   ├── garmin-integration.md
│   │   ├── samsung-health-integration.md
│   │   ├── withings-integration.md
│   │   ├── oura-integration.md
│   │   ├── whoop-integration.md
│   │   └── adding-new-device.md                        # How to add new device
│   ├── compliance/
│   │   ├── hipaa-requirements.md
│   │   ├── security-policies.md
│   │   └── privacy-policy.md
│   ├── deployment/
│   │   ├── azure-setup.md
│   │   ├── terraform-guide.md
│   │   └── ci-cd-pipeline.md
│   └── developer-guide/
│       ├── getting-started.md
│       ├── coding-standards.md
│       ├── device-adapter-pattern.md
│       └── testing-guide.md
│
├── scripts/
│   ├── setup-dev-environment.ps1
│   ├── create-migration.ps1
│   ├── seed-test-data.sql
│   └── deploy-to-azure.ps1
│
├── .github/
│   └── workflows/
│       ├── ci-build.yml
│       ├── cd-deploy.yml
│       ├── terraform-plan.yml
│       └── security-scan.yml
│
├── docker/
│   ├── Dockerfile.api
│   ├── Dockerfile.web
│   ├── docker-compose.yml
│   └── docker-compose.override.yml
│
├── .gitignore
├── .editorconfig
├── .terraformignore
├── CardiTrack.sln
├── README.md
├── LICENSE
└── CHANGELOG.md
```

---

## Architecture Overview

### Clean Architecture Layers

```
┌─────────────────────────────────────────────┐
│         Presentation Layer                  │
│  (API, Web, Mobile, Functions)             │
└─────────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────────┐
│       Application Layer                     │
│  (Use Cases, DTOs, Interfaces)             │
└─────────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────────┐
│         Domain Layer                        │
│  (Entities, Value Objects, Enums)          │
└─────────────────────────────────────────────┘
                    ↑
┌─────────────────────────────────────────────┐
│      Infrastructure Layer                   │
│  (EF Core, External APIs, Jobs, ML)        │
└─────────────────────────────────────────────┘
```

### Multi-Device Architecture

The solution uses the **Adapter Pattern** to support multiple wearable devices:

```
Device APIs (Fitbit, Apple, Garmin, Samsung, etc.)
                    ↓
        Device-Specific Adapters
                    ↓
        Normalized Health Data
                    ↓
        Pattern Analysis Engine
                    ↓
            Family Alerts
```

---

## Key Design Patterns

### 1. Adapter Pattern (Device Integration)
Each wearable device has a dedicated adapter that converts device-specific data formats to a common, normalized format.

**Example:**
```csharp
public interface IDeviceDataAdapter
{
    Task<ActivityData> NormalizeActivityData(object rawData);
    Task<HeartRateData> NormalizeHeartRateData(object rawData);
    Task<SleepData> NormalizeSleepData(object rawData);
    DeviceCapabilities GetCapabilities();
}
```

### 2. Factory Pattern (Device Service Creation)
```csharp
public class DeviceServiceFactory
{
    public IDeviceApiClient CreateDeviceService(DeviceType deviceType)
    {
        return deviceType switch
        {
            DeviceType.Fitbit => new FitbitApiClient(),
            DeviceType.AppleWatch => new AppleHealthApiClient(),
            DeviceType.Garmin => new GarminApiClient(),
            // ... more devices
        };
    }
}
```

### 3. Repository Pattern (Data Access)
All database operations go through repositories implementing the Unit of Work pattern.

### 4. CQRS (Command Query Responsibility Segregation)
Separate read and write operations for optimal performance and scalability.

---

## Project Dependencies

### Dependency Flow
```
Presentation → Application → Domain
Infrastructure → Application → Domain
Functions → Application → Domain
Shared → (Used by all layers)
```

### Core Projects
- **CardiTrack.Domain**: Pure business logic, no dependencies
- **CardiTrack.Application**: Use cases and interfaces
- **CardiTrack.Infrastructure**: External integrations (DB, APIs, ML)
- **CardiTrack.Shared**: Cross-cutting utilities

### Presentation Projects
- **CardiTrack.API**: RESTful API (ASP.NET Core 8)
- **CardiTrack.Web**: Family dashboard (Blazor Server)
- **CardiTrack.Mobile**: Mobile app (.NET MAUI)
- **CardiTrack.Functions**: Background jobs (Azure Functions)

---

## Technology Stack

### Backend
- **.NET 8**: Core framework
- **ASP.NET Core 8**: Web API
- **Entity Framework Core**: ORM
- **SQL Server / PostgreSQL**: Database
- **Hangfire**: Background job processing
- **Azure Functions**: Serverless compute
- **ML.NET**: Machine learning for pattern analysis

### Frontend
- **Blazor Server**: Web dashboard
- **.NET MAUI**: Cross-platform mobile app
- **Bootstrap 5**: UI framework
- **SignalR**: Real-time updates

### Infrastructure
- **Azure**: Cloud hosting
- **Terraform**: Infrastructure as Code
- **Docker**: Containerization
- **GitHub Actions**: CI/CD

### External Services
- **Fitbit API**: Wearable data
- **Apple HealthKit**: iOS integration
- **Garmin Connect API**: Garmin devices
- **Samsung Health SDK**: Samsung wearables
- **Twilio**: SMS notifications
- **SendGrid**: Email notifications

---

## Database Schema Overview

### Core Tables

**Users**: Family members (caregivers)
**Elders**: Elderly individuals being monitored
**FamilyMembers**: Relationship linking
**Devices**: Wearable device catalog
**DeviceConnections**: OAuth tokens and connection status per device
**ActivityLogs**: Normalized health data from all devices
**PatternBaselines**: AI-learned normal patterns
**Alerts**: Generated health alerts
**AuditLogs**: HIPAA-compliant audit trail
**Subscriptions**: Billing and plan management

### Multi-Device Support

```sql
-- Device Connections (Multi-device per elder)
CREATE TABLE DeviceConnections (
    Id INT PRIMARY KEY IDENTITY,
    ElderId INT NOT NULL,
    DeviceType NVARCHAR(50) NOT NULL, -- 'Fitbit', 'AppleWatch', etc.
    AccessToken NVARCHAR(MAX),
    RefreshToken NVARCHAR(MAX),
    ConnectionStatus NVARCHAR(50),
    IsPrimary BIT DEFAULT 0,
    LastSyncDate DATETIME
);

-- Activity Logs (Device-agnostic)
CREATE TABLE ActivityLogs (
    Id INT PRIMARY KEY IDENTITY,
    ElderId INT NOT NULL,
    DeviceConnectionId INT,
    DataSource NVARCHAR(50), -- Which device provided this data
    Date DATE NOT NULL,
    Steps INT,
    RestingHeartRate INT,
    SleepMinutes INT,
    SpO2Average DECIMAL(5,2),
    -- ... more normalized metrics
);
```

---

## Supported Devices

### Phase 1 (MVP)
- ✅ **Fitbit** (Charge 6, Inspire 3, Sense 2, Versa 4)

### Phase 2 (Months 3-6)
- 🔄 **Apple Watch** (Series 4+)
- 🔄 **Garmin** (Venu, Forerunner, Vivoactive)
- 🔄 **Samsung Galaxy Watch** (5, 6)

### Phase 3 (Months 6-12)
- ⏳ **Withings** (ScanWatch, Body+)
- ⏳ **Oura Ring** (Gen 3)
- ⏳ **Whoop** (4.0)

### Future
- Medical-grade blood pressure monitors
- Continuous glucose monitors (CGM)
- Medical ECG devices

---

## Device Capabilities Matrix

| Device          | Heart Rate | SpO2 | ECG | Steps | Sleep | GPS | Blood Pressure |
|-----------------|-----------|------|-----|-------|-------|-----|----------------|
| Fitbit Charge 6 | ✅        | ✅   | ✅  | ✅    | ✅    | ✅  | ❌             |
| Apple Watch 9   | ✅        | ✅   | ✅  | ✅    | ✅    | ✅  | ❌             |
| Garmin Venu     | ✅        | ✅   | ❌  | ✅    | ✅    | ✅  | ❌             |
| Samsung Watch 6 | ✅        | ✅   | ✅  | ✅    | ✅    | ✅  | ✅             |
| Withings Scan   | ✅        | ✅   | ✅  | ✅    | ✅    | ❌  | ❌             |
| Oura Ring       | ✅        | ✅   | ❌  | ✅    | ✅    | ❌  | ❌             |
| Whoop 4.0       | ✅        | ✅   | ❌  | ❌    | ✅    | ❌  | ❌             |

---

## HIPAA Compliance Features

### Technical Safeguards
- ✅ **Encryption at rest**: Azure SQL TDE
- ✅ **Encryption in transit**: TLS 1.2+
- ✅ **Access controls**: RBAC and MFA
- ✅ **Audit logging**: All PHI access tracked
- ✅ **Session timeout**: 15 minutes
- ✅ **Token encryption**: Encrypted device OAuth tokens

### Administrative Safeguards
- Privacy policies
- Security policies
- Breach notification procedures
- Workforce training
- Business Associate Agreements (BAAs)

### Monitoring
- Real-time security alerts
- Access pattern anomaly detection
- Failed authentication tracking
- Regular security audits

---

## AI/ML Pattern Analysis

### Algorithms
- **Anomaly Detection**: ML.NET IidSpikeDetector
- **Time Series Forecasting**: SSA forecasting
- **Pattern Classification**: Activity level classification

### Workflow
1. Collect 30-90 days baseline data
2. Calculate personalized patterns (steps, HR, sleep)
3. Run daily anomaly detection
4. Generate contextual alerts
5. Continuously improve models

### Alert Types
1. **Activity Alerts**: Unusual inactivity (preventive)
2. **Heart Rate Alerts**: Elevated resting HR (preventive)
3. **Sleep Disruption**: Poor sleep quality (preventive)
4. **Sudden Pattern Break**: No morning activity (reactive)
5. **Long-term Trends**: Declining mobility (preventive)

---

## Infrastructure (Terraform)

### Azure Resources
- **App Service**: API and Web hosting
- **SQL Database**: Primary data store (HIPAA-compliant)
- **Function Apps**: Background jobs
- **Storage Account**: Backups and logs
- **Key Vault**: Secrets management
- **Application Insights**: Monitoring
- **SignalR Service**: Real-time updates

### Environments
- **dev**: Development and testing
- **staging**: Pre-production validation
- **production**: Live system with full HIPAA compliance

---

## Testing Strategy

### Unit Tests (CardiTrack.UnitTests)
- Domain logic validation
- Service method testing
- Validator testing
- Device adapter testing

### Integration Tests (CardiTrack.IntegrationTests)
- API endpoint testing
- Database operations
- External API integrations (Fitbit, Garmin, etc.)
- Background job execution

### E2E Tests (CardiTrack.E2ETests)
- Complete user workflows
- Device connection flows
- Alert generation and delivery
- Dashboard functionality

---

## Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server or PostgreSQL
- Azure account (for deployment)
- Terraform (for infrastructure)
- Device API credentials (Fitbit, etc.)

### Setup
```bash
# Clone repository
git clone https://github.com/yourorg/carditrack.git

# Run setup script
.\scripts\setup-dev-environment.ps1

# Create database
dotnet ef database update --project src/Infrastructure/CardiTrack.Infrastructure

# Run API
cd src/Presentation/CardiTrack.API
dotnet run

# Run Web Dashboard
cd src/Presentation/CardiTrack.Web
dotnet run
```

---

## Contributing

See `docs/developer-guide/coding-standards.md` for guidelines.

---

## License

See `LICENSE` file for details.

---

## Contact

For questions or support, see `README.md` for contact information.
