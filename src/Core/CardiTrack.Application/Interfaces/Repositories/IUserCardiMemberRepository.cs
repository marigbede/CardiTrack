using CardiTrack.Domain.Entities;

namespace CardiTrack.Application.Interfaces.Repositories;

public interface IUserCardiMemberRepository : IRepository<UserCardiMember>
{
    Task<IEnumerable<UserCardiMember>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<UserCardiMember>> GetByCardiMemberIdAsync(Guid cardiMemberId);
}
