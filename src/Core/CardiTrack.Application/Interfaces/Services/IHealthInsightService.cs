using CardiTrack.Application.DTOs.Responses;

namespace CardiTrack.Application.Interfaces.Services;

public interface IHealthInsightService
{
    Task<AlertInsightResponse> AnalyzeAlertAsync(Guid alertId, CancellationToken ct = default);
    Task<BaselineInsightResponse> AnalyzeBaselineAsync(Guid cardiMemberId, CancellationToken ct = default);
}
