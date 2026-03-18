namespace CardiTrack.Application.DTOs.Common;

public enum ChatRole { User, Model }

public class ChatMessage
{
    public required ChatRole Role { get; init; }
    public required string Content { get; init; }
}
