namespace CardiTrack.Infrastructure.ExternalClients;

/// <summary>
/// Generic wearable API client. Each provider implements this interface.
/// </summary>
public interface IDeviceApiClient
{
    Task<DeviceHealthSnapshot> GetHealthSnapshotAsync(string accessToken, DateOnly date);
}
