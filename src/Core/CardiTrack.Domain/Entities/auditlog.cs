using CardiTrack.Domain.Common;

namespace CardiTrack.Domain.Entities;

/// <summary>
/// HIPAA Compliance - Audit trail for all PHI access
/// Retention: 90 days minimum
/// </summary>
public class AuditLog : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid? CardiMemberId { get; set; } // Nullable for system actions
    public string Action { get; set; } = string.Empty; // ViewDashboard, ViewAlert, ExportData, etc.
    public string EntityType { get; set; } = string.Empty; // CardiMember, Alert, ActivityLog, etc.
    public Guid? EntityId { get; set; }
    public DateTime Timestamp { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public string RequestPath { get; set; } = string.Empty;
    public string HttpMethod { get; set; } = string.Empty;
    public int ResponseStatus { get; set; }

    // JSON: Summary of PHI viewed (not the actual data)
    public string? DataAccessed { get; set; }

    // JSON: For update/delete operations
    public string? ChangedFields { get; set; }

    public AuditLog()
    {
        Timestamp = DateTime.UtcNow;
    }
}
