using CardiTrack.Application.Interfaces.Repositories;
using CardiTrack.Infrastructure.Extensions;
using CardiTrack.Infrastructure.ExternalClients;
using CardiTrack.Infrastructure.Persistence;
using CardiTrack.Infrastructure.Repositories;
using CardiTrack.Infrastructure.Security;
using CardiTrack.Infrastructure.Settings;
using CardiTrack.Worker;
using Microsoft.EntityFrameworkCore;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;

        // Device provider config array
        services.Configure<List<DeviceProviderSettings>>(
            configuration.GetSection(DeviceProviderSettings.SectionName));

        // Database
        services.AddDbContext<CardiTrackDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // Encryption — key must be a base64-encoded 256-bit value in config/Key Vault
        services.AddScoped<IEncryptionService>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var key = config["Encryption:Key"]
                ?? throw new InvalidOperationException(
                    "Encryption:Key is not configured. Provide a base64-encoded 256-bit key.");
            return new AesEncryptionService(key);
        });

        // Repositories
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICardiMemberRepository, CardiMemberRepository>();
        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        services.AddScoped<IUserCardiMemberRepository, UserCardiMemberRepository>();
        services.AddScoped<IDeviceConnectionRepository, DeviceConnectionRepository>();
        services.AddScoped<IActivityLogRepository, ActivityLogRepository>();
        services.AddScoped<IDeviceRepository, DeviceRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // External clients
        services.AddHttpClient();
        services.AddScoped<IOAuthTokenRefreshService, OAuthTokenRefreshService>();

        // Fitbit provider (keyed IDeviceApiClient + keyed IDeviceSyncService)
        services.AddFitbitProvider();

        // Background workers
        services.AddWorker<WearableSyncWorker>(configuration, nameof(WearableSyncWorker));
    })
    .Build();

await host.RunAsync();
