using System.ComponentModel.DataAnnotations;

namespace CardiTrack.Domain.Enums;

public enum UserRole
{
    [Display(Name = "Member")]
    Member = 1,      // Default for Family type

    [Display(Name = "Administrator")]
    Admin = 2,       // Business admin

    [Display(Name = "Staff Member")]
    Staff = 3        // Business staff
}
