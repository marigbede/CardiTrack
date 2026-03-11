using CardiTrack.Application.Interfaces.Repositories;
using CardiTrack.Application.Interfaces.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CardiTrack.Functions;

public class SyncWearableDataFunction
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IDeviceConnectionRepository _deviceConnections;
    private readonly ILogger<SyncWearableDataFunction> _logger;

    public SyncWearableDataFunction(
        IServiceProvider serviceProvider,
        IDeviceConnectionRepository deviceConnections,
        ILogger<SyncWearableDataFunction> logger)
    {
        _serviceProvider = serviceProvider;
        _deviceConnections = deviceConnections;
        _logger = logger;
    }

    [Function("SyncWearableData")]
    public async Task Run([TimerTrigger("0 */30 * * * *")] TimerInfo timer)
    {
        _logger.LogInformation("SyncWearableData triggered at {Time}", DateTime.UtcNow);

        var connections = await _deviceConnections.GetDueForSyncAsync(thresholdMinutes: 30);

        var successCount = 0;
        var failureCount = 0;

        foreach (var connection in connections)
        {
            var syncService = _serviceProvider.GetKeyedService<IDeviceSyncService>(connection.DeviceType);

            if (syncService is null)
            {
                _logger.LogWarning(
                    "No sync service registered for DeviceType {DeviceType}. " +
                    "Skipping DeviceConnection {Id}.",
                    connection.DeviceType, connection.Id);
                continue;
            }

            try
            {
                await syncService.SyncCardiMemberAsync(connection);
                successCount++;
                _logger.LogInformation(
                    "Synced DeviceConnection {Id} (DeviceType={DeviceType}) for CardiMember {CardiMemberId}.",
                    connection.Id, connection.DeviceType, connection.CardiMemberId);
            }
            catch (Exception ex)
            {
                failureCount++;
                _logger.LogError(ex,
                    "Failed to sync DeviceConnection {Id} (DeviceType={DeviceType}) for CardiMember {CardiMemberId}.",
                    connection.Id, connection.DeviceType, connection.CardiMemberId);
            }
        }

        _logger.LogInformation(
            "SyncWearableData complete. Success: {Success}, Failed: {Failed}.",
            successCount, failureCount);
    }
}
