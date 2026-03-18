using CardiTrack.Domain.Entities;

namespace CardiTrack.Application.Interfaces.Repositories;

public interface IAlertRepository : IRepository<Alert>
{
    Task<IEnumerable<Alert>> GetByCardiMemberAsync(Guid cardiMemberId, bool activeOnly);
    Task<Alert?> GetByIdWithCardiMemberAsync(Guid alertId);
}
