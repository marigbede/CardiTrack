using CardiTrack.Domain.Entities;
using CardiTrack.Infrastructure.Persistence;
using CardiTrack.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CardiTrack.Infrastructure.Repositories;

public class CardiMemberRepository : Repository<CardiMember>, ICardiMemberRepository
{
    public CardiMemberRepository(CardiTrackDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<CardiMember>> GetByOrganizationIdAsync(Guid organizationId)
    {
        return await _dbSet
            .Where(cm => cm.OrganizationId == organizationId && cm.IsActive)
            .ToListAsync();
    }

    public async Task<CardiMember?> GetWithRelationshipsAsync(Guid id)
    {
        return await _dbSet
            .Include(cm => cm.UserCardiMembers)
            .FirstOrDefaultAsync(cm => cm.Id == id);
    }
}
