namespace CardiTrack.Infrastructure.Settings;

public class DeviceProviderSettings
{
    public const string SectionName = "DeviceProviders";

    /// <summary>Matches DeviceType enum name (e.g. "Fitbit", "Garmin").</summary>
    public string Provider { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string AuthorizationUrl { get; set; } = string.Empty;
    public string TokenUrl { get; set; } = string.Empty;
    public string ApiBaseUrl { get; set; } = string.Empty;
    public List<string> Scopes { get; set; } = [];
    public string RedirectUri { get; set; } = string.Empty;

    /// <summary>Access token lifetime in hours. Used to compute TokenExpiry on storage.</summary>
    public int TokenLifetimeHours { get; set; } = 8;
}
