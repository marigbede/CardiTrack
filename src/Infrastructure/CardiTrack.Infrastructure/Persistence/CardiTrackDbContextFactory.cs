using CardiTrack.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CardiTrack.Infrastructure.Persistence;

/// <summary>
/// Design-time factory for creating DbContext during migrations
/// </summary>
public class CardiTrackDbContextFactory : IDesignTimeDbContextFactory<CardiTrackDbContext>
{
    public CardiTrackDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();
        var loader = new ConfigurationLoader(configuration);
        var connectionString = loader.Get(ConfigurationKeys.ConnectionStrings.DefaultConnection)
            ?? throw new ArgumentNullException(ConfigurationKeys.ConnectionStrings.DefaultConnection);

        var optionsBuilder = new DbContextOptionsBuilder<CardiTrackDbContext>();
        optionsBuilder.UseNpgsql(connectionString, b => b.MigrationsAssembly("CardiTrack.Infrastructure"));

        return new CardiTrackDbContext(optionsBuilder.Options);
    }
}
