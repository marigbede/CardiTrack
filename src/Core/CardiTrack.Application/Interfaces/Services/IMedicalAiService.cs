namespace CardiTrack.Application.Interfaces.Services;

/// <summary>
/// High-level service for medical analysis tasks.
/// Active provider is config-driven via AI:MedicalProvider.
/// </summary>
public interface IMedicalAiService
{
    Task<string> GenerateAsync(string prompt, CancellationToken ct = default);
}
