using CardiTrack.Application.Interfaces.Repositories;
using CardiTrack.Domain.Entities;
using CardiTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CardiTrack.Infrastructure.Repositories;

public class SubscriptionRepository : Repository<Subscription>, ISubscriptionRepository
{
    public SubscriptionRepository(CardiTrackDbContext context) : base(context)
    {
    }

    public async Task<Subscription?> GetByOrganizationIdAsync(Guid organizationId)
    {
        return await _dbSet.FirstOrDefaultAsync(s => s.OrganizationId == organizationId);
    }
}
