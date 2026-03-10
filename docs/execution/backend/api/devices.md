# Device Management API

Handles wearable device connections via OAuth, device status management, primary device designation, and token refresh.

**User Stories:** 1.3 (Device Connection Wizard), 6.2 (Device Management)

---

## GET `/api/v1/cardimembers/{id}/devices`

List all wearable devices connected to a CardiMember.

**Priority:** P0 | **Auth Required:** Yes

### Path Parameters

| Parameter | Description |
|-----------|-------------|
| `id` | CardiMember ID |

### Response `200 OK`

```json
{
  "devices": [
    {
      "deviceId": "dev_01J9...",
      "provider": "fitbit",
      "displayName": "Fitbit Charge 6",
      "status": "active",
      "isPrimary": true,
      "lastSyncedAt": "2026-03-09T08:30:00Z",
      "connectedAt": "2026-01-15T09:00:00Z",
      "tokenExpiresAt": "2026-06-09T09:00:00Z"
    },
    {
      "deviceId": "dev_02J9...",
      "provider": "garmin",
      "displayName": "Garmin Venu 3",
      "status": "token_expired",
      "isPrimary": false,
      "lastSyncedAt": "2026-02-01T10:00:00Z",
      "connectedAt": "2025-12-01T09:00:00Z",
      "tokenExpiresAt": "2026-02-01T09:00:00Z"
    }
  ]
}
```

**Device Status Values:**

| Status | Description |
|--------|-------------|
| `active` | Syncing normally |
| `disconnected` | OAuth connection removed |
| `token_expired` | OAuth token needs refresh |
| `pending` | OAuth flow not yet completed |

---

## POST `/api/v1/cardimembers/{id}/devices`

Initiate an OAuth device connection. Returns a redirect URL for the provider's authorization page.

**Priority:** P0 | **Auth Required:** Yes

### Request Body

```json
{
  "provider": "fitbit",
  "redirectUri": "carditrack://oauth/callback"
}
```

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `provider` | string | Yes | One of: `fitbit`, `apple_health`, `garmin`, `samsung_health`, `withings` |
| `redirectUri` | string | Yes | Deep link URI for mobile callback |

### Response `200 OK`

```json
{
  "authorizationUrl": "https://www.fitbit.com/oauth2/authorize?client_id=...",
  "state": "csrf_state_token_abc123",
  "codeVerifier": "pkce_verifier_xyz"
}
```

> The client stores `codeVerifier` and `state` locally, then redirects the user to `authorizationUrl`. After authorization, the provider redirects to `redirectUri` with a `code` and `state` parameter, which is sent to the OAuth callback endpoint.

---

## GET `/api/v1/oauth/callback/{provider}`

OAuth callback handler. Exchanges the authorization code for an access token and stores the connection.

**Priority:** P0 | **Auth Required:** Yes (via state token)

### Path Parameters

| Parameter | Description |
|-----------|-------------|
| `provider` | OAuth provider name (e.g. `fitbit`) |

### Query Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `code` | string | Yes | Authorization code from provider |
| `state` | string | Yes | CSRF state token |
| `code_verifier` | string | Yes | PKCE verifier |

### Response `201 Created`

```json
{
  "deviceId": "dev_01J9...",
  "provider": "fitbit",
  "displayName": "Fitbit Charge 6",
  "status": "active",
  "isPrimary": false,
  "connectedAt": "2026-03-09T10:00:00Z"
}
```

### Errors

| Code | Status | Description |
|------|--------|-------------|
| `INVALID_STATE_TOKEN` | 400 | CSRF state mismatch |
| `OAUTH_EXCHANGE_FAILED` | 502 | Provider rejected code exchange |
| `PROVIDER_PERMISSION_DENIED` | 403 | User denied required scopes |

---

## GET `/api/v1/cardimembers/{id}/devices/{deviceId}`

Get details and current status for a single connected device.

**Priority:** P1 | **Auth Required:** Yes

### Response `200 OK`

```json
{
  "deviceId": "dev_01J9...",
  "provider": "fitbit",
  "displayName": "Fitbit Charge 6",
  "status": "active",
  "isPrimary": true,
  "scopes": ["activity", "heartrate", "sleep"],
  "lastSyncedAt": "2026-03-09T08:30:00Z",
  "connectedAt": "2026-01-15T09:00:00Z",
  "tokenExpiresAt": "2026-06-09T09:00:00Z"
}
```

### Errors

| Code | Status | Description |
|------|--------|-------------|
| `DEVICE_NOT_FOUND` | 404 | Device ID not found for this CardiMember |

---

## PUT `/api/v1/cardimembers/{id}/devices/{deviceId}/primary`

Set this device as the primary data source. Clears primary flag from any previously primary device.

**Priority:** P1 | **Auth Required:** Yes

### Response `200 OK`

```json
{
  "deviceId": "dev_01J9...",
  "isPrimary": true,
  "updatedAt": "2026-03-09T10:00:00Z"
}
```

---

## POST `/api/v1/cardimembers/{id}/devices/{deviceId}/reconnect`

Initiate a token refresh for a device with an expired or revoked OAuth token.

**Priority:** P1 | **Auth Required:** Yes

### Request Body

```json
{
  "redirectUri": "carditrack://oauth/callback"
}
```

### Response `200 OK`

```json
{
  "authorizationUrl": "https://www.fitbit.com/oauth2/authorize?client_id=...",
  "state": "csrf_state_token_def456",
  "codeVerifier": "pkce_verifier_new"
}
```

> Follows the same PKCE OAuth flow as initial connection.

---

## DELETE `/api/v1/cardimembers/{id}/devices/{deviceId}`

Remove a device connection. Historical data synced via this device is retained.

**Priority:** P1 | **Auth Required:** Yes | **Required Role:** Admin, Staff

### Response `204 No Content`

### Errors

| Code | Status | Description |
|------|--------|-------------|
| `CANNOT_DELETE_ONLY_DEVICE` | 422 | At least one device must remain connected |

---

**Supported Providers:**

| Provider | `provider` Value | Scopes Requested |
|----------|-----------------|-----------------|
| Fitbit | `fitbit` | `activity`, `heartrate`, `sleep` |
| Apple Health | `apple_health` | `HKQuantityTypeStepCount`, `HKQuantityTypeHeartRate`, `HKCategoryTypeAsleepCore` |
| Garmin | `garmin` | `activities`, `heart_rate`, `sleep` |
| Samsung Health | `samsung_health` | `steps`, `heart_rate`, `sleep` |
| Withings | `withings` | `user.metrics` |

---

**Related:** [README.md](README.md) | [health-data.md](health-data.md) | [User Stories 1.3, 6.2](../UI/MOBILE/USER_STORIES.md)
