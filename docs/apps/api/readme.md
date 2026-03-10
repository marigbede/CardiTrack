# CardiTrack API Documentation

## Overview

The CardiTrack API is a RESTful ASP.NET Core 10 Web API that serves as the backend for the CardiTrack platform. It handles authentication, device integrations, health data processing, alert management, and family member coordination.

## Technology Stack

- **.NET 10**: Core framework
- **ASP.NET Core 10**: Web API framework
- **Entity Framework Core**: ORM for database access
- **Auth0/JWT**: Authentication & authorization
- **Swagger/OpenAPI**: API documentation
- **SignalR**: Real-time notifications
- **Serilog**: Structured logging

## Project Structure

```
CardiTrack.API/
├── Controllers/
│   ├── AuthController.cs
│   ├── CardiMembersController.cs
│   ├── DashboardController.cs
│   ├── AlertsController.cs
│   ├── DevicesController.cs
│   ├── SubscriptionsController.cs
│   └── Webhooks/
│       ├── FitbitWebhookController.cs
│       ├── GarminWebhookController.cs
│       └── AppleHealthWebhookController.cs
├── DTOs/
│   ├── Requests/
│   └── Responses/
├── Middleware/
│   ├── ErrorHandlingMiddleware.cs
│   ├── AuditLoggingMiddleware.cs
│   └── HipaaComplianceMiddleware.cs
├── Extensions/
│   ├── ServiceCollectionExtensions.cs
│   ├── Auth0Extensions.cs
│   ├── SwaggerExtensions.cs
│   └── SerilogExtensions.cs
├── Infrastructure/
│   └── HealthChecks/
├── Program.cs
└── appsettings.json
```

## API Endpoints

### Authentication

#### POST /api/auth/register
Register a new user account.

**Request:**
```json
{
  "email": "user@example.com",
  "password": "SecurePassword123!",
  "firstName": "John",
  "lastName": "Doe",
  "phone": "+1234567890"
}
```

**Response:**
```json
{
  "userId": "550e8400-e29b-41d4-a716-446655440000",
  "email": "user@example.com",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

#### POST /api/auth/login
Authenticate and receive JWT token.

**Request:**
```json
{
  "email": "user@example.com",
  "password": "SecurePassword123!"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAt": "2026-01-09T00:00:00Z",
  "user": {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "email": "user@example.com",
    "firstName": "John",
    "lastName": "Doe"
  }
}
```

### CardiMembers Management

#### GET /api/cardimembers
Get all CardiMembers for the authenticated user's organization.

**Response:**
```json
{
  "cardiMembers": [
    {
      "id": "650e8400-e29b-41d4-a716-446655440000",
      "firstName": "Mary",
      "lastName": "Johnson",
      "age": 78,
      "deviceCount": 2,
      "lastSync": "2026-01-08T10:30:00Z",
      "healthStatus": "Normal",
      "activeAlerts": 0
    }
  ]
}
```

#### POST /api/cardimembers
Create a new CardiMember profile.

**Request:**
```json
{
  "firstName": "Mary",
  "lastName": "Johnson",
  "email": "mary.j@example.com",
  "phone": "+1234567890",
  "dateOfBirth": "1948-03-15",
  "gender": "Female"
}
```

#### GET /api/cardimembers/{id}
Get detailed information about a specific CardiMember.

**Response:**
```json
{
  "id": "650e8400-e29b-41d4-a716-446655440000",
  "firstName": "Mary",
  "lastName": "Johnson",
  "email": "mary.j@example.com",
  "dateOfBirth": "1948-03-15",
  "age": 78,
  "connectedDevices": [
    {
      "deviceType": "Fitbit",
      "model": "Charge 6",
      "status": "Connected",
      "lastSync": "2026-01-08T10:30:00Z"
    }
  ],
  "recentAlerts": [],
  "healthSummary": {
    "avgDailySteps": 4500,
    "avgRestingHeartRate": 68,
    "avgSleepHours": 7.2
  }
}
```

### Dashboard

#### GET /api/dashboard/{cardiMemberId}
Get comprehensive dashboard data for a CardiMember.

**Response:**
```json
{
  "cardiMember": {
    "id": "650e8400-e29b-41d4-a716-446655440000",
    "name": "Mary Johnson"
  },
  "todayMetrics": {
    "steps": 3200,
    "distance": 2.1,
    "activeMinutes": 45,
    "restingHeartRate": 68,
    "sleepHours": 7.5
  },
  "weekTrends": {
    "avgSteps": 4100,
    "stepsTrend": "down",
    "avgHeartRate": 69,
    "heartRateTrend": "stable"
  },
  "recentAlerts": [],
  "connectedDevices": [
    {
      "type": "Fitbit",
      "status": "Connected",
      "lastSync": "2026-01-08T10:30:00Z"
    }
  ]
}
```

### Devices

#### GET /api/devices/supported
Get list of supported wearable devices.

**Response:**
```json
{
  "devices": [
    {
      "type": "Fitbit",
      "models": ["Charge 6", "Inspire 3", "Sense 2"],
      "capabilities": ["HeartRate", "Steps", "Sleep", "SpO2", "ECG"],
      "oauth": true
    },
    {
      "type": "AppleWatch",
      "models": ["Series 8", "Series 9", "Ultra 2"],
      "capabilities": ["HeartRate", "Steps", "Sleep", "SpO2", "ECG"],
      "oauth": true
    }
  ]
}
```

#### POST /api/devices/connect/{cardiMemberId}
Initiate device connection for a CardiMember.

**Request:**
```json
{
  "deviceType": "Fitbit"
}
```

**Response:**
```json
{
  "authorizationUrl": "https://www.fitbit.com/oauth2/authorize?...",
  "connectionId": "750e8400-e29b-41d4-a716-446655440000",
  "expiresIn": 600
}
```

#### GET /api/devices/{cardiMemberId}/connections
Get all device connections for a CardiMember.

**Response:**
```json
{
  "connections": [
    {
      "id": "750e8400-e29b-41d4-a716-446655440000",
      "deviceType": "Fitbit",
      "status": "Connected",
      "isPrimary": true,
      "connectedDate": "2025-12-01T00:00:00Z",
      "lastSyncDate": "2026-01-08T10:30:00Z"
    }
  ]
}
```

### Alerts

#### GET /api/alerts/{cardiMemberId}
Get alerts for a CardiMember.

**Query Parameters:**
- `severity`: Filter by severity (Green, Yellow, Orange, Red)
- `startDate`: Filter by start date
- `endDate`: Filter by end date
- `acknowledged`: Filter by acknowledgment status

**Response:**
```json
{
  "alerts": [
    {
      "id": "850e8400-e29b-41d4-a716-446655440000",
      "alertType": "InactivityAlert",
      "severity": "Yellow",
      "title": "Unusual Inactivity",
      "message": "Steps have dropped 60% below baseline for 2 days.",
      "triggeredDate": "2026-01-08T08:00:00Z",
      "acknowledged": false,
      "metricValues": {
        "currentSteps": 1800,
        "baselineSteps": 4500,
        "deviation": -60
      }
    }
  ]
}
```

#### POST /api/alerts/{alertId}/acknowledge
Acknowledge an alert.

**Request:**
```json
{
  "notes": "Called Mary, she's feeling fine."
}
```

### Webhooks

#### POST /api/webhooks/fitbit
Receive webhook notifications from Fitbit.

**Request (from Fitbit):**
```json
[
  {
    "collectionType": "activities",
    "date": "2026-01-08",
    "ownerId": "ABC123",
    "ownerType": "user",
    "subscriptionId": "1"
  }
]
```

## Authentication

All endpoints (except /auth/register and /auth/login) require a Bearer token in the Authorization header:

```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Token Structure

```json
{
  "sub": "550e8400-e29b-41d4-a716-446655440000",
  "email": "user@example.com",
  "role": "Admin",
  "organizationId": "450e8400-e29b-41d4-a716-446655440000",
  "exp": 1704844800
}
```

## Error Handling

### Standard Error Response

```json
{
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "Invalid request parameters",
    "details": [
      {
        "field": "email",
        "message": "Email is required"
      }
    ]
  }
}
```

### HTTP Status Codes

- **200 OK**: Successful request
- **201 Created**: Resource created successfully
- **400 Bad Request**: Invalid request parameters
- **401 Unauthorized**: Missing or invalid authentication
- **403 Forbidden**: Insufficient permissions
- **404 Not Found**: Resource not found
- **409 Conflict**: Resource already exists
- **500 Internal Server Error**: Server error

## Rate Limiting

- **Anonymous**: 10 requests/minute
- **Authenticated**: 100 requests/minute
- **Premium**: 1000 requests/minute

Headers returned:
```
X-RateLimit-Limit: 100
X-RateLimit-Remaining: 95
X-RateLimit-Reset: 1704844800
```

## HIPAA Compliance

### Audit Logging

All PHI access is logged with:
- User ID
- CardiMember ID
- Action performed
- Timestamp
- IP address
- User agent

### Data Encryption

- TLS 1.2+ for data in transit
- Field-level encryption for sensitive data
- Database TDE for data at rest

## Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...",
    "Redis": "..."
  },
  "Auth0": {
    "Domain": "carditrack.auth0.com",
    "Audience": "https://api.carditrack.com"
  },
  "Fitbit": {
    "ClientId": "...",
    "ClientSecret": "...",
    "CallbackUrl": "https://api.carditrack.com/api/devices/callback/fitbit"
  },
  "Twilio": {
    "AccountSid": "...",
    "AuthToken": "...",
    "FromNumber": "+1234567890"
  },
  "ApplicationInsights": {
    "ConnectionString": "..."
  }
}
```

## Running Locally

```bash
# Navigate to API project
cd src/Presentation/CardiTrack.API

# Restore dependencies
dotnet restore

# Update database
dotnet ef database update --project ../../Infrastructure/CardiTrack.Infrastructure

# Run API
dotnet run

# API will be available at:
# https://localhost:7001
# http://localhost:5001
```

## Swagger Documentation

When running locally, access Swagger UI at:
```
https://localhost:7001/swagger
```

## Health Checks

#### GET /health
Basic health check endpoint.

**Response:**
```json
{
  "status": "Healthy",
  "checks": {
    "database": "Healthy",
    "redis": "Healthy",
    "fitbit_api": "Healthy"
  }
}
```

## Testing

```bash
# Run unit tests
dotnet test tests/CardiTrack.UnitTests

# Run integration tests
dotnet test tests/CardiTrack.IntegrationTests

# Run all tests
dotnet test
```

## Deployment

See [Infrastructure Documentation](../INFRASTRUCTURE.md) for deployment instructions.

## Support

For API support, contact: api-support@carditrack.com
