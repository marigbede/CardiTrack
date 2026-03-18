using CardiTrack.Application.DTOs.Common;

namespace CardiTrack.Application.Interfaces.Clients;

/// <summary>
/// Common interface for all external AI/LLM provider clients.
/// Register with a keyed DI slot matching AI:GeneralProvider or AI:MedicalProvider.
/// </summary>
public interface IExternalAiClient
{
    Task<string> GenerateAsync(string prompt, CancellationToken ct = default);
    Task<string> ChatAsync(IReadOnlyList<ChatMessage> history, string userMessage, CancellationToken ct = default);
}
