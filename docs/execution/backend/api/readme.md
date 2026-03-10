# CardiTrack Backend API Documentation

This directory documents the REST API endpoints required to support the CardiTrack mobile and web applications, derived from the [Mobile User Stories](../UI/MOBILE/USER_STORIES.md).

## Base URL

```
https://api.carditrack.com/api/v1
```

## Authentication

All endpoints (except auth routes) require a JWT Bearer token issued by Auth0.

```
Authorization: Bearer <access_token>
```

Tokens expire after **7 days** on mobile. Re-authentication or biometric re-verification is required upon expiry.

## Versioning

All routes are prefixed with `/api/v1/`. Breaking changes will increment the version.

## Standard Error Format

```json
{
  "error": {
    "code": "RESOURCE_NOT_FOUND",
    "message": "CardiMember with id 'abc123' was not found.",
    "traceId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  }
}
```

| HTTP Status | When Used |
|-------------|-----------|
| 200 | Success |
| 201 | Resource created |
| 204 | Success, no content |
| 400 | Validation error / bad request |
| 401 | Missing or invalid token |
| 403 | Authenticated but not authorized |
| 404 | Resource not found |
| 409 | Conflict (e.g. duplicate invite) |
| 422 | Business rule violation |
| 500 | Internal server error |

## MVP Priority Legend

| Priority | Meaning |
|----------|---------|
| **P0** | Must Have — MVP launch blocker |
| **P1** | Should Have — MVP launch goal |
| **P2** | Nice to Have — post-launch sprint |
| **Future** | Post-MVP roadmap |

## API Domains

| File | Domain | Key User Stories |
|------|--------|-----------------|
| [auth.md](auth.md) | Authentication | 1.1, 10.2 |
| [cardimembers.md](cardimembers.md) | CardiMember Management | 1.2, 7.1, 7.2, 7.3 |
| [devices.md](devices.md) | Device Management | 1.3, 6.2 |
| [health-data.md](health-data.md) | Health Data & Dashboard | 2.1, 2.2, 2.3, 5.2, 10.1 |
| [alerts.md](alerts.md) | Alerts & Notification Preferences | 3.1, 3.2, 3.3, 11.1–11.3 |
| [family.md](family.md) | Family Collaboration | 4.1, 4.2, 8.3 |
| [notifications.md](notifications.md) | Push Notifications | 3.2, 5.1 |
| [subscriptions.md](subscriptions.md) | Subscription Management | 6.1 |
| [reports.md](reports.md) | Reports & Exports | 2.3, 9.2 |

## Related Documentation

- [Mobile User Stories](../UI/MOBILE/USER_STORIES.md)
- [Web User Stories](../UI/WEB/USER_STORIES.md)
- [Entity Summary](../../technical/ENTITY_SUMMARY.md)
- [Auth0 Integration](../../technical/AUTH0_INTEGRATION.md)
- [User Onboarding Process](../../technical/USER_ONBOARDING_PROCESS.md)

---

**Document Version:** 1.0
**Last Updated:** March 2026
**Owner:** Backend Engineering Team
