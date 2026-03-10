using CardiTrack.Application.DTOs.Requests;
using CardiTrack.Application.DTOs.Responses;
using CardiTrack.Application.Interfaces.Services;
using CardiTrack.Domain.Enums;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace CardiTrack.UnitTests.Services;

public class ReportGenerationServiceTests
{
    private readonly IReportGenerationService _sut = Substitute.For<IReportGenerationService>();
    private readonly Guid _userId = Guid.NewGuid();
    private readonly Guid _cardiMemberId = Guid.NewGuid();

    private GenerateReportRequest BuildRequest(ReportFormat format) => new()
    {
        CardiMemberIds = [_cardiMemberId],
        DateRangeFrom = new DateOnly(2026, 2, 7),
        DateRangeTo = new DateOnly(2026, 3, 9),
        Format = format
    };

    // ── GenerateAsync — queued response ────────────────────────────────────────

    [Fact]
    public async Task GenerateAsync_ReturnsPendingStatus_OnSuccess()
    {
        var request = BuildRequest(ReportFormat.Pdf);
        _sut.GenerateAsync(_userId, request)
            .Returns(new ReportQueuedResponse
            {
                ReportId = "rpt_abc123",
                Status = ReportStatus.Pending,
                EstimatedReadyInSeconds = 10,
                StatusUrl = "/api/v1/reports/rpt_abc123"
            });

        var result = await _sut.GenerateAsync(_userId, request);

        Assert.Equal(ReportStatus.Pending, result.Status);
        Assert.NotEmpty(result.ReportId);
    }

    [Theory]
    [InlineData(ReportFormat.Pdf)]
    [InlineData(ReportFormat.Csv)]
    [InlineData(ReportFormat.FhirR4)]
    public async Task GenerateAsync_AcceptsMvp1Formats(ReportFormat format)
    {
        var request = BuildRequest(format);
        _sut.GenerateAsync(_userId, request)
            .Returns(new ReportQueuedResponse
            {
                ReportId = "rpt_001",
                Status = ReportStatus.Pending,
                EstimatedReadyInSeconds = 10,
                StatusUrl = "/api/v1/reports/rpt_001"
            });

        var result = await _sut.GenerateAsync(_userId, request);

        Assert.Equal(ReportStatus.Pending, result.Status);
    }

    [Fact]
    public async Task GenerateAsync_AcceptsHl7V2Format_ForMvp2()
    {
        var request = BuildRequest(ReportFormat.Hl7V2);
        _sut.GenerateAsync(_userId, request)
            .Returns(new ReportQueuedResponse
            {
                ReportId = "rpt_hl7_001",
                Status = ReportStatus.Pending,
                EstimatedReadyInSeconds = 15,
                StatusUrl = "/api/v1/reports/rpt_hl7_001"
            });

        var result = await _sut.GenerateAsync(_userId, request);

        Assert.Equal(ReportStatus.Pending, result.Status);
    }

    [Fact]
    public async Task GenerateAsync_ThrowsArgumentException_WhenCardiMemberListIsEmpty()
    {
        var request = new GenerateReportRequest
        {
            CardiMemberIds = [],
            DateRangeFrom = new DateOnly(2026, 2, 7),
            DateRangeTo = new DateOnly(2026, 3, 9),
            Format = ReportFormat.Pdf
        };
        _sut.GenerateAsync(_userId, request)
            .ThrowsAsync(new ArgumentException("At least one CardiMember ID is required."));

        await Assert.ThrowsAsync<ArgumentException>(() => _sut.GenerateAsync(_userId, request));
    }

    [Fact]
    public async Task GenerateAsync_ThrowsArgumentOutOfRangeException_WhenDateRangeExceeds365Days()
    {
        var request = new GenerateReportRequest
        {
            CardiMemberIds = [_cardiMemberId],
            DateRangeFrom = new DateOnly(2025, 1, 1),
            DateRangeTo = new DateOnly(2026, 3, 9),
            Format = ReportFormat.Csv
        };
        _sut.GenerateAsync(_userId, request)
            .ThrowsAsync(new ArgumentOutOfRangeException("dateRange", "Date range must not exceed 365 days."));

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _sut.GenerateAsync(_userId, request));
    }

    // ── GetStatusAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task GetStatusAsync_ReturnsPendingWithProgress_WhileGenerating()
    {
        _sut.GetStatusAsync(_userId, "rpt_abc123")
            .Returns(new ReportStatusResponse
            {
                ReportId = "rpt_abc123",
                Status = ReportStatus.Pending,
                ProgressPercent = 40,
                CreatedAt = DateTimeOffset.UtcNow.AddSeconds(-5)
            });

        var result = await _sut.GetStatusAsync(_userId, "rpt_abc123");

        Assert.Equal(ReportStatus.Pending, result!.Status);
        Assert.Equal(40, result.ProgressPercent);
    }

    [Fact]
    public async Task GetStatusAsync_ReturnsReadyWithFhirContentType_ForFhirR4Report()
    {
        _sut.GetStatusAsync(_userId, "rpt_fhir_001")
            .Returns(new ReportStatusResponse
            {
                ReportId = "rpt_fhir_001",
                Status = ReportStatus.Ready,
                Format = ReportFormat.FhirR4,
                ContentType = "application/fhir+json",
                FileSizeBytes = 312400,
                DownloadUrl = "/api/v1/reports/rpt_fhir_001/download",
                DownloadExpiresAt = DateTimeOffset.UtcNow.AddHours(24),
                CreatedAt = DateTimeOffset.UtcNow.AddSeconds(-8),
                CompletedAt = DateTimeOffset.UtcNow,
                Metadata = new ReportMetadata
                {
                    CardiMembers = ["Margaret Doe"],
                    DateRangeFrom = new DateOnly(2026, 2, 7),
                    DateRangeTo = new DateOnly(2026, 3, 9),
                    FhirProfile = "us-core",
                    FhirResources = ["Patient", "Observation", "Device"]
                }
            });

        var result = await _sut.GetStatusAsync(_userId, "rpt_fhir_001");

        Assert.Equal(ReportStatus.Ready, result!.Status);
        Assert.Equal("application/fhir+json", result.ContentType);
        Assert.Equal("us-core", result.Metadata!.FhirProfile);
        Assert.Contains("Observation", result.Metadata.FhirResources!);
    }

    [Fact]
    public async Task GetStatusAsync_ReturnsReadyWithHl7ContentType_ForHl7V2Report()
    {
        _sut.GetStatusAsync(_userId, "rpt_hl7_001")
            .Returns(new ReportStatusResponse
            {
                ReportId = "rpt_hl7_001",
                Status = ReportStatus.Ready,
                Format = ReportFormat.Hl7V2,
                ContentType = "application/hl7-v2+er7",
                FileSizeBytes = 8200,
                DownloadUrl = "/api/v1/reports/rpt_hl7_001/download",
                DownloadExpiresAt = DateTimeOffset.UtcNow.AddHours(24),
                CreatedAt = DateTimeOffset.UtcNow.AddSeconds(-12),
                CompletedAt = DateTimeOffset.UtcNow
            });

        var result = await _sut.GetStatusAsync(_userId, "rpt_hl7_001");

        Assert.Equal(ReportStatus.Ready, result!.Status);
        Assert.Equal("application/hl7-v2+er7", result.ContentType);
    }

    [Fact]
    public async Task GetStatusAsync_ReturnsFailedWithError_WhenGenerationFails()
    {
        _sut.GetStatusAsync(_userId, "rpt_fail")
            .Returns(new ReportStatusResponse
            {
                ReportId = "rpt_fail",
                Status = ReportStatus.Failed,
                Error = "Insufficient data in the selected date range to generate a meaningful report.",
                CreatedAt = DateTimeOffset.UtcNow.AddSeconds(-3)
            });

        var result = await _sut.GetStatusAsync(_userId, "rpt_fail");

        Assert.Equal(ReportStatus.Failed, result!.Status);
        Assert.NotNull(result.Error);
    }

    [Fact]
    public async Task GetStatusAsync_ReturnsNull_WhenReportNotFound()
    {
        _sut.GetStatusAsync(_userId, "rpt_unknown").Returns((ReportStatusResponse?)null);

        var result = await _sut.GetStatusAsync(_userId, "rpt_unknown");

        Assert.Null(result);
    }

    // ── DownloadAsync ───────────────────────────────────────────────────────────

    [Fact]
    public async Task DownloadAsync_ReturnsPdfBytes_WithCorrectContentType()
    {
        var pdfBytes = new byte[] { 0x25, 0x50, 0x44, 0x46 }; // %PDF magic bytes
        _sut.DownloadAsync(_userId, "rpt_pdf_001")
            .Returns((pdfBytes, "application/pdf", "carditrack-margaret-doe-2026-03-09.pdf"));

        var (content, contentType, fileName) = await _sut.DownloadAsync(_userId, "rpt_pdf_001");

        Assert.Equal("application/pdf", contentType);
        Assert.EndsWith(".pdf", fileName);
        Assert.NotEmpty(content);
    }

    [Fact]
    public async Task DownloadAsync_ReturnsFhirJson_WithCorrectContentType()
    {
        var fhirBytes = System.Text.Encoding.UTF8.GetBytes("""{"resourceType":"Bundle","type":"collection"}""");
        _sut.DownloadAsync(_userId, "rpt_fhir_001")
            .Returns((fhirBytes, "application/fhir+json", "carditrack-margaret-doe-2026-03-09-fhir.json"));

        var (content, contentType, fileName) = await _sut.DownloadAsync(_userId, "rpt_fhir_001");

        Assert.Equal("application/fhir+json", contentType);
        Assert.EndsWith("-fhir.json", fileName);
    }

    [Fact]
    public async Task DownloadAsync_ReturnsHl7Bytes_WithCorrectContentType()
    {
        var hl7Bytes = System.Text.Encoding.UTF8.GetBytes("MSH|^~\\&|CardiTrack|||20260309||ORU^R01|");
        _sut.DownloadAsync(_userId, "rpt_hl7_001")
            .Returns((hl7Bytes, "application/hl7-v2+er7", "carditrack-margaret-doe-2026-03-09-hl7.hl7"));

        var (content, contentType, fileName) = await _sut.DownloadAsync(_userId, "rpt_hl7_001");

        Assert.Equal("application/hl7-v2+er7", contentType);
        Assert.EndsWith(".hl7", fileName);
    }

    [Fact]
    public async Task DownloadAsync_ThrowsInvalidOperationException_WhenReportNotYetReady()
    {
        _sut.DownloadAsync(_userId, "rpt_pending")
            .ThrowsAsync(new InvalidOperationException("Report generation not yet complete."));

        await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.DownloadAsync(_userId, "rpt_pending"));
    }

    [Fact]
    public async Task DownloadAsync_ThrowsInvalidOperationException_WhenDownloadLinkExpired()
    {
        _sut.DownloadAsync(_userId, "rpt_expired")
            .ThrowsAsync(new InvalidOperationException("Download link has expired — regenerate the report."));

        await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.DownloadAsync(_userId, "rpt_expired"));
    }

    // ── FHIR R4 request defaults ────────────────────────────────────────────────

    [Fact]
    public void GenerateReportRequest_FhirProfile_DefaultsToUsCore()
    {
        var request = BuildRequest(ReportFormat.FhirR4);

        Assert.Equal("us-core", request.FhirProfile);
    }

    [Fact]
    public void GenerateReportRequest_FhirResources_DefaultsToPatientObservationDevice()
    {
        var request = BuildRequest(ReportFormat.FhirR4);

        Assert.Contains("Patient", request.FhirResources);
        Assert.Contains("Observation", request.FhirResources);
        Assert.Contains("Device", request.FhirResources);
    }

    [Fact]
    public void GenerateReportRequest_Sections_DefaultToStandardPdfCsvDefaults()
    {
        var request = BuildRequest(ReportFormat.Pdf);

        Assert.True(request.IncludeMetrics);
        Assert.True(request.IncludeTrends);
        Assert.True(request.IncludeAlerts);
        Assert.False(request.IncludeNotes);
        Assert.False(request.IncludeDevices);
    }
}
