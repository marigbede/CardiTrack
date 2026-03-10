# CardiTrack Azure Functions Documentation

## Overview

CardiTrack Azure Functions provide serverless background processing for the CardiTrack platform. These functions handle scheduled tasks like device data synchronization, pattern analysis, alert processing, and token management. They run independently of the main API and can scale automatically based on workload.

## Technology Stack

- **.NET 10**: Core framework
- **Azure Functions v4**: Serverless compute platform
- **Isolated Process Model**: Out-of-process execution
- **Timer Triggers**: Scheduled execution (CRON expressions)
- **Queue Triggers**: Event-driven processing
- **Durable Functions**: Complex workflows and orchestrations
- **Application Insights**: Monitoring and diagnostics

## Project Structure

```
CardiTrack.Functions/
├── Functions/
│   ├── DeviceSyncFunction.cs           # Sync data from wearable devices
│   ├── PatternAnalysisFunction.cs      # Analyze health patterns with AI/ML
│   ├── AlertProcessingFunction.cs      # Process and send health alerts
│   ├── TokenRefreshFunction.cs         # Refresh OAuth tokens
│   ├── BaselineCalculationFunction.cs  # Recalculate health baselines
│   ├── ReportGenerationFunction.cs     # Generate weekly/monthly reports
│   └── DataCleanupFunction.cs          # Archive old data
├── Services/
│   ├── IDeviceService.cs
│   ├── DeviceService.cs
│   ├── IPatternAnalysisService.cs
│   ├── PatternAnalysisService.cs
│   ├── IAlertService.cs
│   ├── AlertService.cs
│   └── INotificationService.cs
├── Models/
│   ├── SyncResult.cs
│   ├── AnalysisResult.cs
│   └── AlertContext.cs
├── Extensions/
│   ├── ServiceCollectionExtensions.cs
│   └── ConfigurationExtensions.cs
├── host.json                            # Function host configuration
├── local.settings.json                 # Local development settings
└── CardiTrack.Functions.csproj         # Project file
```

## Core Functions

### 1. DeviceSyncFunction

Synchronizes health data from wearable devices (Fitbit, Apple Watch, Garmin) to the CardiTrack database.

#### Implementation

```csharp
public class DeviceSyncFunction
{
    private readonly IDeviceService _deviceService;
    private readonly ILogger<DeviceSyncFunction> _logger;

    public DeviceSyncFunction(
        IDeviceService deviceService,
        ILogger<DeviceSyncFunction> logger)
    {
        _deviceService = deviceService;
        _logger = logger;
    }

    [Function("DeviceSyncFunction")]
    public async Task Run(
        [TimerTrigger("0 */30 * * * *")] TimerInfo timer) // Every 30 minutes
    {
        _logger.LogInformation($"DeviceSync started at: {DateTime.UtcNow}");

        try
        {
            // Get all active device connections
            var connections = await _deviceService.GetActiveConnectionsAsync();
            _logger.LogInformation($"Found {connections.Count} active device connections");

            var results = new List<SyncResult>();

            // Sync each device in parallel
            var syncTasks = connections.Select(async connection =>
            {
                try
                {
                    var result = await _deviceService.SyncDeviceDataAsync(connection);
                    results.Add(result);
                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to sync device {connection.Id}");
                    return new SyncResult
                    {
                        ConnectionId = connection.Id,
                        Success = false,
                        Error = ex.Message
                    };
                }
            });

            await Task.WhenAll(syncTasks);

            // Log summary
            var successful = results.Count(r => r.Success);
            var failed = results.Count(r => !r.Success);
            _logger.LogInformation($"Sync completed: {successful} successful, {failed} failed");

            // Queue pattern analysis for successfully synced devices
            foreach (var result in results.Where(r => r.Success))
            {
                await QueuePatternAnalysis(result.CardiMemberId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DeviceSync function failed");
            throw;
        }
    }

    private async Task QueuePatternAnalysis(Guid cardiMemberId)
    {
        // Queue message for pattern analysis
        var queueClient = new QueueClient(
            Environment.GetEnvironmentVariable("AzureWebJobsStorage"),
            "pattern-analysis-queue");

        var message = new { CardiMemberId = cardiMemberId, Date = DateTime.UtcNow };
        await queueClient.SendMessageAsync(JsonSerializer.Serialize(message));
    }
}
```

#### Schedule

- **Trigger**: Timer (CRON: `0 */30 * * * *`)
- **Frequency**: Every 30 minutes
- **Execution Time**: 2-5 minutes (depending on number of devices)

#### Key Operations

1. Fetch all active device connections from database
2. For each device:
   - Check if token is expired, refresh if needed
   - Fetch latest health data from device API
   - Transform data to CardiTrack format
   - Save to ActivityLogs table
3. Update LastSyncDate timestamp
4. Queue pattern analysis for synced devices

#### Error Handling

- **Token Expired**: Automatically refresh using refresh token
- **API Rate Limit**: Implement exponential backoff
- **Device Disconnected**: Mark connection as inactive, notify user
- **Network Timeout**: Retry up to 3 times

### 2. PatternAnalysisFunction

Analyzes health data patterns using AI/ML to detect anomalies and generate alerts.

#### Implementation

```csharp
public class PatternAnalysisFunction
{
    private readonly IPatternAnalysisService _patternService;
    private readonly IAlertService _alertService;
    private readonly ILogger<PatternAnalysisFunction> _logger;

    public PatternAnalysisFunction(
        IPatternAnalysisService patternService,
        IAlertService alertService,
        ILogger<PatternAnalysisFunction> logger)
    {
        _patternService = patternService;
        _alertService = alertService;
        _logger = logger;
    }

    [Function("PatternAnalysisFunction")]
    public async Task Run(
        [QueueTrigger("pattern-analysis-queue")] string message)
    {
        _logger.LogInformation($"Pattern analysis triggered: {message}");

        try
        {
            var request = JsonSerializer.Deserialize<AnalysisRequest>(message);

            // Load historical data (30-90 days)
            var history = await _patternService.GetHistoricalDataAsync(
                request.CardiMemberId,
                days: 90);

            // Get current baseline
            var baseline = await _patternService.GetBaselineAsync(request.CardiMemberId);

            // Get today's data
            var todayData = await _patternService.GetTodayDataAsync(
                request.CardiMemberId,
                request.Date);

            // Run ML anomaly detection
            var analysisResult = await _patternService.DetectAnomaliesAsync(
                todayData,
                baseline,
                history);

            _logger.LogInformation(
                $"Analysis complete for CardiMember {request.CardiMemberId}: " +
                $"{analysisResult.AnomaliesDetected.Count} anomalies detected");

            // Generate alerts for significant anomalies
            foreach (var anomaly in analysisResult.AnomaliesDetected)
            {
                if (anomaly.Severity >= AlertSeverity.Yellow)
                {
                    var alert = await _alertService.CreateAlertAsync(
                        request.CardiMemberId,
                        anomaly);

                    _logger.LogInformation(
                        $"Alert created: {alert.Id} - {alert.Title} ({alert.Severity})");

                    // Queue alert for processing (notification sending)
                    await QueueAlertProcessing(alert);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Pattern analysis failed");
            throw;
        }
    }

    private async Task QueueAlertProcessing(Alert alert)
    {
        var queueClient = new QueueClient(
            Environment.GetEnvironmentVariable("AzureWebJobsStorage"),
            "alert-processing-queue");

        var message = new { AlertId = alert.Id };
        await queueClient.SendMessageAsync(JsonSerializer.Serialize(message));
    }
}
```

#### Trigger

- **Type**: Queue Trigger
- **Queue**: `pattern-analysis-queue`
- **Source**: Queued by DeviceSyncFunction after successful sync

#### Key Operations

1. Load CardiMember's historical health data (30-90 days)
2. Retrieve personalized baseline patterns
3. Fetch today's metrics
4. Run ML algorithms:
   - Z-score calculation for each metric
   - Trend analysis (declining, improving, stable)
   - Day-of-week pattern comparison
   - Multi-day deviation detection
5. Generate alerts for significant anomalies
6. Queue alerts for notification processing

#### ML Algorithms

- **Anomaly Detection**: IID Spike Detection, Change Point Detection
- **Time Series Analysis**: Seasonal patterns, trending
- **Statistical Methods**: Z-scores, standard deviations
- **Thresholds**:
  - Yellow Alert: >2 standard deviations
  - Orange Alert: >3 standard deviations
  - Red Alert: >4 standard deviations or critical pattern

### 3. AlertProcessingFunction

Processes generated alerts and sends notifications to family members via SMS, email, and push notifications.

#### Implementation

```csharp
public class AlertProcessingFunction
{
    private readonly IAlertService _alertService;
    private readonly INotificationService _notificationService;
    private readonly ILogger<AlertProcessingFunction> _logger;

    public AlertProcessingFunction(
        IAlertService alertService,
        INotificationService notificationService,
        ILogger<AlertProcessingFunction> logger)
    {
        _alertService = alertService;
        _notificationService = notificationService;
        _logger = logger;
    }

    [Function("AlertProcessingFunction")]
    public async Task Run(
        [QueueTrigger("alert-processing-queue")] string message)
    {
        _logger.LogInformation($"Alert processing triggered: {message}");

        try
        {
            var request = JsonSerializer.Deserialize<AlertProcessingRequest>(message);

            // Load alert details
            var alert = await _alertService.GetAlertByIdAsync(request.AlertId);
            if (alert == null)
            {
                _logger.LogWarning($"Alert {request.AlertId} not found");
                return;
            }

            // Get family members who should receive this alert
            var familyMembers = await _alertService.GetAlertRecipientsAsync(
                alert.CardiMemberId,
                alert.Severity);

            _logger.LogInformation(
                $"Sending alert {alert.Id} to {familyMembers.Count} family members");

            // Send notifications via multiple channels
            var notificationTasks = familyMembers.Select(async member =>
            {
                try
                {
                    // Send push notification
                    if (!string.IsNullOrEmpty(member.DeviceToken))
                    {
                        await _notificationService.SendPushNotificationAsync(
                            member.DeviceToken,
                            alert.Title,
                            alert.Message,
                            new { alertId = alert.Id });
                    }

                    // Send SMS for high-severity alerts
                    if (alert.Severity >= AlertSeverity.Orange && !string.IsNullOrEmpty(member.Phone))
                    {
                        await _notificationService.SendSmsAsync(
                            member.Phone,
                            $"CardiTrack Alert: {alert.Title}. {alert.Message}");
                    }

                    // Send email
                    if (!string.IsNullOrEmpty(member.Email))
                    {
                        await _notificationService.SendEmailAsync(
                            member.Email,
                            $"CardiTrack Alert: {alert.Title}",
                            GenerateEmailBody(alert, member));
                    }

                    _logger.LogInformation(
                        $"Notifications sent to {member.Email} for alert {alert.Id}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to send notification to {member.Email}");
                }
            });

            await Task.WhenAll(notificationTasks);

            // Mark alert as processed
            await _alertService.MarkAlertAsProcessedAsync(alert.Id);

            _logger.LogInformation($"Alert processing completed for {alert.Id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Alert processing failed");
            throw;
        }
    }

    private string GenerateEmailBody(Alert alert, FamilyMember member)
    {
        return $@"
            <h2>CardiTrack Health Alert</h2>
            <p>Hi {member.FirstName},</p>
            <p><strong>{alert.Title}</strong></p>
            <p>{alert.Message}</p>
            <p><strong>Severity:</strong> {alert.Severity}</p>
            <p><strong>Time:</strong> {alert.TriggeredDate:MMM dd, yyyy h:mm tt}</p>
            <p><a href='https://app.carditrack.com/alerts/{alert.Id}'>View Details</a></p>
            <p>Stay healthy,<br/>CardiTrack Team</p>
        ";
    }
}
```

#### Trigger

- **Type**: Queue Trigger
- **Queue**: `alert-processing-queue`
- **Source**: Queued by PatternAnalysisFunction when alert is generated

#### Notification Channels

1. **Push Notifications**: Sent to mobile app (iOS/Android)
2. **SMS**: For Orange/Red severity alerts
3. **Email**: For all alerts with detailed information

#### Alert Severity Rules

- **Green**: Email only, informational
- **Yellow**: Push + Email
- **Orange**: Push + Email + SMS
- **Red**: Push + Email + SMS + Phone call (future)

### 4. TokenRefreshFunction

Automatically refreshes OAuth tokens for device connections before they expire.

#### Implementation

```csharp
public class TokenRefreshFunction
{
    private readonly IDeviceService _deviceService;
    private readonly ILogger<TokenRefreshFunction> _logger;

    public TokenRefreshFunction(
        IDeviceService deviceService,
        ILogger<TokenRefreshFunction> logger)
    {
        _deviceService = deviceService;
        _logger = logger;
    }

    [Function("TokenRefreshFunction")]
    public async Task Run(
        [TimerTrigger("0 0 */4 * * *")] TimerInfo timer) // Every 4 hours
    {
        _logger.LogInformation($"Token refresh started at: {DateTime.UtcNow}");

        try
        {
            // Get connections with tokens expiring in next 2 hours
            var expiringConnections = await _deviceService.GetExpiringTokenConnectionsAsync(
                hoursAhead: 2);

            _logger.LogInformation(
                $"Found {expiringConnections.Count} connections with expiring tokens");

            var refreshTasks = expiringConnections.Select(async connection =>
            {
                try
                {
                    await _deviceService.RefreshTokenAsync(connection);
                    _logger.LogInformation(
                        $"Token refreshed for connection {connection.Id}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        $"Failed to refresh token for connection {connection.Id}");

                    // If refresh fails, notify user to reconnect device
                    await NotifyReconnectRequired(connection);
                }
            });

            await Task.WhenAll(refreshTasks);

            _logger.LogInformation("Token refresh completed");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Token refresh function failed");
            throw;
        }
    }

    private async Task NotifyReconnectRequired(DeviceConnection connection)
    {
        // Queue notification to user that device needs reconnection
        var queueClient = new QueueClient(
            Environment.GetEnvironmentVariable("AzureWebJobsStorage"),
            "alert-processing-queue");

        var alert = new
        {
            CardiMemberId = connection.CardiMemberId,
            Type = "DeviceReconnectRequired",
            DeviceType = connection.DeviceType,
            Message = $"Please reconnect your {connection.DeviceType} device to continue monitoring."
        };

        await queueClient.SendMessageAsync(JsonSerializer.Serialize(alert));
    }
}
```

#### Schedule

- **Trigger**: Timer (CRON: `0 0 */4 * * *`)
- **Frequency**: Every 4 hours
- **Purpose**: Proactively refresh tokens before expiration

#### Token Expiration Handling

- **Fitbit**: Tokens expire after 8 hours
- **Apple**: Tokens expire after 6 months
- **Garmin**: Tokens expire after 1 year

#### Refresh Process

1. Identify connections with tokens expiring in next 2 hours
2. Call device API refresh endpoint with refresh token
3. Save new access token and refresh token to database
4. Update TokenExpiry timestamp
5. If refresh fails (invalid refresh token), notify user to reconnect

### 5. BaselineCalculationFunction

Recalculates personalized health baselines for pattern analysis.

#### Implementation

```csharp
public class BaselineCalculationFunction
{
    private readonly IPatternAnalysisService _patternService;
    private readonly ILogger<BaselineCalculationFunction> _logger;

    public BaselineCalculationFunction(
        IPatternAnalysisService patternService,
        ILogger<BaselineCalculationFunction> logger)
    {
        _patternService = patternService;
        _logger = logger;
    }

    [Function("BaselineCalculationFunction")]
    public async Task Run(
        [TimerTrigger("0 0 2 * * MON")] TimerInfo timer) // Every Monday at 2 AM
    {
        _logger.LogInformation($"Baseline calculation started at: {DateTime.UtcNow}");

        try
        {
            // Get all active CardiMembers
            var cardiMembers = await _patternService.GetActiveCardiMembersAsync();
            _logger.LogInformation($"Calculating baselines for {cardiMembers.Count} CardiMembers");

            var calculationTasks = cardiMembers.Select(async member =>
            {
                try
                {
                    // Calculate baseline using last 30 days of data
                    var baseline = await _patternService.CalculateBaselineAsync(
                        member.Id,
                        periodDays: 30);

                    // Save to database
                    await _patternService.SaveBaselineAsync(baseline);

                    _logger.LogInformation(
                        $"Baseline calculated for CardiMember {member.Id}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        $"Failed to calculate baseline for CardiMember {member.Id}");
                }
            });

            await Task.WhenAll(calculationTasks);

            _logger.LogInformation("Baseline calculation completed");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Baseline calculation function failed");
            throw;
        }
    }
}
```

#### Schedule

- **Trigger**: Timer (CRON: `0 0 2 * * MON`)
- **Frequency**: Weekly (every Monday at 2 AM)
- **Duration**: 10-30 minutes (depending on number of CardiMembers)

#### Baseline Metrics

- Average steps by day of week
- Standard deviation of steps
- Average resting heart rate
- Standard deviation of heart rate
- Average sleep duration
- Typical bedtime and wake time
- Average sleep efficiency
- Active minutes averages

### 6. ReportGenerationFunction

Generates weekly and monthly health reports for family members.

#### Implementation

```csharp
public class ReportGenerationFunction
{
    private readonly IReportService _reportService;
    private readonly INotificationService _notificationService;
    private readonly ILogger<ReportGenerationFunction> _logger;

    public ReportGenerationFunction(
        IReportService reportService,
        INotificationService notificationService,
        ILogger<ReportGenerationFunction> logger)
    {
        _reportService = reportService;
        _notificationService = notificationService;
        _logger = logger;
    }

    [Function("WeeklyReportFunction")]
    public async Task RunWeekly(
        [TimerTrigger("0 0 8 * * MON")] TimerInfo timer) // Every Monday at 8 AM
    {
        await GenerateReports(ReportPeriod.Weekly);
    }

    [Function("MonthlyReportFunction")]
    public async Task RunMonthly(
        [TimerTrigger("0 0 8 1 * *")] TimerInfo timer) // First day of month at 8 AM
    {
        await GenerateReports(ReportPeriod.Monthly);
    }

    private async Task GenerateReports(ReportPeriod period)
    {
        _logger.LogInformation($"{period} report generation started at: {DateTime.UtcNow}");

        try
        {
            var cardiMembers = await _reportService.GetActiveCardiMembersAsync();

            foreach (var member in cardiMembers)
            {
                try
                {
                    // Generate report
                    var report = await _reportService.GenerateReportAsync(member.Id, period);

                    // Get family members
                    var familyMembers = await _reportService.GetFamilyMembersAsync(member.Id);

                    // Send report to each family member
                    foreach (var family in familyMembers.Where(f => f.ReceiveReports))
                    {
                        await _notificationService.SendEmailAsync(
                            family.Email,
                            $"{period} Health Report: {member.FirstName} {member.LastName}",
                            GenerateReportEmail(report, member));
                    }

                    _logger.LogInformation($"Report sent for CardiMember {member.Id}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to generate report for {member.Id}");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Report generation failed");
            throw;
        }
    }
}
```

#### Schedules

- **Weekly Report**: Every Monday at 8 AM
- **Monthly Report**: First day of each month at 8 AM

### 7. DataCleanupFunction

Archives old data and cleans up expired records.

#### Implementation

```csharp
public class DataCleanupFunction
{
    private readonly IDataCleanupService _cleanupService;
    private readonly ILogger<DataCleanupFunction> _logger;

    public DataCleanupFunction(
        IDataCleanupService cleanupService,
        ILogger<DataCleanupFunction> logger)
    {
        _cleanupService = cleanupService;
        _logger = logger;
    }

    [Function("DataCleanupFunction")]
    public async Task Run(
        [TimerTrigger("0 0 3 * * *")] TimerInfo timer) // Daily at 3 AM
    {
        _logger.LogInformation($"Data cleanup started at: {DateTime.UtcNow}");

        try
        {
            // Archive activity logs older than 2 years
            var archivedLogs = await _cleanupService.ArchiveOldActivityLogsAsync(
                olderThanDays: 730);

            // Delete acknowledged alerts older than 90 days
            var deletedAlerts = await _cleanupService.DeleteOldAlertsAsync(
                olderThanDays: 90,
                acknowledgedOnly: true);

            // Clean up expired audit logs (retain 7 years for HIPAA)
            var cleanedAudits = await _cleanupService.ArchiveOldAuditLogsAsync(
                olderThanDays: 2555);

            _logger.LogInformation(
                $"Cleanup completed: {archivedLogs} logs archived, " +
                $"{deletedAlerts} alerts deleted, {cleanedAudits} audits archived");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Data cleanup failed");
            throw;
        }
    }
}
```

#### Schedule

- **Trigger**: Timer (CRON: `0 0 3 * * *`)
- **Frequency**: Daily at 3 AM

## Configuration

### host.json

Function host configuration:

```json
{
  "version": "2.0",
  "logging": {
    "applicationInsights": {
      "samplingSettings": {
        "isEnabled": true,
        "maxTelemetryItemsPerSecond": 20
      }
    },
    "logLevel": {
      "default": "Information",
      "Function": "Information",
      "Host": "Warning"
    }
  },
  "extensions": {
    "queues": {
      "maxPollingInterval": "00:00:02",
      "batchSize": 16,
      "maxDequeueCount": 5,
      "newBatchThreshold": 8
    },
    "http": {
      "routePrefix": "api"
    }
  },
  "concurrency": {
    "dynamicConcurrencyEnabled": true,
    "maximumFunctionConcurrency": 100
  },
  "retry": {
    "strategy": "exponentialBackoff",
    "maxRetryCount": 3,
    "minimumInterval": "00:00:05",
    "maximumInterval": "00:05:00"
  }
}
```

### local.settings.json

Local development configuration:

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "ConnectionStrings__DefaultConnection": "Server=localhost;Database=CardiTrack;Trusted_Connection=True;",
    "Fitbit__ClientId": "YOUR_CLIENT_ID",
    "Fitbit__ClientSecret": "YOUR_CLIENT_SECRET",
    "Twilio__AccountSid": "YOUR_ACCOUNT_SID",
    "Twilio__AuthToken": "YOUR_AUTH_TOKEN",
    "Twilio__FromNumber": "+1234567890",
    "SendGrid__ApiKey": "YOUR_API_KEY",
    "SendGrid__FromEmail": "noreply@carditrack.com",
    "ApplicationInsights__ConnectionString": "InstrumentationKey=..."
  }
}
```

### Azure App Settings

Production configuration in Azure:

```bash
az functionapp config appsettings set \
  --name carditrack-functions \
  --resource-group carditrack-rg \
  --settings \
    "ConnectionStrings__DefaultConnection=SERVER_CONNECTION" \
    "Fitbit__ClientId=PROD_CLIENT_ID" \
    "Twilio__AccountSid=PROD_ACCOUNT_SID"
```

## Deployment

### Local Development

```bash
# Install Azure Functions Core Tools
npm install -g azure-functions-core-tools@4

# Navigate to Functions project
cd C:\Code\Github\Carditrack\src\Functions\CardiTrack.Functions

# Restore dependencies
dotnet restore

# Build project
dotnet build

# Run locally
func start
```

### Deploy to Azure

```bash
# Login to Azure
az login

# Create Function App
az functionapp create \
  --name carditrack-functions \
  --resource-group carditrack-rg \
  --consumption-plan-location eastus \
  --runtime dotnet-isolated \
  --runtime-version 10 \
  --functions-version 4 \
  --storage-account carditrackstorage

# Deploy
func azure functionapp publish carditrack-functions
```

### CI/CD with GitHub Actions

```yaml
name: Deploy Azure Functions

on:
  push:
    branches: [main]
    paths:
      - 'src/Functions/CardiTrack.Functions/**'

jobs:
  deploy:
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v3

      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '10.0.x'

      - name: Build
        run: dotnet build src/Functions/CardiTrack.Functions --configuration Release

      - name: Publish
        run: dotnet publish src/Functions/CardiTrack.Functions --configuration Release --output ./publish

      - name: Deploy to Azure Functions
        uses: Azure/functions-action@v1
        with:
          app-name: carditrack-functions
          package: ./publish
          publish-profile: ${{ secrets.AZURE_FUNCTIONAPP_PUBLISH_PROFILE }}
```

## Monitoring and Troubleshooting

### Application Insights

View function execution logs and metrics:

```bash
# View live metrics
az monitor app-insights metrics show \
  --app carditrack-ai \
  --resource-group carditrack-rg \
  --metrics 'requests/count'

# Query logs
az monitor app-insights query \
  --app carditrack-ai \
  --analytics-query "traces | where message contains 'DeviceSync' | take 100"
```

### Key Metrics to Monitor

1. **Execution Count**: Number of function executions
2. **Success Rate**: % of successful executions
3. **Duration**: Average execution time
4. **Failures**: Failed executions and exceptions
5. **Queue Depth**: Number of messages in queues
6. **Token Refresh Rate**: % of successful token refreshes

### Common Issues

**Issue: Function timeout**
```json
// Increase timeout in host.json
{
  "functionTimeout": "00:10:00"
}
```

**Issue: Queue messages piling up**
```bash
# Check queue depth
az storage queue stats --name pattern-analysis-queue

# Solution: Increase batch size in host.json
{
  "extensions": {
    "queues": {
      "batchSize": 32
    }
  }
}
```

**Issue: High memory usage**
```bash
# Scale up Function App plan
az functionapp plan update \
  --name carditrack-plan \
  --resource-group carditrack-rg \
  --sku P1V2
```

**Issue: Token refresh failures**
- Check device API credentials
- Verify refresh token is not expired
- Check network connectivity to device APIs

### Debugging

Enable detailed logging:

```json
{
  "logging": {
    "logLevel": {
      "default": "Debug",
      "Function": "Debug"
    }
  }
}
```

View function logs:

```bash
# Stream logs
func azure functionapp logstream carditrack-functions

# Or in Azure Portal
az webapp log tail --name carditrack-functions --resource-group carditrack-rg
```

## Scaling and Performance

### Auto-Scaling

Azure Functions automatically scale based on:
- Queue depth (for queue-triggered functions)
- CPU usage
- Memory usage
- Request rate

### Performance Optimization

1. **Batch Processing**: Process multiple items per execution
2. **Parallel Execution**: Use `Task.WhenAll` for concurrent operations
3. **Connection Pooling**: Reuse HttpClient and DbContext
4. **Caching**: Cache reference data in memory
5. **Queue Partitioning**: Separate queues for different priorities

### Cost Optimization

```bash
# Monitor costs
az consumption usage list \
  --resource-group carditrack-rg \
  --start-date 2026-01-01 \
  --end-date 2026-01-31
```

**Cost Reduction Strategies**:
- Use consumption plan for variable workloads
- Optimize execution time to reduce compute costs
- Use queue batching to reduce executions
- Archive old data to reduce storage costs

## Testing

### Unit Testing Functions

```csharp
public class DeviceSyncFunctionTests
{
    private readonly Mock<IDeviceService> _mockDeviceService;
    private readonly Mock<ILogger<DeviceSyncFunction>> _mockLogger;
    private readonly DeviceSyncFunction _function;

    public DeviceSyncFunctionTests()
    {
        _mockDeviceService = new Mock<IDeviceService>();
        _mockLogger = new Mock<ILogger<DeviceSyncFunction>>();
        _function = new DeviceSyncFunction(_mockDeviceService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Run_Success_SyncsAllDevices()
    {
        // Arrange
        var connections = new List<DeviceConnection>
        {
            new DeviceConnection { Id = Guid.NewGuid(), DeviceType = "Fitbit" }
        };
        _mockDeviceService.Setup(x => x.GetActiveConnectionsAsync())
            .ReturnsAsync(connections);

        // Act
        await _function.Run(null);

        // Assert
        _mockDeviceService.Verify(x => x.SyncDeviceDataAsync(It.IsAny<DeviceConnection>()), Times.Once);
    }
}
```

### Integration Testing

```csharp
public class FunctionIntegrationTests : IClassFixture<FunctionTestFixture>
{
    private readonly FunctionTestFixture _fixture;

    public FunctionIntegrationTests(FunctionTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task DeviceSync_EndToEnd_Success()
    {
        // Create test device connection
        var connection = await _fixture.CreateTestDeviceConnection();

        // Trigger function
        await _fixture.TriggerDeviceSyncFunction();

        // Wait for processing
        await Task.Delay(5000);

        // Verify data was synced
        var activityLog = await _fixture.GetLatestActivityLog(connection.CardiMemberId);
        Assert.NotNull(activityLog);
    }
}
```

## Best Practices

1. **Idempotency**: Ensure functions can be safely retried
2. **Error Handling**: Implement comprehensive try-catch blocks
3. **Logging**: Log all important operations and errors
4. **Timeouts**: Set appropriate timeouts for external API calls
5. **Monitoring**: Use Application Insights for observability
6. **Security**: Store secrets in Azure Key Vault
7. **Testing**: Write unit and integration tests
8. **Documentation**: Document function behavior and dependencies

## Related Documentation

- [API Documentation](../api/README.md)
- [Web Dashboard Documentation](../web/README.md)
- [Mobile App Documentation](../mobile/README.md)
- [Infrastructure Guide](../../INFRASTRUCTURE.md)
- [Azure Functions Documentation](https://learn.microsoft.com/azure/azure-functions/)

## Support

For Azure Functions issues, contact: functions-support@carditrack.com
