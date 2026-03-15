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
        var loader = new ConfigurationLoader(new ConfigurationBuilder().Build());
        var connectionString = loader.GetRequired(ConfigurationKeys.ConnectionStrings.DefaultConnection);

        var optionsBuilder = new DbContextOptionsBuilder<CardiTrackDbContext>();
        optionsBuilder.UseNpgsql(connectionString, b => b.MigrationsAssembly("CardiTrack.Infrastructure"));

        return new CardiTrackDbContext(optionsBuilder.Options);
    }
}
