# CardiTrack Entity Summary

This document provides an overview of all domain entities in the CardiTrack system.

## Entity Overview

### Core Entities

#### 1. **Organization**
- Represents either a Family account or Business (care home)
- Contains: Name, Type (Family/Business), IsActive
- **No FK constraints** - uses Guid references only

#### 2. **User**
- Login account for family members or care home staff
- Contains: Email, PasswordHash, Name, Phone, Role, OrganizationId
- Role hidden for Family type organizations

#### 3. **CardiMember**
- Person being monitored (can be the User themselves)
- Contains: Name, Email, Phone, DateOfBirth, Gender, OrganizationId
- Medical notes stored encrypted
- Links to devices, activity logs, alerts, and pattern baselines

#### 4. **UserCardiMember** (Join Table)
- Many-to-many relationship between Users and CardiMembers
- Contains: RelationshipType, IsPrimaryCaregiver, permissions
- Enables multiple users to monitor same CardiMember (care home scenario)

### Device & Health Data Entities

#### 5. **DeviceConnection**
- Stores OAuth tokens for connected wearable devices
- **Device-agnostic design** - supports Fitbit, Apple Watch, Garmin, Samsung, etc.
- Contains: DeviceType, ConnectionStatus, AccessToken (encrypted), RefreshToken (encrypted)
- No FK constraints - uses CardiMemberId (Guid)

#### 6. **ActivityLog**
- Normalized health data from all device types
- Contains: Steps, Heart Rate, Sleep metrics, SpO2, VO2Max, etc.
- Tracks DataSource (which device provided the data)
- No FK constraints - uses CardiMemberId and DeviceConnectionId (Guid)

#### 7. **Alert**
- AI-generated health alerts
- Contains: AlertType (Inactivity, HeartRate, Sleep, PatternBreak, Trend)
- Severity levels: Green, Yellow, Orange, Red
- Tracks acknowledgment by users

#### 8. **PatternBaseline**
- AI-learned normal patterns for each CardiMember
- Calculated over 30, 60, or 90 day periods
- Contains: Average steps, heart rate baselines, sleep patterns
- Includes day-of-week variations (JSON)

### Business Entities

#### 9. **Subscription**
- Billing and subscription management
- Contains: Tier (Basic, Complete, Plus), Status, Price, BillingCycle
- MaxCardiMembers and MaxUsers (tier limits)
- Features stored as JSON for flexibility

#### 10. **Device** (Catalog)
- Reference data for supported wearable devices
- Contains: DeviceType, Manufacturer, ModelName, Capabilities (JSON)
- OAuth configuration stored as JSON
- Used for UI display and capability checking

### Compliance Entities

#### 11. **AuditLog**
- HIPAA compliance audit trail
- Tracks all PHI (Protected Health Information) access
- Contains: UserId, CardiMemberId, Action, EntityType, Timestamp
- IP address, user agent, request details
- 90-day minimum retention required

## Design Principles

### 1. No Foreign Key Constraints
- All relationships use Guid references without FK constraints
- Prevents orphan record issues during deletions
- Application-level referential integrity via repositories
- More flexible for soft deletes and data archival

### 2. Guid Primary Keys
- All entities use Guid for Id (not int)
- Better for distributed systems
- No sequential ID enumeration security risk
- Easier cross-database/cross-service references

### 3. Device-Agnostic Architecture
- DeviceType enum supports all wearables (Fitbit, Apple Watch, Garmin, Samsung, Withings, Oura, Whoop)
- ActivityLog.DataSource tracks which device provided data
- Normalized data schema works with any device
- Device catalog table for device capabilities

### 4. Soft Deletes
- Entities implement ISoftDeletable interface
- IsActive flag instead of hard deletes
- Maintains data integrity and audit trail
- HIPAA compliance for data retention

### 5. JSON for Flexibility
- NotificationPreferences, Metadata, Features stored as JSON
- Allows schema evolution without migrations
- Device-specific data stored flexibly
- Pattern baselines store day-of-week arrays

### 6. Security & Encryption
- OAuth tokens (AccessToken, RefreshToken) encrypted in database
- MedicalNotes encrypted
- PasswordHash for user authentication
- Audit logging for all PHI access

## Entity Relationships

```
Organization (1) ──→ (N) User
Organization (1) ──→ (N) CardiMember
Organization (1) ──→ (1) Subscription

User (M) ←──→ (N) CardiMember (via UserCardiMember join table)

CardiMember (1) ──→ (N) DeviceConnection
CardiMember (1) ──→ (N) ActivityLog
CardiMember (1) ──→ (N) Alert
CardiMember (1) ──→ (N) PatternBaseline

DeviceConnection (1) ──→ (N) ActivityLog
```

## Enums

- **OrganizationType**: Family, Business
- **UserRole**: Member, Admin, Staff
- **RelationshipType**: Self, Parent, Spouse, Grandparent, Sibling, Child, Other
- **DeviceType**: Fitbit, AppleWatch, Garmin, Samsung, Withings, Oura, Whoop, Other
- **ConnectionStatus**: Connected, Disconnected, TokenExpired, AuthError, SyncError
- **AlertType**: Inactivity, HeartRate, Sleep, PatternBreak, Trend
- **AlertSeverity**: Green, Yellow, Orange, Red
- **SubscriptionTier**: Basic, Complete, Plus
- **SubscriptionStatus**: Trial, Active, PastDue, Cancelled, Suspended
- **BillingCycle**: Monthly, Annual
- **Gender**: Male, Female, Other, PreferNotToSay

## File Structure

```
CardiTrack.Domain/
├── Common/
│   └── BaseEntity.cs
├── Interfaces/
│   ├── IEntity.cs
│   └── ISoftDeletable.cs
├── Enums/
│   ├── OrganizationType.cs
│   ├── UserRole.cs
│   ├── RelationshipType.cs
│   ├── DeviceType.cs
│   ├── ConnectionStatus.cs
│   ├── AlertType.cs
│   ├── AlertSeverity.cs
│   ├── SubscriptionTier.cs
│   ├── SubscriptionStatus.cs
│   ├── BillingCycle.cs
│   └── Gender.cs
└── Entities/
    ├── Organization.cs
    ├── User.cs
    ├── CardiMember.cs
    ├── UserCardiMember.cs
    ├── DeviceConnection.cs
    ├── ActivityLog.cs
    ├── Alert.cs
    ├── PatternBaseline.cs
    ├── Subscription.cs
    ├── Device.cs
    └── AuditLog.cs
```

## Next Steps

1. Create EF Core DbContext and entity configurations (CardiTrack.Infrastructure)
2. Configure entity mappings (FluentAPI)
3. Set up encryption for sensitive fields (tokens, medical notes)
4. Create initial database migration
5. Implement repositories with Guid-based queries
6. Add indexes for performance (CardiMemberId, UserId, Date fields)
7. Configure JSON column serialization
8. Set up audit logging middleware
