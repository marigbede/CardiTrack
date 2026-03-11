using CardiTrack.Application.Interfaces.Repositories;
using CardiTrack.Infrastructure.Persistence;
using CardiTrack.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace CardiTrack.UnitTests.Infrastructure;

public class TestDatabaseFixture : IAsyncLifetime
{
    private MsSqlContainer _container = null!;
    private ServiceProvider _serviceProvider = null!;

    public async Task InitializeAsync()
    {
        _container = SqlServerTestContainerFactory.CreateStandardContainer();
        await _container.StartAsync();

        var services = new ServiceCollection();

        services.AddDbContext<CardiTrackDbContext>(options =>
            options.UseSqlServer(_container.GetConnectionString())
                   .EnableDetailedErrors()
                   .EnableSensitiveDataLogging());

        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<ICardiMemberRepository, CardiMemberRepository>();
        services.AddScoped<IDeviceConnectionRepository, DeviceConnectionRepository>();
        services.AddScoped<IActivityLogRepository, ActivityLogRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        services.AddScoped<IUserCardiMemberRepository, UserCardiMemberRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        _serviceProvider = services.BuildServiceProvider();

        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CardiTrackDbContext>();
        await context.Database.MigrateAsync();
    }

    /// <summary>Creates a new DI scope. Each test should use its own scope.</summary>
    public IServiceScope CreateScope() => _serviceProvider.CreateScope();

    public async Task DisposeAsync()
    {
        await _serviceProvider.DisposeAsync();
        await _container.DisposeAsync();
    }
}
