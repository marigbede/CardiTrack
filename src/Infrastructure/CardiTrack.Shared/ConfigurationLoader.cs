using Microsoft.Extensions.Configuration;

namespace CardiTrack.Shared;

/// <summary>
/// Loads configuration values with explicit environment-variable-first precedence.
///
/// Lookup order for a given key (e.g. "Auth0:Domain"):
///   1. Environment variable using '__' separator (e.g. "Auth0__Domain")
///   2. IConfiguration fallback (covers appsettings.json and appsettings.{env}.json)
/// </summary>
public sealed class ConfigurationLoader(IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration
        ?? throw new ArgumentNullException(nameof(configuration));

    /// <summary>Returns the value for <paramref name="key"/>, or <c>null</c> if not found.</summary>
    public string? Get(string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        return Environment.GetEnvironmentVariable(ToEnvVarKey(key))
            ?? _configuration[key];
    }

    /// <summary>
    /// Returns the value for <paramref name="key"/>.
    /// Throws <see cref="InvalidOperationException"/> if the value is absent or whitespace.
    /// </summary>
    public string GetRequired(string key)
    {
        var value = Get(key);
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException(
                $"Required configuration key '{key}' is not set. " +
                $"Set it in appsettings.json or as environment variable '{ToEnvVarKey(key)}'.");
        return value;
    }

    /// <summary>Converts "Auth0:Domain" → "Auth0__Domain" for environment variable lookup.</summary>
    public static string ToEnvVarKey(string key) => key.Replace(":", "__");
}
