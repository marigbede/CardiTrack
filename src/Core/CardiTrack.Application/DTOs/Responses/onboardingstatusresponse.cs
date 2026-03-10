namespace CardiTrack.Application.DTOs.Responses;

public class OnboardingStatusResponse
{
    public bool HasOrganization { get; set; }
    public bool HasUserAccount { get; set; }
    public bool HasCardiMember { get; set; }
    public bool HasDeviceConnected { get; set; }
    public bool HasNotificationPreferences { get; set; }
    public bool IsOnboardingComplete { get; set; }
    public int CurrentStep { get; set; }
    public int TotalSteps { get; set; } = 7;
    public string? NextStepMessage { get; set; }
}
