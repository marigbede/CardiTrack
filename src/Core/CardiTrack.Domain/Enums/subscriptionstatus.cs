using System.ComponentModel.DataAnnotations;

namespace CardiTrack.Domain.Enums;

public enum SubscriptionStatus
{
    [Display(Name = "Trial")]
    Trial = 1,

    [Display(Name = "Active")]
    Active = 2,

    [Display(Name = "Past Due")]
    PastDue = 3,

    [Display(Name = "Cancelled")]
    Cancelled = 4,

    [Display(Name = "Suspended")]
    Suspended = 5
}
