using Microsoft.Extensions.Options;

namespace CardiTrack.Worker;

public static class WorkerServiceExtensions
{
    public static IServiceCollection AddWorker<T>(
        this IServiceCollection services,
        IConfiguration configuration,
        string name)
        where T : BackgroundService
    {
        services.Configure<WorkerOptions>(name, configuration.GetSection($"Workers:{name}"));
        services.AddHostedService<T>();
        return services;
    }
}
