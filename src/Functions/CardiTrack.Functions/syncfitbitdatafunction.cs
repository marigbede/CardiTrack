using CardiTrack.Application.Interfaces.Repositories;
using CardiTrack.Application.Interfaces.Services;
using CardiTrack.Domain.Enums;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CardiTrack.Functions;

public class SyncFitbitDataFunction
{
    private readonly IFitbitSyncService _syncService;
    private readonly IDeviceConnectionRepository _deviceConnections;
    private readonly ILogger<SyncFitbitDataFunction> _logger;

    public SyncFitbitDataFunction(
        IFitbitSyncService syncService,
        IDeviceConnectionRepository deviceConnections,
        ILogger<SyncFitbitDataFunction> logger)
    {
        _syncService = syncService;
        _deviceConnections = deviceConnections;
        _logger = logger;
    }

    [Function("SyncFitbitData")]
    public async Task Run([TimerTrigger("0 */30 * * * *")] TimerInfo timer)
    {
        _logger.LogInformation("SyncFitbitData triggered at {Time}", DateTime.UtcNow);

        var connections = await _deviceConnections.GetDueForSyncAsync(thresholdMinutes: 30);
        var fitbitConnections = connections
            .Where(c => c.DeviceType == DeviceType.Fitbit)
            .ToList();

        _logger.LogInformation("Found {Count} Fitbit connections due for sync.", fitbitConnections.Count);

        var successCount = 0;
        var failureCount = 0;

        foreach (var connection in fitbitConnections)
        {
            try
            {
                await _syncService.SyncCardiMemberAsync(connection);
                successCount++;
                _logger.LogInformation(
                    "Synced DeviceConnection {Id} for CardiMember {CardiMemberId}.",
                    connection.Id, connection.CardiMemberId);
            }
            catch (Exception ex)
            {
                failureCount++;
                _logger.LogError(ex,
                    "Failed to sync DeviceConnection {Id} for CardiMember {CardiMemberId}.",
                    connection.Id, connection.CardiMemberId);
            }
        }

        _logger.LogInformation(
            "SyncFitbitData complete. Success: {Success}, Failed: {Failed}.",
            successCount, failureCount);
    }
}
