using System.ComponentModel.DataAnnotations;

namespace CardiTrack.Domain.Enums;

public enum OrganizationType
{
    [Display(Name = "Family Account")]
    Family = 1,

    [Display(Name = "Business Account")]
    Business = 2
}
