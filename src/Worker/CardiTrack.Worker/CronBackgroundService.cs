using Cronos;

namespace CardiTrack.Worker;

public abstract class CronBackgroundService : BackgroundService
{
    private readonly CronExpression _cron;
    private readonly TimeZoneInfo _timeZone;

    protected CronBackgroundService(string cronExpression, TimeZoneInfo? timeZone = null)
    {
        _cron = CronExpression.Parse(cronExpression, CronFormat.IncludeSeconds);
        _timeZone = timeZone ?? TimeZoneInfo.Utc;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTimeOffset.UtcNow;
            var next = _cron.GetNextOccurrence(now, _timeZone);

            if (next is null)
                break;

            var delay = next.Value - now;
            if (delay > TimeSpan.Zero)
                await Task.Delay(delay, stoppingToken);

            if (!stoppingToken.IsCancellationRequested)
                await ExecuteJobAsync(stoppingToken);
        }
    }

    protected abstract Task ExecuteJobAsync(CancellationToken stoppingToken);
}
