using CardiTrack.Domain.Common;
using CardiTrack.Domain.Enums;
using CardiTrack.Domain.Interfaces;

namespace CardiTrack.Domain.Entities;

public class User : BaseEntity, ISoftDeletable
{
    public Guid OrganizationId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public UserRole Role { get; set; } = UserRole.Member;
    public DateTime? LastLoginDate { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<UserCardiMember> UserCardiMembers { get; set; } = new List<UserCardiMember>();
}
