using CardiTrack.Application.Interfaces.Clients;
using CardiTrack.Application.Interfaces.Services;
using CardiTrack.Infrastructure.ExternalClients.General;
using CardiTrack.Infrastructure.ExternalClients.Medical;
using CardiTrack.Infrastructure.Services;
using CardiTrack.Infrastructure.Settings;
using CardiTrack.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CardiTrack.Infrastructure.Extensions;

public static class AiServiceExtensions
{
    public static IServiceCollection AddAiServices(
        this IServiceCollection services,
        IConfiguration configuration,
        ConfigurationLoader config)
    {
        var providers = configuration
            .GetSection(ConfigurationKeys.AI.ProvidersSectionName)
            .Get<List<AiProviderSettings>>() ?? [];

        var generalName = config.GetRequired(ConfigurationKeys.AI.GeneralProvider);
        var medicalName = config.GetRequired(ConfigurationKeys.AI.MedicalProvider);

        var generalSettings = providers.FirstOrDefault(p => p.Name == generalName)
            ?? throw new InvalidOperationException(
                $"AI provider '{generalName}' not found in {ConfigurationKeys.AI.ProvidersSectionName}.");

        var medicalSettings = providers.FirstOrDefault(p => p.Name == medicalName)
            ?? throw new InvalidOperationException(
                $"AI provider '{medicalName}' not found in {ConfigurationKeys.AI.ProvidersSectionName}.");

        services.AddHttpClient("GeminiClient", client =>
        {
            client.BaseAddress = new Uri(generalSettings.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(generalSettings.TimeoutSeconds);
        });

        if (!string.Equals(generalName, medicalName, StringComparison.OrdinalIgnoreCase))
        {
            services.AddHttpClient("MedGemmaClient", client =>
            {
                client.BaseAddress = new Uri(medicalSettings.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(medicalSettings.TimeoutSeconds);
            });
        }

        services.AddKeyedScoped<IExternalAiClient>("GeneralProvider", (sp, _) =>
            generalName switch
            {
                "Gemini" => new GeminiClient(sp.GetRequiredService<IHttpClientFactory>(), generalSettings),
                _ => throw new InvalidOperationException($"Unsupported general AI provider: '{generalName}'.")
            });

        services.AddKeyedScoped<IExternalAiClient>("MedicalProvider", (sp, _) =>
            medicalName switch
            {
                "MedGemma" => new MedGemmaClient(sp.GetRequiredService<IHttpClientFactory>(), medicalSettings),
                _ => throw new InvalidOperationException($"Unsupported medical AI provider: '{medicalName}'.")
            });

        services.AddScoped<IGenerativeAiService, GenerativeAiService>();
        services.AddScoped<IMedicalAiService, MedicalAiService>();
        services.AddScoped<IHealthInsightService, HealthInsightService>();
        services.AddScoped<IReportGenerationService, ReportGenerationService>();

        return services;
    }
}
