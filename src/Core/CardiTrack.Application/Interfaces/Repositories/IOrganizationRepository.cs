using CardiTrack.Domain.Entities;

namespace CardiTrack.Application.Interfaces.Repositories;

public interface IOrganizationRepository : IRepository<Organization>
{
    Task<Organization?> GetWithSubscriptionAsync(Guid id);
}
