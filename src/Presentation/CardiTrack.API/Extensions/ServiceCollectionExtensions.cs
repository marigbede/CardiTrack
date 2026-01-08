using CardiTrack.API.Infrastructure.UserContext;
using AspNetCoreRateLimit;

namespace CardiTrack.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // TODO: Register application services when they are created
        // services.AddScoped<IOrganizationService, OrganizationService>();
        // services.AddScoped<IUserService, UserService>();
        // services.AddScoped<ICardiMemberService, CardiMemberService>();
        // services.AddScoped<IDeviceService, DeviceService>();
        // services.AddScoped<INotificationService, NotificationService>();
        // services.AddScoped<ISubscriptionService, SubscriptionService>();
        // services.AddScoped<IAuth0Service, Auth0Service>();

        return services;
    }

    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // TODO: Register infrastructure services when they are created
        // services.AddScoped<IUnitOfWork, UnitOfWork>();
        // services.AddScoped<IEncryptionService, AesEncryptionService>();
        // services.AddScoped<IAuditService, AuditService>();

        // HTTP Client for Auth0 service
        services.AddHttpClient("Auth0Client", client =>
        {
            var auth0Domain = configuration["Auth0:Domain"];
            client.BaseAddress = new Uri($"https://{auth0Domain}/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        return services;
    }

    public static IServiceCollection AddCachingServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Redis distributed cache
        var redisConnection = configuration.GetConnectionString("Redis");
        if (!string.IsNullOrEmpty(redisConnection))
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnection;
                options.InstanceName = "CardiTrack_";
            });
        }
        else
        {
            // Fallback to in-memory cache if Redis not configured
            services.AddDistributedMemoryCache();
        }

        services.AddMemoryCache();

        return services;
    }

    public static IServiceCollection AddRateLimiting(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Rate limiting configuration
        services.AddMemoryCache();
        services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
        services.AddInMemoryRateLimiting();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

        return services;
    }

    public static IServiceCollection AddUserContextServices(
        this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();

        return services;
    }
}
