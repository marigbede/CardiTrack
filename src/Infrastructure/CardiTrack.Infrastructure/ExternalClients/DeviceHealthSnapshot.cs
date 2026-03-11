namespace CardiTrack.Infrastructure.ExternalClients;

/// <summary>
/// Provider-neutral health data snapshot for one member for one day.
/// Fields map 1:1 to ActivityLog nullable columns — providers return null for metrics they don't support.
/// </summary>
public record DeviceHealthSnapshot(
    // Activity
    int? Steps,
    decimal? DistanceKm,
    int? ActiveMinutes,
    int? SedentaryMinutes,
    int? Floors,
    int? CaloriesBurned,
    // Heart rate
    int? RestingHeartRate,
    int? AvgHeartRate,
    int? MaxHeartRate,
    int? MinHeartRate,
    // Sleep
    int? TotalSleepMinutes,
    int? SleepEfficiency,
    DateTime? SleepStartTime,
    DateTime? SleepEndTime,
    int? DeepSleepMinutes,
    int? LightSleepMinutes,
    int? RemSleepMinutes,
    int? AwakeMinutes
);
