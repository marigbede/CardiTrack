using System.ComponentModel.DataAnnotations;

namespace CardiTrack.Domain.Enums;

public enum RelationshipType
{
    [Display(Name = "Self")]
    Self = 1,

    [Display(Name = "Parent")]
    Parent = 2,

    [Display(Name = "Spouse")]
    Spouse = 3,

    [Display(Name = "Grandparent")]
    Grandparent = 4,

    [Display(Name = "Sibling")]
    Sibling = 5,

    [Display(Name = "Child")]
    Child = 6,

    [Display(Name = "Other")]
    Other = 99
}
