using CardiTrack.Domain.Common;
using CardiTrack.Domain.Enums;
using CardiTrack.Domain.Interfaces;

namespace CardiTrack.Domain.Entities;

public class Alert : BaseEntity, ISoftDeletable
{
    public Guid CardiMemberId { get; set; }
    public AlertType AlertType { get; set; }
    public AlertSeverity Severity { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime TriggeredDate { get; set; }
    public DateTime? AcknowledgedDate { get; set; }
    public Guid? AcknowledgedByUserId { get; set; } // User who acknowledged the alert
    public bool IsResolved { get; set; }

    // JSON: { "steps": 450, "baseline": 5200, "deviation": -91.3 }
    public string? MetricValues { get; set; }

    public bool IsActive { get; set; } = true;

    public Alert()
    {
        TriggeredDate = DateTime.UtcNow;
    }
}
