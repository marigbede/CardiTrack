namespace CardiTrack.Application.DTOs.Responses;

public class ErrorResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<ValidationError> Errors { get; set; } = new();
    public string? TraceId { get; set; }
    public DateTime Timestamp { get; set; }
}

public class ValidationError
{
    public string Field { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
