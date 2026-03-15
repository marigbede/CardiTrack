namespace CardiTrack.Shared;

public static class ConfigurationKeys
{
    public static class ConnectionStrings
    {
        public const string DefaultConnection = "ConnectionStrings:DefaultConnection";
        public const string Redis = "ConnectionStrings:Redis";
    }

    public static class Auth0
    {
        public const string Domain = "Auth0:Domain";
        public const string Audience = "Auth0:Audience";
        public const string ClientId = "Auth0:ClientId";
        public const string ClientSecret = "Auth0:ClientSecret";
        public const string CallbackUrl = "Auth0:CallbackUrl";
        public const string LogoutUrl = "Auth0:LogoutUrl";
    }

    public static class Cors
    {
        public const string AllowedOrigins = "Cors:AllowedOrigins";
    }

    public static class Serilog
    {
        public const string SeqUrl = "Serilog:SeqUrl";
    }

    public static class Encryption
    {
        public const string Key = "Encryption:Key";
        public const string IV = "Encryption:IV";
    }

    public static class Workers
    {
        public static class WearableSyncWorker
        {
            public const string CronExpression = "Workers:WearableSyncWorker:CronExpression";
        }
    }

    /// <summary>Array section — use with IConfiguration.GetSection(), not ConfigurationLoader.Get().</summary>
    public static class DeviceProviders
    {
        public const string SectionName = "DeviceProviders";
    }
}
