using System.ComponentModel.DataAnnotations;

namespace CardiTrack.Domain.Enums;

public enum DeviceType
{
    [Display(Name = "Fitbit")]
    Fitbit = 1,

    [Display(Name = "Apple Watch")]
    AppleWatch = 2,

    [Display(Name = "Garmin")]
    Garmin = 3,

    [Display(Name = "Samsung")]
    Samsung = 4,

    [Display(Name = "Withings")]
    Withings = 5,

    [Display(Name = "Oura")]
    Oura = 6,

    [Display(Name = "Whoop")]
    Whoop = 7,

    [Display(Name = "Other")]
    Other = 99
}
