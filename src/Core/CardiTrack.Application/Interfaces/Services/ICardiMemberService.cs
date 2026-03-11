using CardiTrack.Application.DTOs.Requests;
using CardiTrack.Application.DTOs.Responses;

namespace CardiTrack.Application.Interfaces.Services;

public interface ICardiMemberService
{
    Task<CardiMemberResponse> CreateCardiMemberAsync(
        Guid organizationId,
        Guid userId,
        CreateCardiMemberRequest request);

    Task<CardiMemberResponse?> GetByIdAsync(Guid id);
    Task<List<CardiMemberResponse>> GetByOrganizationIdAsync(Guid organizationId);
}
