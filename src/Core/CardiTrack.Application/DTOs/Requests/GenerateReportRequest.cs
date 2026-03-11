using CardiTrack.Domain.Enums;

namespace CardiTrack.Application.DTOs.Requests;

public class GenerateReportRequest
{
    public required IReadOnlyList<Guid> CardiMemberIds { get; init; }
    public required DateOnly DateRangeFrom { get; init; }
    public required DateOnly DateRangeTo { get; init; }
    public required ReportFormat Format { get; init; }

    // FHIR-specific options (used when Format == FhirR4)
    public string FhirProfile { get; init; } = "us-core";
    public IReadOnlyList<string> FhirResources { get; init; } = ["Patient", "Observation", "Device"];

    // PDF/CSV section toggles
    public bool IncludeMetrics { get; init; } = true;
    public bool IncludeTrends { get; init; } = true;
    public bool IncludeAlerts { get; init; } = true;
    public bool IncludeNotes { get; init; } = false;
    public bool IncludeDevices { get; init; } = false;

    public string? Title { get; init; }
}
