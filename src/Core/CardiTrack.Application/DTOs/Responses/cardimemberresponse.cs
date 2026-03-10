using CardiTrack.Domain.Enums;

namespace CardiTrack.Application.DTOs.Responses;

public class CardiMemberResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public int Age { get; set; }
    public Gender Gender { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public RelationshipType Relationship { get; set; }
    public bool IsPrimaryCaregiver { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
}
