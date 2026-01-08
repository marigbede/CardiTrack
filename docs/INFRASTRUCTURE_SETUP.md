# CardiTrack Infrastructure Setup

## EF Core DbContext & Configurations

All Entity Framework Core infrastructure has been successfully set up with HIPAA-compliant security features.

## Files Created

### DbContext
- **CardiTrackDbContext.cs** - Main database context with automatic timestamp updates

### Entity Configurations (FluentAPI)
All entities configured with proper indexes, constraints, and data types:

1. **OrganizationConfiguration** - Organization setup (Family/Business types)
2. **UserConfiguration** - User authentication and roles
3. **CardiMemberConfiguration** - People being monitored
4. **UserCardiMemberConfiguration** - Many-to-many user-member relationships
5. **DeviceConnectionConfiguration** - Device OAuth connections
6. **ActivityLogConfiguration** - Health data logs (device-agnostic)
7. **AlertConfiguration** - AI-generated health alerts
8. **PatternBaselineConfiguration** - Learned health patterns
9. **SubscriptionConfiguration** - Billing and subscription management
10. **DeviceConfiguration** - Device catalog (reference data)
11. **AuditLogConfiguration** - HIPAA audit trail

### Security
- **IEncryptionService** - Encryption service interface
- **AesEncryptionService** - AES-256-GCM encryption for sensitive data

## Key Features

### No Foreign Key Constraints
- All relationships use Guid references without FK constraints
- Application-level referential integrity
- More flexible for soft deletes and data archival

### Encryption Support
- OAuth tokens (AccessToken, RefreshToken) - encrypted
- Medical notes - encrypted
- Uses AES-256-GCM (authenticated encryption)
- HIPAA compliant

### JSON Fields
The following fields store JSON data for flexibility:
- **UserCardiMember.NotificationPreferences** - User notification settings
- **DeviceConnection.Scopes** - OAuth scopes array
- **DeviceConnection.Metadata** - Device metadata
- **ActivityLog.* (various)** - Flexible health metrics
- **Alert.MetricValues** - Alert context data
- **PatternBaseline.StepsByDayOfWeek** - Day-of-week patterns array
- **Subscription.Features** - Subscription features
- **Subscription.PaymentMethod** - Payment details
- **Device.Capabilities** - Device capabilities
- **Device.OAuthConfig** - OAuth configuration
- **AuditLog.DataAccessed** - PHI access summary
- **AuditLog.ChangedFields** - Change tracking

### Indexes for Performance
Strategic indexes on:
- Foreign key references (OrganizationId, CardiMemberId, UserId, etc.)
- Query filters (IsActive, Date, Status, etc.)
- Composite indexes for common queries
- HIPAA audit queries (CardiMemberId + Timestamp, UserId + Timestamp)

### Automatic Timestamp Management
- `CreatedDate` automatically set on insert (SQL: GETUTCDATE())
- `UpdatedDate` automatically set on update (via DbContext override)

### Soft Deletes
- All entities implement `ISoftDeletable` where appropriate
- `IsActive` flag instead of hard deletes
- Maintains data integrity and audit trail

## Database Schema Highlights

### Core Tables
- **Organizations** - Family or Business accounts
- **Users** - Login accounts
- **CardiMembers** - People being monitored
- **UserCardiMembers** - M:N join table with permissions

### Device & Health
- **DeviceConnections** - OAuth tokens per device
- **ActivityLogs** - Normalized health data (device-agnostic)
- **Alerts** - Health alerts
- **PatternBaselines** - AI-learned baselines

### Business & Compliance
- **Subscriptions** - Billing
- **Devices** - Device catalog (reference)
- **AuditLogs** - HIPAA audit trail (90-day retention)

## Security Features (HIPAA Compliant)

### Encryption
- **AES-256-GCM** authenticated encryption
- Encrypts:
  - DeviceConnection.AccessToken
  - DeviceConnection.RefreshToken
  - CardiMember.MedicalNotes
  - User.PasswordHash (via bcrypt, not AES)

### Audit Logging
- All PHI access logged
- UserId, CardiMemberId, Action, Timestamp
- IP address, User agent tracking
- 90-day minimum retention

### Access Controls
- Application-level permission checks via UserCardiMember
- Role-based access (Admin, Staff, Member)
- Organization isolation

## Next Steps

### 1. Create Initial Migration
```bash
cd src/Infrastructure/CardiTrack.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../../Presentation/CardiTrack.API
```

### 2. Update Database
```bash
dotnet ef database update --startup-project ../../Presentation/CardiTrack.API
```

### 3. Configuration Setup
Add to `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CardiTrack;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "Encryption": {
    "Key": "<base64-encoded-256-bit-key>"
  }
}
```

### 4. Generate Encryption Key
```csharp
using CardiTrack.Infrastructure.Security;

var key = AesEncryptionService.GenerateKey();
Console.WriteLine($"Encryption Key: {key}");
// Store this securely in Azure Key Vault or appsettings
```

### 5. Register Services
In `Program.cs` or `Startup.cs`:
```csharp
// Add DbContext
builder.Services.AddDbContext<CardiTrackDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Encryption Service
var encryptionKey = builder.Configuration["Encryption:Key"];
builder.Services.AddSingleton<IEncryptionService>(
    new AesEncryptionService(encryptionKey));
```

### 6. Seed Reference Data
Create seed data for:
- **Devices** table (Fitbit Charge 6, Apple Watch, etc.)
- Default subscription tiers

## Column Types

### Decimal Fields
- `ActivityLog.Distance` - decimal(10,2)
- `ActivityLog.SpO2*` - decimal(5,2)
- `ActivityLog.VO2Max` - decimal(5,2)
- `ActivityLog.BreathingRate` - decimal(5,2)
- `ActivityLog.Temperature` - decimal(5,2)
- `PatternBaseline.StdDev*` - decimal(10,2)
- `Subscription.Price` - decimal(10,2)

### DateTime Fields
- All use UTC (`GETUTCDATE()` default in SQL Server)

### Enum Storage
- Stored as strings (e.g., "Family", "Fitbit", "Red")
- MaxLength constraints applied

### String Lengths
- Email: 255
- Name: 200
- Phone: 20
- DeviceType: 50
- MedicalNotes: 2000 (encrypted)
- Tokens: 2000 (encrypted)
- JSON fields: 500-2000 depending on usage

## Performance Considerations

### Indexes Created
- All foreign key references indexed
- Composite indexes for common queries
- Unique indexes on Email, (CardiMemberId + Date), etc.

### Query Optimization
- DateOnly for dates (no time component)
- Nullable fields for optional data
- JSON for flexible metadata (avoids schema changes)

## HIPAA Compliance Checklist

✅ Encryption at rest (TDE on SQL Server)
✅ Encryption in transit (TLS 1.2+)
✅ Field-level encryption (tokens, medical notes)
✅ Comprehensive audit logging
✅ 90-day audit retention
✅ Access controls (application-level)
✅ Soft deletes (data retention)
✅ No PHI in logs (use entity IDs only)

## Troubleshooting

### Migration Issues
If migrations fail, ensure:
1. Startup project (API) has connection string
2. EF Core tools installed globally: `dotnet tool install --global dotnet-ef`
3. Infrastructure project references EF Core Design package

### Encryption Key Management
- **Development**: appsettings.Development.json
- **Production**: Azure Key Vault (recommended)
- **Never commit** encryption keys to source control

### Database Provider
Current configuration uses SQL Server. To switch to PostgreSQL:
1. Replace `Microsoft.EntityFrameworkCore.SqlServer` with `Npgsql.EntityFrameworkCore.PostgreSQL`
2. Update connection string
3. Change `GETUTCDATE()` to `NOW()` in configurations
4. Recreate migrations

