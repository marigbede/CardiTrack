using CardiTrack.Application.Interfaces.Repositories;
using CardiTrack.Domain.Entities;
using CardiTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CardiTrack.Infrastructure.Repositories;

public class UserCardiMemberRepository : Repository<UserCardiMember>, IUserCardiMemberRepository
{
    public UserCardiMemberRepository(CardiTrackDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<UserCardiMember>> GetByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .Where(ucm => ucm.UserId == userId)
            .Include(ucm => ucm.CardiMember)
            .ToListAsync();
    }

    public async Task<IEnumerable<UserCardiMember>> GetByCardiMemberIdAsync(Guid cardiMemberId)
    {
        return await _dbSet
            .Where(ucm => ucm.CardiMemberId == cardiMemberId)
            .Include(ucm => ucm.User)
            .ToListAsync();
    }
}
