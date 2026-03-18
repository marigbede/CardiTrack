using CardiTrack.Application.Interfaces.Repositories;
using CardiTrack.Domain.Entities;
using CardiTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CardiTrack.Infrastructure.Repositories;

public class AlertRepository : Repository<Alert>, IAlertRepository
{
    public AlertRepository(CardiTrackDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Alert>> GetByCardiMemberAsync(Guid cardiMemberId, bool activeOnly)
    {
        var query = _dbSet.Where(a => a.CardiMemberId == cardiMemberId);

        if (activeOnly)
            query = query.Where(a => a.IsActive);

        return await query.OrderByDescending(a => a.TriggeredDate).ToListAsync();
    }

    public async Task<Alert?> GetByIdWithCardiMemberAsync(Guid alertId)
    {
        return await _dbSet.FirstOrDefaultAsync(a => a.Id == alertId);
    }
}
