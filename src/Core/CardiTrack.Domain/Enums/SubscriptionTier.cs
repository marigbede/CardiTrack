using System.ComponentModel.DataAnnotations;

namespace CardiTrack.Domain.Enums;

public enum SubscriptionTier
{
    [Display(Name = "Basic")]
    Basic = 1,

    [Display(Name = "Complete")]
    Complete = 2,

    [Display(Name = "Plus")]
    Plus = 3
}
