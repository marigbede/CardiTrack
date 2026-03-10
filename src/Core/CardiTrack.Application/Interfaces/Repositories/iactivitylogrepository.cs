using CardiTrack.Domain.Entities;

namespace CardiTrack.Application.Interfaces.Repositories;

public interface IActivityLogRepository : IRepository<ActivityLog>
{
    Task UpsertAsync(ActivityLog log);
    Task<IEnumerable<ActivityLog>> GetByCardiMemberAndDateRangeAsync(Guid cardiMemberId, DateOnly from, DateOnly to);
}
