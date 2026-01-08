using System.ComponentModel.DataAnnotations;

namespace CardiTrack.Domain.Enums;

public enum BillingCycle
{
    [Display(Name = "Monthly")]
    Monthly = 1,

    [Display(Name = "Annual")]
    Annual = 2
}
