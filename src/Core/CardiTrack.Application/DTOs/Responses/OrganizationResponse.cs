using CardiTrack.Domain.Enums;

namespace CardiTrack.Application.DTOs.Responses;

public class OrganizationResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public OrganizationType Type { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public SubscriptionResponse? Subscription { get; set; }
}

public class SubscriptionResponse
{
    public Guid Id { get; set; }
    public SubscriptionTier Tier { get; set; }
    public SubscriptionStatus Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? TrialEndDate { get; set; }
    public int MaxCardiMembers { get; set; }
    public int MaxUsers { get; set; }
}
