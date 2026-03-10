# Authentication API

Handles user registration, login, social auth via Auth0, and biometric authentication for the mobile app.

**User Stories:** 1.1 (First-Time User Registration), 10.2 (Biometric Login)

---

## POST `/api/v1/auth/register`

Register a new user with email and password.

**Priority:** P0 | **Auth Required:** No

### Request Body

```json
{
  "email": "jane@example.com",
  "password": "SecurePass123!",
  "acceptedTerms": true
}
```

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `email` | string | Yes | Valid email address |
| `password` | string | Yes | Min 8 chars, 1 uppercase, 1 number |
| `acceptedTerms` | boolean | Yes | Must be `true` |

### Response `201 Created`

```json
{
  "userId": "usr_01J8K2...",
  "email": "jane@example.com",
  "accessToken": "eyJhbGciOi...",
  "refreshToken": "v1.MRjq...",
  "expiresIn": 604800
}
```

### Errors

| Code | Status | Description |
|------|--------|-------------|
| `EMAIL_ALREADY_EXISTS` | 409 | Email is already registered |
| `TERMS_NOT_ACCEPTED` | 400 | `acceptedTerms` must be true |
| `WEAK_PASSWORD` | 400 | Password does not meet requirements |

---

## POST `/api/v1/auth/login`

Authenticate with email and password.

**Priority:** P0 | **Auth Required:** No

### Request Body

```json
{
  "email": "jane@example.com",
  "password": "SecurePass123!"
}
```

### Response `200 OK`

```json
{
  "userId": "usr_01J8K2...",
  "email": "jane@example.com",
  "accessToken": "eyJhbGciOi...",
  "refreshToken": "v1.MRjq...",
  "expiresIn": 604800
}
```

### Errors

| Code | Status | Description |
|------|--------|-------------|
| `INVALID_CREDENTIALS` | 401 | Email or password incorrect |
| `ACCOUNT_LOCKED` | 403 | Too many failed attempts |

---

## POST `/api/v1/auth/social`

Initiate or complete social login via Google or Apple (Auth0 PKCE flow).

**Priority:** P0 | **Auth Required:** No

### Request Body

```json
{
  "provider": "google",
  "idToken": "eyJhbGciOi...",
  "acceptedTerms": true
}
```

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `provider` | string | Yes | `"google"` or `"apple"` |
| `idToken` | string | Yes | ID token from provider |
| `acceptedTerms` | boolean | Yes (new users) | Required on first login |

### Response `200 OK`

```json
{
  "userId": "usr_01J8K2...",
  "email": "jane@example.com",
  "isNewUser": true,
  "accessToken": "eyJhbGciOi...",
  "refreshToken": "v1.MRjq...",
  "expiresIn": 604800
}
```

### Errors

| Code | Status | Description |
|------|--------|-------------|
| `INVALID_ID_TOKEN` | 401 | Token verification failed |
| `TERMS_NOT_ACCEPTED` | 400 | New user must accept terms |

---

## POST `/api/v1/auth/refresh`

Exchange a refresh token for a new access token.

**Priority:** P0 | **Auth Required:** No

### Request Body

```json
{
  "refreshToken": "v1.MRjq..."
}
```

### Response `200 OK`

```json
{
  "accessToken": "eyJhbGciOi...",
  "expiresIn": 604800
}
```

### Errors

| Code | Status | Description |
|------|--------|-------------|
| `INVALID_REFRESH_TOKEN` | 401 | Token is expired or invalid |

---

## POST `/api/v1/auth/biometric/setup`

Register a device biometric credential (Face ID / Touch ID) for the authenticated user.

**Priority:** P1 | **Auth Required:** Yes

### Request Body

```json
{
  "deviceId": "device_abc123",
  "platform": "ios",
  "biometricPublicKey": "MIIBIjANBgkq..."
}
```

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `deviceId` | string | Yes | Unique device identifier |
| `platform` | string | Yes | `"ios"` or `"android"` |
| `biometricPublicKey` | string | Yes | Public key from device secure enclave |

### Response `201 Created`

```json
{
  "biometricId": "bio_xyz789",
  "deviceId": "device_abc123",
  "createdAt": "2026-03-09T10:00:00Z"
}
```

### Errors

| Code | Status | Description |
|------|--------|-------------|
| `DEVICE_ALREADY_REGISTERED` | 409 | Biometric already set up for this device |

---

## POST `/api/v1/auth/biometric/verify`

Authenticate using a biometric signature. Returns a new access token without requiring password.

**Priority:** P1 | **Auth Required:** No

### Request Body

```json
{
  "biometricId": "bio_xyz789",
  "deviceId": "device_abc123",
  "signature": "Base64EncodedSignature=="
}
```

### Response `200 OK`

```json
{
  "userId": "usr_01J8K2...",
  "accessToken": "eyJhbGciOi...",
  "refreshToken": "v1.MRjq...",
  "expiresIn": 604800
}
```

### Errors

| Code | Status | Description |
|------|--------|-------------|
| `INVALID_SIGNATURE` | 401 | Biometric verification failed |
| `BIOMETRIC_NOT_FOUND` | 404 | No biometric registered for this device |
| `BIOMETRIC_EXPIRED` | 401 | Re-authentication required (every 7 days) |

---

**Related:** [README.md](README.md) | [User Stories 1.1, 10.2](../UI/MOBILE/USER_STORIES.md)
