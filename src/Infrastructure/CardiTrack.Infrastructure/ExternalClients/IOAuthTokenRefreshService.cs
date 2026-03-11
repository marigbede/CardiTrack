using CardiTrack.Domain.Entities;
using CardiTrack.Infrastructure.Settings;

namespace CardiTrack.Infrastructure.ExternalClients;

public interface IOAuthTokenRefreshService
{
    /// <summary>
    /// Checks whether the access token on the given connection is expired (or expiring within 5 minutes)
    /// and refreshes it if needed. Returns the current (possibly refreshed) plain-text access token.
    /// Throws if the refresh fails, and marks the connection as TokenExpired.
    /// </summary>
    Task<string> RefreshIfExpiredAsync(DeviceConnection connection, DeviceProviderSettings providerConfig);
}
