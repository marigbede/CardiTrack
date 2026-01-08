namespace CardiTrack.Domain.Interfaces;

public interface IEntity
{
    Guid Id { get; set; }
    DateTime CreatedDate { get; set; }
    DateTime? UpdatedDate { get; set; }
}
