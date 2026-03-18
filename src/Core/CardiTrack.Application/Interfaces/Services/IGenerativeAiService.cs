using CardiTrack.Application.DTOs.Common;

namespace CardiTrack.Application.Interfaces.Services;

/// <summary>
/// High-level service for report generation and conversational AI.
/// Active provider is config-driven via AI:GeneralProvider.
/// </summary>
public interface IGenerativeAiService
{
    Task<string> GenerateAsync(string prompt, CancellationToken ct = default);
    Task<string> ChatAsync(IReadOnlyList<ChatMessage> history, string userMessage, CancellationToken ct = default);
}
