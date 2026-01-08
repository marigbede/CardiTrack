using CardiTrack.API.Infrastructure.UserContext;
using AspNetCoreRateLimit;

namespace CardiTrack.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register application services
        services.AddScoped<CardiTrack.Application.Interfaces.Services.IOrganizationService, CardiTrack.Application.Services.OrganizationService>();
        services.AddScoped<CardiTrack.Application.Interfaces.Services.IUserService, CardiTrack.Application.Services.UserService>();
        services.AddScoped<CardiTrack.Application.Interfaces.Services.ICardiMemberService, CardiTrack.Application.Services.CardiMemberService>();
        services.AddScoped<CardiTrack.Application.Interfaces.Services.ISubscriptionService, CardiTrack.Application.Services.SubscriptionService>();

        return services;
    }

    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register repositories
        services.AddScoped<CardiTrack.Application.Interfaces.Repositories.IOrganizationRepository, CardiTrack.Infrastructure.Repositories.OrganizationRepository>();
        services.AddScoped<CardiTrack.Application.Interfaces.Repositories.IUserRepository, CardiTrack.Infrastructure.Repositories.UserRepository>();
        services.AddScoped<CardiTrack.Application.Interfaces.Repositories.ICardiMemberRepository, CardiTrack.Infrastructure.Repositories.CardiMemberRepository>();
        services.AddScoped<CardiTrack.Application.Interfaces.Repositories.ISubscriptionRepository, CardiTrack.Infrastructure.Repositories.SubscriptionRepository>();
        services.AddScoped<CardiTrack.Application.Interfaces.Repositories.IUserCardiMemberRepository, CardiTrack.Infrastructure.Repositories.UserCardiMemberRepository>();

        // Register Unit of Work
        services.AddScoped<CardiTrack.Application.Interfaces.Repositories.IUnitOfWork, CardiTrack.Infrastructure.Repositories.UnitOfWork>();

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
