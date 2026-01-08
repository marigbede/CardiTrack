using System.ComponentModel.DataAnnotations;

namespace CardiTrack.Domain.Enums;

public enum Gender
{
    [Display(Name = "Male")]
    Male = 1,

    [Display(Name = "Female")]
    Female = 2,

    [Display(Name = "Other")]
    Other = 3,

    [Display(Name = "Prefer Not to Say")]
    PreferNotToSay = 4
}
