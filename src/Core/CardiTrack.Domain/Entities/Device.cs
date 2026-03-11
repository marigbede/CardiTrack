using CardiTrack.Domain.Common;
using CardiTrack.Domain.Enums;
using CardiTrack.Domain.Interfaces;

namespace CardiTrack.Domain.Entities;

/// <summary>
/// Device Catalog - Reference data for supported wearable devices
/// </summary>
public class Device : BaseEntity, ISoftDeletable
{
    public DeviceType DeviceType { get; set; }
    public string Manufacturer { get; set; } = string.Empty;
    public string ModelName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;

    // JSON: { "hasHeartRate": true, "hasSpO2": true, "hasECG": false, "hasGPS": true, ... }
    public string Capabilities { get; set; } = "{}";

    public string? ApiEndpoint { get; set; }

    // JSON: { "authUrl": "...", "tokenUrl": "...", "scopes": [...], "clientIdRequired": true }
    public string? OAuthConfig { get; set; }

    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }
    public string? IconUrl { get; set; }
}
