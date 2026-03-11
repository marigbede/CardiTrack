namespace CardiTrack.Domain.Interfaces;

public interface ISoftDeletable
{
    bool IsActive { get; set; }
}
