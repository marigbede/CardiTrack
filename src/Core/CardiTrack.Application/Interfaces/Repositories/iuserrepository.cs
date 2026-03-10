using CardiTrack.Domain.Entities;

namespace CardiTrack.Application.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByAuth0UserIdAsync(string auth0UserId);
    Task<User?> GetByEmailAsync(string email);
    Task UpdateLastLoginAsync(Guid userId);
}
