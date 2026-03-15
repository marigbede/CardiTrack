using CardiTrack.Application.Interfaces.Repositories;
using CardiTrack.Application.Interfaces.Services;
using CardiTrack.Domain.Entities;
using CardiTrack.Domain.Enums;

namespace CardiTrack.Application.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly IUnitOfWork _unitOfWork;

    public SubscriptionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task CreateTrialSubscriptionAsync(Guid organizationId, OrganizationType orgType)
    {
        var subscription = new Subscription
        {
            OrganizationId = organizationId,
            Tier = SubscriptionTier.Complete,
            Status = SubscriptionStatus.Trial,
            StartDate = DateTime.UtcNow,
            TrialEndDate = DateTime.UtcNow.AddDays(30),
            BillingCycle = BillingCycle.Monthly,
            Price = 0,
            Currency = "USD",
            MaxCardiMembers = orgType == OrganizationType.Family ? 5 : 50,
            MaxUsers = orgType == OrganizationType.Family ? 1 : 20,
            Features = "{\"deviceTypes\":[\"All\"],\"alertTypes\":[\"All\"],\"realTimeSync\":true}"
        };

        await _unitOfWork.Subscriptions.AddAsync(subscription);
    }
}
