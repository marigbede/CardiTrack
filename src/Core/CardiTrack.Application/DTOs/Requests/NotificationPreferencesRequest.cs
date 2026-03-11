using System.ComponentModel.DataAnnotations;
using CardiTrack.Domain.Enums;

namespace CardiTrack.Application.DTOs.Requests;

public class NotificationPreferencesRequest
{
    [Required(ErrorMessage = "CardiMember ID is required")]
    public Guid CardiMemberId { get; set; }

    public bool ReceiveSmsAlerts { get; set; }
    public bool ReceiveEmailAlerts { get; set; }
    public bool ReceivePushAlerts { get; set; }

    public List<AlertType> EnabledAlertTypes { get; set; } = new();

    public TimeOnly? QuietHoursStart { get; set; }
    public TimeOnly? QuietHoursEnd { get; set; }
}
