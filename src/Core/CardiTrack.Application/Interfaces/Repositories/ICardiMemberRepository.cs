using CardiTrack.Domain.Entities;

namespace CardiTrack.Application.Interfaces.Repositories;

public interface ICardiMemberRepository : IRepository<CardiMember>
{
    Task<IEnumerable<CardiMember>> GetByOrganizationIdAsync(Guid organizationId);
    Task<CardiMember?> GetWithRelationshipsAsync(Guid id);
}
