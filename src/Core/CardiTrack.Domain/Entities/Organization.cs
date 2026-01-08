using CardiTrack.Domain.Common;
using CardiTrack.Domain.Enums;
using CardiTrack.Domain.Interfaces;

namespace CardiTrack.Domain.Entities;

public class Organization : BaseEntity, ISoftDeletable
{
    public string Name { get; set; } = string.Empty;
    public OrganizationType Type { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties (not mapped as FK, just for reference)
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<CardiMember> CardiMembers { get; set; } = new List<CardiMember>();
    public Subscription? Subscription { get; set; }
}
