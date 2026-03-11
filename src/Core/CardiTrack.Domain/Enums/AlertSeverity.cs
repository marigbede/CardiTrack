using System.ComponentModel.DataAnnotations;

namespace CardiTrack.Domain.Enums;

public enum AlertSeverity
{
    [Display(Name = "Green")]
    Green = 1,    // Informational

    [Display(Name = "Yellow")]
    Yellow = 2,   // Minor concern

    [Display(Name = "Orange")]
    Orange = 3,   // Moderate concern

    [Display(Name = "Red")]
    Red = 4       // Urgent attention needed
}
