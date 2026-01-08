using CardiTrack.Domain.Common;
using CardiTrack.Domain.Enums;

namespace CardiTrack.Domain.Entities;

public class Subscription : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public SubscriptionTier Tier { get; set; }
    public SubscriptionStatus Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? TrialEndDate { get; set; }
    public BillingCycle BillingCycle { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = "USD";

    // Tier Limits
    public int MaxCardiMembers { get; set; }
    public int MaxUsers { get; set; } // Hidden for Family type

    // JSON: { "deviceTypes": ["Fitbit", "AppleWatch"], "alertTypes": ["All"], "realTimeSync": true }
    public string Features { get; set; } = "{}";

    // JSON: { "last4": "4242", "brand": "Visa", "expiryMonth": 12, "expiryYear": 2025 }
    public string? PaymentMethod { get; set; }

    public Subscription()
    {
        StartDate = DateTime.UtcNow;
        Status = SubscriptionStatus.Trial;
    }
}
