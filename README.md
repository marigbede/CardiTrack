# CardiTrack

> Multi-device elderly health monitoring platform with AI-powered preventive alerts

CardiTrack is an affordable health monitoring service that connects to wearable devices (Fitbit, Apple Watch, Garmin, Samsung, etc.) to provide families with peace of mind through preventive health monitoring powered by AI pattern analysis.

## 🎯 Key Features

- **Multi-Device Support**: Works with Fitbit, Apple Watch, Garmin, Samsung, and more
- **Preventive Alerts**: AI detects concerning patterns BEFORE emergencies
- **Affordable**: 50-70% cheaper than traditional medical alert systems ($8-15/month vs $40-70/month)
- **Device-Agnostic**: Works with devices elderly users already own
- **HIPAA Compliant**: Enterprise-grade security and encryption
- **Family Dashboard**: Real-time health monitoring for caregivers

## 🏗️ Architecture

CardiTrack follows **Clean Architecture** principles with clear separation of concerns:

```
┌─────────────────────────────────────┐
│    Presentation Layer               │
│  (API, Web, Mobile, Functions)      │
└─────────────────────────────────────┘
              ↓
┌─────────────────────────────────────┐
│    Application Layer                │
│  (Use Cases, DTOs, Interfaces)      │
└─────────────────────────────────────┘
              ↓
┌─────────────────────────────────────┐
│    Domain Layer                     │
│  (Entities, Value Objects)          │
└─────────────────────────────────────┘
              ↑
┌─────────────────────────────────────┐
│    Infrastructure Layer             │
│  (EF Core, External APIs, ML)       │
└─────────────────────────────────────┘
```

## 📁 Solution Structure

```
CardiTrack/
├── src/
│   ├── Core/
│   │   ├── CardiTrack.Domain          # Business entities
│   │   └── CardiTrack.Application     # Use cases & interfaces
│   ├── Infrastructure/
│   │   ├── CardiTrack.Infrastructure  # External services, DB, ML
│   │   └── CardiTrack.Shared          # Common utilities
│   ├── Presentation/
│   │   ├── CardiTrack.API             # REST API
│   │   ├── CardiTrack.Web             # Blazor dashboard
│   │   └── CardiTrack.Mobile          # .NET MAUI app
│   └── Functions/
│       └── CardiTrack.Functions       # Background jobs
├── tests/
│   ├── CardiTrack.UnitTests
│   ├── CardiTrack.IntegrationTests
│   └── CardiTrack.E2ETests
├── infrastructure/
│   └── terraform/                     # Infrastructure as Code
│       ├── environments/
│       │   ├── dev/
│       │   └── production/
│       └── modules/
└── docs/                              # Documentation
```

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download) or later
- [SQL Server](https://www.microsoft.com/sql-server) or [PostgreSQL](https://www.postgresql.org/)
- [Azure Account](https://azure.microsoft.com/) (for deployment)
- [Terraform](https://www.terraform.io/) (for infrastructure)

### Local Development

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourorg/carditrack.git
   cd carditrack
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Set up database**
   ```bash
   cd src/Infrastructure/CardiTrack.Infrastructure
   dotnet ef database update
   ```

4. **Run the API**
   ```bash
   cd src/Presentation/CardiTrack.API
   dotnet run
   ```

5. **Run the Web Dashboard**
   ```bash
   cd src/Presentation/CardiTrack.Web
   dotnet run
   ```

### Running Tests

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test tests/CardiTrack.UnitTests
```

## 🏥 HIPAA Compliance

CardiTrack is designed with HIPAA compliance in mind:

- ✅ Encryption at rest (TDE on SQL Database)
- ✅ Encryption in transit (TLS 1.2+)
- ✅ Audit logging (90-day retention)
- ✅ Access controls (RBAC + MFA)
- ✅ Secure token storage (Azure Key Vault)
- ✅ Data retention policies
- ✅ Business Associate Agreements (BAAs)

## 📊 Supported Devices

### Current Support
- ✅ **Fitbit** (Charge 6, Inspire 3, Sense 2, Versa 4)

### Planned Support
- 🔄 **Apple Watch** (Series 4+)
- 🔄 **Garmin** (Venu, Forerunner, Vivoactive)
- 🔄 **Samsung Galaxy Watch** (5, 6)
- ⏳ **Withings** (ScanWatch)
- ⏳ **Oura Ring** (Gen 3)
- ⏳ **Whoop** (4.0)

## 🧠 AI/ML Features

CardiTrack uses **ML.NET** for pattern analysis:

- **Anomaly Detection**: Identifies unusual patterns in activity, heart rate, and sleep
- **Personalized Baselines**: Learns individual normal patterns (30-90 days)
- **Preventive Alerts**: 5 alert types (activity, heart rate, sleep, pattern break, trends)
- **Continuous Learning**: Models improve over time with more data

## 🌐 Deployment

### Infrastructure Setup

```bash
cd infrastructure/terraform/environments/production
cp terraform.tfvars.example terraform.tfvars
# Edit terraform.tfvars with your values

terraform init
terraform plan
terraform apply
```

See [Terraform README](infrastructure/terraform/README.md) for detailed instructions.

## 📖 Documentation

- [Solution Structure](SOLUTION_STRUCTURE.md)
- [Architecture Guide](docs/architecture/system-architecture.md)
- [Device Integration Guide](docs/devices/adding-new-device.md)
- [HIPAA Requirements](docs/compliance/hipaa-requirements.md)
- [Developer Guide](docs/developer-guide/getting-started.md)

## 🤝 Contributing

We welcome contributions! Please see our contributing guidelines.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🆘 Support

For issues, questions, or feature requests:
- Open an issue on GitHub
- Email: support@carditrack.com
- Documentation: [docs/](docs/)

## 🙏 Acknowledgments

- Fitbit Web API
- Apple HealthKit
- Garmin Connect API
- ML.NET Team
- Azure Platform

---

**Built with ❤️ for family caregivers**
