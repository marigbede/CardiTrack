using CardiTrack.Application.DTOs.Common;
using CardiTrack.Application.Interfaces.Clients;
using CardiTrack.Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CardiTrack.Infrastructure.Services;

public class GenerativeAiService : IGenerativeAiService
{
    private readonly IExternalAiClient _client;

    public GenerativeAiService([FromKeyedServices("GeneralProvider")] IExternalAiClient client)
    {
        _client = client;
    }

    public Task<string> GenerateAsync(string prompt, CancellationToken ct = default)
        => _client.GenerateAsync(prompt, ct);

    public Task<string> ChatAsync(IReadOnlyList<ChatMessage> history, string userMessage, CancellationToken ct = default)
        => _client.ChatAsync(history, userMessage, ct);
}
