namespace CardiTrack.Infrastructure.Settings;

public class AiProviderSettings
{
    /// <summary>Matches the value of AI:GeneralProvider or AI:MedicalProvider (e.g. "Gemini", "MedGemma").</summary>
    public required string Name { get; init; }

    public required string BaseUrl { get; init; }

    public required string Model { get; init; }

    /// <summary>Null for providers that do not require an API key (e.g. self-hosted Ollama).</summary>
    public string? ApiKey { get; init; }

    public int TimeoutSeconds { get; init; } = 60;
}
