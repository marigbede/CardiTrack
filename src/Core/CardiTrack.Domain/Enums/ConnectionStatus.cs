using System.ComponentModel.DataAnnotations;

namespace CardiTrack.Domain.Enums;

public enum ConnectionStatus
{
    [Display(Name = "Connected")]
    Connected = 1,

    [Display(Name = "Disconnected")]
    Disconnected = 2,

    [Display(Name = "Token Expired")]
    TokenExpired = 3,

    [Display(Name = "Authentication Error")]
    AuthError = 4,

    [Display(Name = "Sync Error")]
    SyncError = 5
}
