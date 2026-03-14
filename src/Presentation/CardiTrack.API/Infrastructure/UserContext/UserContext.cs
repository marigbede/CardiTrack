using CardiTrack.Domain.Enums;

namespace CardiTrack.API.Infrastructure.UserContext;

public class UserContext : IUserContext
{
    public Guid UserId { get; private set; }
    public string Auth0UserId { get; private set; } = string.Empty;
    public Guid OrganizationId { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public UserRole Role { get; private set; }
    public bool IsAuthenticated { get; private set; }
    public string Locale { get; private set; } = "en-US";

    public void SetAuthenticatedUser(string auth0UserId, string email, string locale)
    {
        Auth0UserId = auth0UserId;
        Email = email;
        Locale = locale;
        IsAuthenticated = true;
    }

    public void SetFullUserContext(Guid userId, Guid organizationId, UserRole role)
    {
        UserId = userId;
        OrganizationId = organizationId;
        Role = role;
    }
}
