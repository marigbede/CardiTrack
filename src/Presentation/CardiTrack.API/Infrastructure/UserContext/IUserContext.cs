using CardiTrack.Domain.Enums;

namespace CardiTrack.API.Infrastructure.UserContext;

public interface IUserContext
{
    Guid UserId { get; }
    string Auth0UserId { get; }
    Guid OrganizationId { get; }
    string Email { get; }
    UserRole Role { get; }
    bool IsAuthenticated { get; }
}
