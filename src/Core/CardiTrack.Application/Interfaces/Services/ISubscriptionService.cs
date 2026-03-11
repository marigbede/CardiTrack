using CardiTrack.Domain.Enums;

namespace CardiTrack.Application.Interfaces.Services;

public interface ISubscriptionService
{
    Task CreateTrialSubscriptionAsync(Guid organizationId, OrganizationType orgType);
}
