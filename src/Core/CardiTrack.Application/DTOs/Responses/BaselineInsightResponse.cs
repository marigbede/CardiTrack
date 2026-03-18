namespace CardiTrack.Application.DTOs.Responses;

public class BaselineInsightResponse
{
    public required Guid CardiMemberId { get; init; }
    public required string Summary { get; init; }
    public required IReadOnlyList<string> KeyFindings { get; init; }
    public required DateTimeOffset GeneratedAt { get; init; }
}
