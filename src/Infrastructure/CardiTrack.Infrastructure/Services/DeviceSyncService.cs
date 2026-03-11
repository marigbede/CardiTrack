using CardiTrack.Application.Interfaces.Repositories;
using CardiTrack.Application.Interfaces.Services;
using CardiTrack.Domain.Entities;
using CardiTrack.Domain.Enums;
using CardiTrack.Infrastructure.ExternalClients;
using CardiTrack.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace CardiTrack.Infrastructure.Services;

/// <summary>
/// Generic sync service that works with any IDeviceApiClient implementation.
/// Register one instance per provider using .NET keyed services keyed on DeviceType.
/// </summary>
public class DeviceSyncService : IDeviceSyncService
{
    private readonly IOAuthTokenRefreshService _tokenRefresh;
    private readonly IDeviceApiClient _deviceApi;
    private readonly IDeviceConnectionRepository _deviceConnections;
    private readonly IActivityLogRepository _activityLogs;
    private readonly IUnitOfWork _unitOfWork;
    private readonly List<DeviceProviderSettings> _providers;

    public DeviceSyncService(
        IOAuthTokenRefreshService tokenRefresh,
        IDeviceApiClient deviceApi,
        IDeviceConnectionRepository deviceConnections,
        IActivityLogRepository activityLogs,
        IUnitOfWork unitOfWork,
        IOptions<List<DeviceProviderSettings>> providers)
    {
        _tokenRefresh = tokenRefresh;
        _deviceApi = deviceApi;
        _deviceConnections = deviceConnections;
        _activityLogs = activityLogs;
        _unitOfWork = unitOfWork;
        _providers = providers.Value;
    }

    public async Task SyncCardiMemberAsync(DeviceConnection connection)
    {
        var providerConfig = _providers
            .FirstOrDefault(p => p.Provider.Equals(
                connection.DeviceType.ToString(), StringComparison.OrdinalIgnoreCase))
            ?? throw new InvalidOperationException(
                $"No provider config found for device type '{connection.DeviceType}'.");

        string accessToken;
        try
        {
            accessToken = await _tokenRefresh.RefreshIfExpiredAsync(connection, providerConfig);
        }
        catch (Exception)
        {
            // RefreshIfExpiredAsync already updated status; propagate so the caller can log
            throw;
        }

        // Most wearable providers finalise the previous day's data — sync yesterday
        var targetDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));

        try
        {
            var snapshot = await _deviceApi.GetHealthSnapshotAsync(accessToken, targetDate);

            var log = new ActivityLog
            {
                Id = Guid.NewGuid(),
                CardiMemberId = connection.CardiMemberId,
                DeviceConnectionId = connection.Id,
                DataSource = connection.DeviceType,
                Date = targetDate,

                // Activity
                Steps = snapshot.Steps,
                Distance = snapshot.DistanceKm,
                ActiveMinutes = snapshot.ActiveMinutes,
                SedentaryMinutes = snapshot.SedentaryMinutes,
                Floors = snapshot.Floors,
                CaloriesBurned = snapshot.CaloriesBurned,

                // Heart rate
                RestingHeartRate = snapshot.RestingHeartRate,
                AvgHeartRate = snapshot.AvgHeartRate,
                MaxHeartRate = snapshot.MaxHeartRate,
                MinHeartRate = snapshot.MinHeartRate,

                // Sleep
                SleepMinutes = snapshot.TotalSleepMinutes,
                SleepEfficiency = snapshot.SleepEfficiency,
                SleepStartTime = snapshot.SleepStartTime,
                SleepEndTime = snapshot.SleepEndTime,
                DeepSleepMinutes = snapshot.DeepSleepMinutes,
                LightSleepMinutes = snapshot.LightSleepMinutes,
                RemSleepMinutes = snapshot.RemSleepMinutes,
                AwakeMinutes = snapshot.AwakeMinutes
            };

            await _activityLogs.UpsertAsync(log);
            await _unitOfWork.SaveChangesAsync();
            await _deviceConnections.UpdateLastSyncDateAsync(connection.Id, DateTime.UtcNow);
        }
        catch (Exception ex) when (IsProviderApiException(ex))
        {
            await _deviceConnections.UpdateStatusAsync(connection.Id, ConnectionStatus.SyncError);
            throw;
        }
    }

    /// <summary>
    /// Returns true for exceptions that represent a provider API failure (as opposed to
    /// infrastructure failures like network timeouts). Override in provider-specific subclasses
    /// if needed, or broaden to catch a common base exception type.
    /// </summary>
    protected virtual bool IsProviderApiException(Exception ex) =>
        ex is FitbitApiException;
}
