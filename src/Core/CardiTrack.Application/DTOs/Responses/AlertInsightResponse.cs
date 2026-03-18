using CardiTrack.Domain.Enums;

namespace CardiTrack.Application.DTOs.Responses;

public class AlertInsightResponse
{
    public required Guid AlertId { get; init; }
    public required string Explanation { get; init; }
    public required AlertSeverity Severity { get; init; }
    public required string RecommendedAction { get; init; }
}
