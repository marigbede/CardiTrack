namespace CardiTrack.Worker;

public class WorkerOptions
{
    public string CronExpression { get; set; } = "0 * * * * *";
}
