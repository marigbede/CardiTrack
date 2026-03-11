using CardiTrack.Domain.Entities;
using CardiTrack.Infrastructure.Persistence;
using CardiTrack.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CardiTrack.Infrastructure.Repositories;

public class OrganizationRepository : Repository<Organization>, IOrganizationRepository
{
    public OrganizationRepository(CardiTrackDbContext context) : base(context)
    {
    }

    public async Task<Organization?> GetWithSubscriptionAsync(Guid id)
    {
        return await _dbSet
            .Include(o => o.Subscription)
            .FirstOrDefaultAsync(o => o.Id == id);
    }
}
