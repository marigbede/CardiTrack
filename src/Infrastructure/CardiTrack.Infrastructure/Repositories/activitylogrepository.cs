using CardiTrack.Application.Interfaces.Repositories;
using CardiTrack.Domain.Entities;
using CardiTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CardiTrack.Infrastructure.Repositories;

public class ActivityLogRepository : Repository<ActivityLog>, IActivityLogRepository
{
    public ActivityLogRepository(CardiTrackDbContext context) : base(context)
    {
    }

    public async Task UpsertAsync(ActivityLog log)
    {
        var existing = await _dbSet
            .FirstOrDefaultAsync(al => al.DeviceConnectionId == log.DeviceConnectionId
                                       && al.Date == log.Date);

        if (existing is null)
        {
            await _dbSet.AddAsync(log);
        }
        else
        {
            existing.Steps = log.Steps;
            existing.Distance = log.Distance;
            existing.ActiveMinutes = log.ActiveMinutes;
            existing.SedentaryMinutes = log.SedentaryMinutes;
            existing.Floors = log.Floors;
            existing.CaloriesBurned = log.CaloriesBurned;
            existing.RestingHeartRate = log.RestingHeartRate;
            existing.AvgHeartRate = log.AvgHeartRate;
            existing.MaxHeartRate = log.MaxHeartRate;
            existing.MinHeartRate = log.MinHeartRate;
            existing.SleepMinutes = log.SleepMinutes;
            existing.SleepStartTime = log.SleepStartTime;
            existing.SleepEndTime = log.SleepEndTime;
            existing.SleepEfficiency = log.SleepEfficiency;
            existing.DeepSleepMinutes = log.DeepSleepMinutes;
            existing.LightSleepMinutes = log.LightSleepMinutes;
            existing.RemSleepMinutes = log.RemSleepMinutes;
            existing.AwakeMinutes = log.AwakeMinutes;
            _dbSet.Update(existing);
        }
    }

    public async Task<IEnumerable<ActivityLog>> GetByCardiMemberAndDateRangeAsync(
        Guid cardiMemberId, DateOnly from, DateOnly to)
    {
        return await _dbSet
            .Where(al => al.CardiMemberId == cardiMemberId
                         && al.Date >= from
                         && al.Date <= to)
            .OrderBy(al => al.Date)
            .ToListAsync();
    }
}
