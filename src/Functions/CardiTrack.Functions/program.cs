using CardiTrack.Application.Interfaces.Repositories;
using CardiTrack.Application.Interfaces.Services;
using CardiTrack.Infrastructure.Services;
using CardiTrack.Infrastructure.ExternalClients;
using CardiTrack.Infrastructure.Persistence;
using CardiTrack.Infrastructure.Repositories;
using CardiTrack.Infrastructure.Security;
using CardiTrack.Infrastructure.Settings;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;

        // Device provider config array
        services.Configure<List<DeviceProviderSettings>>(
            configuration.GetSection(DeviceProviderSettings.SectionName));

        // Database
        services.AddDbContext<CardiTrackDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Encryption
        services.AddScoped<IEncryptionService, AesEncryptionService>();

        // Repositories
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICardiMemberRepository, CardiMemberRepository>();
        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        services.AddScoped<IUserCardiMemberRepository, UserCardiMemberRepository>();
        services.AddScoped<IDeviceConnectionRepository, DeviceConnectionRepository>();
        services.AddScoped<IActivityLogRepository, ActivityLogRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // External clients
        services.AddHttpClient();
        services.AddHttpClient("FitbitClient", client =>
        {
            client.BaseAddress = new Uri("https://api.fitbit.com");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });
        services.AddScoped<IOAuthTokenRefreshService, OAuthTokenRefreshService>();
        services.AddScoped<IFitbitApiClient, FitbitApiClient>();

        // Application services
        services.AddScoped<IFitbitSyncService, FitbitSyncService>();
    })
    .Build();

await host.RunAsync();
