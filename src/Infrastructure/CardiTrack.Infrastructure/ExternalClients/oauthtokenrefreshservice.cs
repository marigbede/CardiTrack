using System.Net.Http.Headers;
using System.Text.Json;
using CardiTrack.Application.Interfaces.Repositories;
using CardiTrack.Domain.Entities;
using CardiTrack.Domain.Enums;
using CardiTrack.Infrastructure.Security;
using CardiTrack.Infrastructure.Settings;

namespace CardiTrack.Infrastructure.ExternalClients;

public class OAuthTokenRefreshService : IOAuthTokenRefreshService
{
    private readonly IDeviceConnectionRepository _deviceConnections;
    private readonly IEncryptionService _encryption;
    private readonly IHttpClientFactory _httpClientFactory;

    private static readonly TimeSpan ExpiryBuffer = TimeSpan.FromMinutes(5);

    public OAuthTokenRefreshService(
        IDeviceConnectionRepository deviceConnections,
        IEncryptionService encryption,
        IHttpClientFactory httpClientFactory)
    {
        _deviceConnections = deviceConnections;
        _encryption = encryption;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> RefreshIfExpiredAsync(DeviceConnection connection, DeviceProviderSettings providerConfig)
    {
        // Token is still valid — return decrypted access token without refreshing
        if (connection.TokenExpiry.HasValue
            && connection.TokenExpiry.Value > DateTime.UtcNow.Add(ExpiryBuffer)
            && connection.AccessToken is not null)
        {
            return _encryption.Decrypt(connection.AccessToken);
        }

        if (connection.RefreshToken is null)
            throw new InvalidOperationException(
                $"DeviceConnection {connection.Id} has no refresh token stored.");

        var plainRefreshToken = _encryption.Decrypt(connection.RefreshToken);

        using var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Basic",
            Convert.ToBase64String(
                System.Text.Encoding.UTF8.GetBytes($"{providerConfig.ClientId}:{providerConfig.ClientSecret}")));

        var body = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "refresh_token",
            ["refresh_token"] = plainRefreshToken
        });

        HttpResponseMessage response;
        try
        {
            response = await client.PostAsync(providerConfig.TokenUrl, body);
        }
        catch (Exception ex)
        {
            await _deviceConnections.UpdateStatusAsync(connection.Id, ConnectionStatus.TokenExpired);
            throw new InvalidOperationException(
                $"Token refresh HTTP call failed for DeviceConnection {connection.Id}.", ex);
        }

        if (!response.IsSuccessStatusCode)
        {
            await _deviceConnections.UpdateStatusAsync(connection.Id, ConnectionStatus.TokenExpired);
            var errorBody = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException(
                $"Token refresh returned {(int)response.StatusCode} for DeviceConnection {connection.Id}: {errorBody}");
        }

        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var root = doc.RootElement;

        var newAccessToken = root.GetProperty("access_token").GetString()
            ?? throw new InvalidOperationException("Token response missing access_token.");
        var newRefreshToken = root.TryGetProperty("refresh_token", out var rt)
            ? rt.GetString() ?? plainRefreshToken
            : plainRefreshToken;

        var expiresIn = root.TryGetProperty("expires_in", out var exp)
            ? exp.GetInt32()
            : providerConfig.TokenLifetimeHours * 3600;
        var newExpiry = DateTime.UtcNow.AddSeconds(expiresIn);

        await _deviceConnections.UpdateTokenAsync(
            connection.Id,
            _encryption.Encrypt(newAccessToken),
            _encryption.Encrypt(newRefreshToken),
            newExpiry);

        return newAccessToken;
    }
}
