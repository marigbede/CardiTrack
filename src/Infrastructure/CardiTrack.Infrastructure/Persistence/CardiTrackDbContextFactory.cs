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
        // Build configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/CardiTrack.API"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        // Create DbContext options
        var optionsBuilder = new DbContextOptionsBuilder<CardiTrackDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        optionsBuilder.UseNpgsql(connectionString, b => b.MigrationsAssembly("CardiTrack.Infrastructure"));

        return new CardiTrackDbContext(optionsBuilder.Options);
    }
}
