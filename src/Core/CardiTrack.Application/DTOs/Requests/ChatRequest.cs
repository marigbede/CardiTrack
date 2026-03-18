using CardiTrack.Application.DTOs.Common;

namespace CardiTrack.Application.DTOs.Requests;

public class ChatRequest
{
    public required Guid CardiMemberId { get; init; }
    public IReadOnlyList<ChatMessage> History { get; init; } = [];
    public required string Message { get; init; }
}
