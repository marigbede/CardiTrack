using CardiTrack.Application.DTOs.Requests;
using CardiTrack.Application.DTOs.Responses;

namespace CardiTrack.Application.Interfaces.Services;

public interface IReportGenerationService
{
    /// <summary>Enqueues async report generation. Returns immediately with a report ID to poll.</summary>
    Task<ReportQueuedResponse> GenerateAsync(Guid requestingUserId, GenerateReportRequest request);

    /// <summary>Returns current status (pending / ready / failed / expired).</summary>
    Task<ReportStatusResponse?> GetStatusAsync(Guid requestingUserId, string reportId);

    /// <summary>Returns the raw file bytes and content-type for a ready report.</summary>
    Task<(byte[] Content, string ContentType, string FileName)> DownloadAsync(Guid requestingUserId, string reportId);
}
