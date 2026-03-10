using CardiTrack.Domain.Common;
using CardiTrack.Domain.Enums;
using CardiTrack.Domain.Interfaces;

namespace CardiTrack.Domain.Entities;

public class CardiMember : BaseEntity, ISoftDeletable
{
    public Guid OrganizationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? MedicalNotes { get; set; } // Encrypted in database
    public DateTime? LastSyncDate { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<UserCardiMember> UserCardiMembers { get; set; } = new List<UserCardiMember>();
    public ICollection<DeviceConnection> DeviceConnections { get; set; } = new List<DeviceConnection>();
    public ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
    public ICollection<Alert> Alerts { get; set; } = new List<Alert>();
    public ICollection<PatternBaseline> PatternBaselines { get; set; } = new List<PatternBaseline>();
}
