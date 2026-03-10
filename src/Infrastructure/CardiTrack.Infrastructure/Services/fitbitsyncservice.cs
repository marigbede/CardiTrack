using CardiTrack.Application.Interfaces.Repositories;
using CardiTrack.Application.Interfaces.Services;
using CardiTrack.Domain.Entities;
using CardiTrack.Domain.Enums;
using CardiTrack.Infrastructure.ExternalClients;
using CardiTrack.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace CardiTrack.Infrastructure.Services;

public class FitbitSyncService : IFitbitSyncService
{
    private readonly IOAuthTokenRefreshService _tokenRefresh;
    private readonly IFitbitApiClient _fitbitApi;
    private readonly IDeviceConnectionRepository _deviceConnections;
    private readonly IActivityLogRepository _activityLogs;
    private readonly IUnitOfWork _unitOfWork;
    private readonly List<DeviceProviderSettings> _providers;

    public FitbitSyncService(
        IOAuthTokenRefreshService tokenRefresh,
        IFitbitApiClient fitbitApi,
        IDeviceConnectionRepository deviceConnections,
        IActivityLogRepository activityLogs,
        IUnitOfWork unitOfWork,
        IOptions<List<DeviceProviderSettings>> providers)
    {
        _tokenRefresh = tokenRefresh;
        _fitbitApi = fitbitApi;
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

        // Fitbit finalises the previous day's data — sync yesterday
        var targetDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));

        try
        {
            var activities = await _fitbitApi.GetActivitiesAsync(accessToken, targetDate);
            var heartRate = await _fitbitApi.GetHeartRateAsync(accessToken, targetDate);
            var sleep = await _fitbitApi.GetSleepAsync(accessToken, targetDate);

            var log = new ActivityLog
            {
                Id = Guid.NewGuid(),
                CardiMemberId = connection.CardiMemberId,
                DeviceConnectionId = connection.Id,
                DataSource = connection.DeviceType,
                Date = targetDate,

                // Activity
                Steps = activities.Steps,
                Distance = activities.DistanceKm,
                ActiveMinutes = activities.ActiveMinutes,
                SedentaryMinutes = activities.SedentaryMinutes,
                Floors = activities.Floors,
                CaloriesBurned = activities.CaloriesBurned,

                // Heart rate
                RestingHeartRate = heartRate.RestingHeartRate,
                AvgHeartRate = heartRate.AvgHeartRate,
                MaxHeartRate = heartRate.MaxHeartRate,
                MinHeartRate = heartRate.MinHeartRate,

                // Sleep
                SleepMinutes = sleep.TotalSleepMinutes,
                SleepEfficiency = sleep.SleepEfficiency,
                SleepStartTime = sleep.SleepStartTime,
                SleepEndTime = sleep.SleepEndTime,
                DeepSleepMinutes = sleep.DeepSleepMinutes,
                LightSleepMinutes = sleep.LightSleepMinutes,
                RemSleepMinutes = sleep.RemSleepMinutes,
                AwakeMinutes = sleep.AwakeMinutes
            };

            await _activityLogs.UpsertAsync(log);
            await _unitOfWork.SaveChangesAsync();
            await _deviceConnections.UpdateLastSyncDateAsync(connection.Id, DateTime.UtcNow);
        }
        catch (FitbitApiException)
        {
            await _deviceConnections.UpdateStatusAsync(connection.Id, ConnectionStatus.SyncError);
            throw;
        }
    }
}
