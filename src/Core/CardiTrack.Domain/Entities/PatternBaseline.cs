using CardiTrack.Domain.Common;

namespace CardiTrack.Domain.Entities;

public class PatternBaseline : BaseEntity
{
    public Guid CardiMemberId { get; set; }
    public DateTime CalculatedDate { get; set; }
    public int PeriodDays { get; set; } // 30, 60, or 90 days

    // Activity Baseline Metrics
    public int? AvgSteps { get; set; }
    public decimal? StdDevSteps { get; set; }
    public int? AvgActiveMinutes { get; set; }

    // Heart Rate Baseline Metrics
    public int? AvgRestingHeartRate { get; set; }
    public decimal? StdDevHeartRate { get; set; }
    public int? MaxHeartRateObserved { get; set; }

    // Sleep Baseline Metrics
    public int? AvgSleepMinutes { get; set; }
    public TimeOnly? TypicalBedtime { get; set; }
    public TimeOnly? TypicalWakeTime { get; set; }
    public int? AvgSleepEfficiency { get; set; }

    // JSON: [5200, 4800, 5100, 5300, 5400, 4900, 3200] (Mon-Sun)
    public string? StepsByDayOfWeek { get; set; }

    public PatternBaseline()
    {
        CalculatedDate = DateTime.UtcNow;
    }
}
