# AUTH0 INTEGRATION - CARDITRACK

## OVERVIEW

CardiTrack uses **Auth0** as its authentication backend to provide secure, HIPAA-compliant user authentication with support for multiple authentication methods including social login, email/password, and enterprise SSO.

---

## AUTH0 CONFIGURATION

### **Auth0 Account Setup**

**Step 1: Create Auth0 Tenant**
```
Tenant Name: carditrack-prod (or carditrack-dev for development)
Region: US (for HIPAA compliance)
Environment Tag: Production
```

**Step 2: Enable HIPAA BAA**
- Contact Auth0 sales to enable HIPAA features
- Sign Business Associate Agreement (BAA)
- Enable HIPAA compliance mode (restricts certain features)
- Configure audit logging and retention policies

### **Application Configuration**

**Create Auth0 Application:**
```
Application Type: Regular Web Application
Name: CardiTrack Web
Technology: ASP.NET Core
```

**Application Settings:**
```
Client ID: {auto-generated}
Client Secret: {auto-generated} - STORE SECURELY
Application Login URI: https://app.carditrack.com/login
Allowed Callback URLs:
  - https://app.carditrack.com/auth/callback
  - https://localhost:7001/auth/callback (development)
  - carditrack://auth/callback (mobile app)

Allowed Logout URLs:
  - https://app.carditrack.com/
  - https://localhost:7001/ (development)

Allowed Web Origins:
  - https://app.carditrack.com
  - https://localhost:7001 (development)

Allowed Origins (CORS):
  - https://app.carditrack.com
  - https://localhost:7001 (development)
```

**Token Settings:**
```json
{
  "id_token_expiration": 36000,  // 10 hours
  "access_token_expiration": 3600, // 1 hour
  "refresh_token_rotation": "rotating",
  "refresh_token_expiration": 2592000, // 30 days
  "refresh_token_leeway": 0
}
```

---

## SOCIAL CONNECTIONS

### **Google OAuth 2.0**

**Setup:**
1. Go to [Google Cloud Console](https://console.cloud.google.com)
2. Create new project: "CardiTrack"
3. Enable Google+ API
4. Create OAuth 2.0 credentials:
   - Application type: Web application
   - Authorized redirect URIs: `https://carditrack-prod.us.auth0.com/login/callback`
5. Copy Client ID and Client Secret

**Auth0 Configuration:**
```
Connection Name: google-oauth2
Client ID: {from Google Console}
Client Secret: {from Google Console}
Scopes: email, profile
Attributes:
  - email (required)
  - name
  - picture
```

### **Microsoft Account**

**Setup:**
1. Go to [Azure Portal](https://portal.azure.com)
2. Register application in Azure AD
3. Create client secret
4. Add redirect URI: `https://carditrack-prod.us.auth0.com/login/callback`

**Auth0 Configuration:**
```
Connection Name: windowslive
Client ID: {Application (client) ID from Azure}
Client Secret: {Client secret value}
Scopes: openid, profile, email
```

### **Apple Sign In**

**Setup:**
1. Go to [Apple Developer Portal](https://developer.apple.com)
2. Create App ID with Sign In with Apple capability
3. Create Service ID
4. Configure Return URLs: `https://carditrack-prod.us.auth0.com/login/callback`
5. Create private key for Sign In with Apple

**Auth0 Configuration:**
```
Connection Name: apple
Client ID: {Service ID}
Team ID: {from Apple Developer account}
Key ID: {from private key}
Private Key: {upload .p8 file}
Scopes: name, email
```

### **Facebook Login (Optional)**

**Setup:**
1. Go to [Facebook Developers](https://developers.facebook.com)
2. Create app
3. Add Facebook Login product
4. Configure OAuth Redirect URIs: `https://carditrack-prod.us.auth0.com/login/callback`

**Auth0 Configuration:**
```
Connection Name: facebook
App ID: {from Facebook}
App Secret: {from Facebook}
Scopes: public_profile, email
```

---

## DATABASE CONNECTION (EMAIL/PASSWORD)

### **Auth0 Database Configuration**

**Connection Name:** Username-Password-Authentication

**Password Strength:**
```json
{
  "min_length": 8,
  "max_length": 128,
  "require_lowercase": true,
  "require_uppercase": true,
  "require_numbers": true,
  "require_symbols": true
}
```

**Password Policy:**
- Good: 8+ characters with mixed case, numbers, symbols
- Fair: 6+ characters
- Excellent: 10+ characters with all requirements + no dictionary words

**Security Features:**
- ✅ Breached password detection (enabled)
- ✅ Brute force protection (10 failed attempts → account locked)
- ✅ Password history (prevent reuse of last 5 passwords)
- ✅ Password expiration (optional, disabled for consumer app)

**Sign-Up Settings:**
```json
{
  "disable_signup": false,
  "requires_username": false,
  "custom_scripts": {
    "login": "// Custom login validation",
    "create": "// Custom signup validation"
  }
}
```

---

## ENTERPRISE CONNECTIONS (BUSINESS ACCOUNTS)

### **SAML 2.0 (Generic)**

For healthcare organizations with existing SAML IdP:

**Configuration:**
```
Connection Name: {organization-name}-saml
Sign In URL: {from enterprise IdP}
Sign Out URL: {from enterprise IdP}
X509 Signing Certificate: {from enterprise IdP}

User ID Attribute: http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier
Email Attribute: http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress
Name Attribute: http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name

IdP-Initiated SSO: Enabled (optional)
```

### **Azure AD (Enterprise)**

For Microsoft-based healthcare organizations:

**Configuration:**
```
Connection Name: {organization-name}-azure-ad
Microsoft Azure AD Domain: {organization}.onmicrosoft.com
Client ID: {from Azure AD app registration}
Client Secret: {from Azure AD}
Scopes: openid, profile, email

Sync user profile attributes at each login: Yes
Extended Attributes:
  - department
  - job_title
  - employee_id
```

### **Okta**

**Configuration:**
```
Connection Name: {organization-name}-okta
Okta Domain: {organization}.okta.com
Client ID: {from Okta application}
Client Secret: {from Okta}
Scopes: openid, profile, email
```

---

## AUTH0 RULES & ACTIONS

### **Rule 1: Add Custom Claims to Token**

```javascript
function addCustomClaims(user, context, callback) {
  const namespace = 'https://carditrack.com';

  // Add user metadata to access token
  if (user.user_metadata) {
    context.accessToken[namespace + '/organization_id'] = user.user_metadata.organization_id;
    context.accessToken[namespace + '/role'] = user.user_metadata.role;
    context.accessToken[namespace + '/user_id'] = user.user_metadata.carditrack_user_id;
  }

  // Add email verified status
  context.accessToken[namespace + '/email_verified'] = user.email_verified;

  callback(null, user, context);
}
```

### **Rule 2: Enforce Email Verification**

```javascript
function enforceEmailVerification(user, context, callback) {
  // Skip for social connections (auto-verified)
  const socialConnections = ['google-oauth2', 'windowslive', 'apple', 'facebook'];
  if (socialConnections.includes(context.connection)) {
    return callback(null, user, context);
  }

  // Require email verification for database connections
  if (!user.email_verified) {
    return callback(
      new UnauthorizedError('Please verify your email before logging in.')
    );
  }

  callback(null, user, context);
}
```

### **Rule 3: Block Suspended Accounts**

```javascript
function blockSuspendedAccounts(user, context, callback) {
  const namespace = 'https://carditrack.com';
  const ManagementClient = require('auth0@2.42.0').ManagementClient;

  const management = new ManagementClient({
    token: auth0.accessToken,
    domain: auth0.domain
  });

  // Check if user is suspended in our database
  const userId = user.user_metadata && user.user_metadata.carditrack_user_id;

  if (userId) {
    // Call CardiTrack API to check account status
    request.get({
      url: configuration.CARDITRACK_API_URL + '/api/auth/check-status',
      headers: {
        'Authorization': 'Bearer ' + configuration.CARDITRACK_API_SECRET
      },
      qs: {
        user_id: userId
      }
    }, (err, response, body) => {
      if (err) return callback(err);

      const status = JSON.parse(body);
      if (status.is_suspended || !status.is_active) {
        return callback(
          new UnauthorizedError('Your account has been suspended. Please contact support.')
        );
      }

      callback(null, user, context);
    });
  } else {
    callback(null, user, context);
  }
}
```

### **Rule 4: Multi-Factor Authentication (MFA)**

```javascript
function enforceMFA(user, context, callback) {
  const namespace = 'https://carditrack.com';

  // Require MFA for Admin and Staff roles
  const role = user.user_metadata && user.user_metadata.role;
  const requireMFA = ['Admin', 'Staff'].includes(role);

  if (requireMFA && context.authentication.methods.length === 1) {
    context.multifactor = {
      provider: 'any',
      allowRememberBrowser: false
    };
  }

  callback(null, user, context);
}
```

### **Action 1: Sync User to CardiTrack Database (Post-Login)**

```javascript
exports.onExecutePostLogin = async (event, api) => {
  const axios = require('axios');

  const auth0UserId = event.user.user_id;
  const email = event.user.email;
  const name = event.user.name;
  const picture = event.user.picture;
  const emailVerified = event.user.email_verified;

  try {
    // Sync user data to CardiTrack API
    await axios.post(
      `${event.secrets.CARDITRACK_API_URL}/api/auth/sync-user`,
      {
        auth0_user_id: auth0UserId,
        email: email,
        name: name,
        picture: picture,
        email_verified: emailVerified,
        last_login: new Date().toISOString()
      },
      {
        headers: {
          'Authorization': `Bearer ${event.secrets.CARDITRACK_API_SECRET}`
        }
      }
    );

    // Update last login in Auth0 metadata
    api.user.setUserMetadata('last_login_at', new Date().toISOString());

  } catch (error) {
    console.error('Failed to sync user:', error);
    // Don't block login if sync fails
  }
};
```

---

## AUTH0 APIs

### **Auth0 Management API**

Used for programmatic user management:

**Setup:**
1. Create Machine-to-Machine Application in Auth0
2. Authorize app for Auth0 Management API
3. Grant scopes:
   - `read:users`
   - `update:users`
   - `create:users`
   - `delete:users`
   - `read:user_idp_tokens`
   - `update:user_metadata`

**Usage in C#:**
```csharp
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;

public class Auth0Service
{
    private readonly IManagementApiClient _managementClient;

    public Auth0Service(IConfiguration config)
    {
        var token = GetManagementApiToken();
        _managementClient = new ManagementApiClient(
            token,
            new Uri($"https://{config["Auth0:Domain"]}/api/v2")
        );
    }

    public async Task UpdateUserMetadata(string auth0UserId, object metadata)
    {
        var updateRequest = new UserUpdateRequest
        {
            UserMetadata = metadata
        };

        await _managementClient.Users.UpdateAsync(auth0UserId, updateRequest);
    }

    public async Task<User> GetUserByAuth0Id(string auth0UserId)
    {
        return await _managementClient.Users.GetAsync(auth0UserId);
    }

    public async Task LinkAccounts(string primaryUserId, string secondaryUserId)
    {
        var linkRequest = new UserAccountLinkRequest
        {
            Provider = secondaryUserId.Split('|')[0],
            UserId = secondaryUserId.Split('|')[1]
        };

        await _managementClient.Users.LinkAccountAsync(primaryUserId, linkRequest);
    }

    private string GetManagementApiToken()
    {
        // Implement token caching
        // Exchange client credentials for access token
        var client = new HttpClient();
        var request = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("client_id", _config["Auth0:ManagementClientId"]),
            new KeyValuePair<string, string>("client_secret", _config["Auth0:ManagementClientSecret"]),
            new KeyValuePair<string, string>("audience", $"https://{_config["Auth0:Domain"]}/api/v2/")
        });

        var response = await client.PostAsync($"https://{_config["Auth0:Domain"]}/oauth/token", request);
        var content = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(content);

        return tokenResponse.AccessToken;
    }
}
```

---

## SECURITY CONFIGURATIONS

### **Attack Protection**

**Brute Force Protection:**
```json
{
  "enabled": true,
  "shields": ["block", "user_notification"],
  "allowlist": [],
  "mode": "count_per_identifier_and_ip",
  "max_attempts": 10,
  "triggers": [
    {
      "name": "consecutive_failed_logins"
    }
  ]
}
```

**Suspicious IP Throttling:**
```json
{
  "enabled": true,
  "shields": ["block"],
  "allowlist": [],
  "stage": {
    "pre-login": {
      "max_attempts": 100,
      "rate": 864000
    },
    "pre-user-registration": {
      "max_attempts": 50,
      "rate": 1200
    }
  }
}
```

**Breached Password Detection:**
```json
{
  "enabled": true,
  "shields": ["block", "user_notification"],
  "admin_notification_frequency": "daily",
  "method": "standard"
}
```

### **Bot Detection**

**reCAPTCHA Integration:**
```json
{
  "enabled": true,
  "provider": "recaptcha_enterprise",
  "site_key": "{Google reCAPTCHA site key}",
  "secret_key": "{Google reCAPTCHA secret key}",
  "score_threshold": 0.5
}
```

### **Anomaly Detection**

**Configuration:**
```json
{
  "enabled": true,
  "shields": ["block"],
  "triggers": [
    "impossible_travel",
    "new_device",
    "new_location",
    "velocity_attack"
  ]
}
```

---

## LOGGING & MONITORING

### **Log Streams**

**Stream to Azure Application Insights:**
```json
{
  "type": "http",
  "name": "Azure Application Insights",
  "sink": {
    "http_endpoint": "https://{app-insights}.azurewebsites.net/api/logs",
    "http_content_type": "application/json",
    "http_authorization": "Bearer {token}"
  },
  "filters": [
    {
      "type": "log_type",
      "name": "ss"  // Successful login
    },
    {
      "type": "log_type",
      "name": "f"   // Failed login
    },
    {
      "type": "log_type",
      "name": "fsa" // Failed signup
    }
  ]
}
```

**Important Log Event Types:**
- `s` - Success login
- `f` - Failed login
- `fsa` - Failed signup
- `fu` - Failed change password
- `pwd_leak` - Breached password
- `limit_wc` - Blocked account (brute force)
- `limit_mu` - Blocked IP address
- `api_limit` - Rate limit exceeded

### **Audit Log Retention**

For HIPAA compliance:
```
Retention Period: 90 days minimum (recommended 1 year)
Export to: Azure Blob Storage (encrypted)
Format: JSON
Schedule: Daily incremental backups
```

---

## APPSETTINGS CONFIGURATION

### **appsettings.json**

```json
{
  "Auth0": {
    "Domain": "carditrack-prod.us.auth0.com",
    "ClientId": "{your-client-id}",
    "ClientSecret": "{your-client-secret}",
    "Audience": "https://api.carditrack.com",
    "CallbackPath": "/auth/callback",
    "RedirectUri": "https://app.carditrack.com/auth/callback",
    "LogoutRedirectUri": "https://app.carditrack.com/",
    "Scopes": "openid profile email phone",

    "ManagementApi": {
      "ClientId": "{management-api-client-id}",
      "ClientSecret": "{management-api-client-secret}",
      "Audience": "https://carditrack-prod.us.auth0.com/api/v2/"
    },

    "Connections": {
      "Google": "google-oauth2",
      "Microsoft": "windowslive",
      "Apple": "apple",
      "Facebook": "facebook",
      "EmailPassword": "Username-Password-Authentication"
    }
  },

  "JwtBearer": {
    "Authority": "https://carditrack-prod.us.auth0.com/",
    "Audience": "https://api.carditrack.com",
    "ValidateIssuer": true,
    "ValidateAudience": true,
    "ValidateLifetime": true,
    "ClockSkew": "00:00:00"
  }
}
```

### **Environment Variables (Secrets)**

Store in Azure Key Vault or GitHub Secrets:
```
AUTH0_DOMAIN=carditrack-prod.us.auth0.com
AUTH0_CLIENT_ID={client-id}
AUTH0_CLIENT_SECRET={client-secret}
AUTH0_MANAGEMENT_CLIENT_ID={management-client-id}
AUTH0_MANAGEMENT_CLIENT_SECRET={management-client-secret}
```

---

## TESTING

### **Auth0 Test Users**

Create test users for each connection type:
```
Email/Password: test@carditrack.com (password: TestPass123!)
Google: testuser@gmail.com (configure in Google Workspace)
Microsoft: testuser@outlook.com
```

### **Integration Tests**

```csharp
[Fact]
public async Task Auth0_Login_ReturnsValidJWT()
{
    // Arrange
    var client = _factory.CreateClient();
    var authUrl = $"https://{_auth0Config.Domain}/authorize?...";

    // Act
    var response = await client.GetAsync(authUrl);

    // Assert
    Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
}

[Fact]
public async Task JWT_Validation_AcceptsValidToken()
{
    // Arrange
    var validToken = await GetTestJWTToken();

    // Act
    var result = await _jwtValidator.ValidateToken(validToken);

    // Assert
    Assert.True(result.IsValid);
    Assert.NotNull(result.Claims["sub"]);
}
```

---

## COST ESTIMATE

**Auth0 Pricing (HIPAA-Compliant Plan):**
- **Professional Plan**: $240/month (up to 1,000 active users)
- **BAA Add-on**: Included in Professional plan
- **Additional Users**: $0.40/month per additional MAU (Monthly Active User)
- **MFA**: SMS-based MFA costs extra via Twilio

**Projected Costs:**
```
0-1,000 users: $240/month
1,000-10,000 users: $240 + (9,000 × $0.40) = $3,840/month
10,000-100,000 users: Enterprise pricing (contact sales)
```

**Cost per User:**
- 1,000 MAU: $0.24/user/month
- 10,000 MAU: $0.38/user/month
- Still cheaper than building custom auth + HIPAA compliance

---

## MIGRATION PLAN

### **Phase 1: Setup (Week 1)**
- [ ] Create Auth0 tenant
- [ ] Sign BAA
- [ ] Configure social connections
- [ ] Set up development environment

### **Phase 2: Integration (Week 2-3)**
- [ ] Implement Auth0 SDK in .NET backend
- [ ] Add Universal Login to web app
- [ ] Implement JWT validation middleware
- [ ] Add user sync endpoints

### **Phase 3: Testing (Week 4)**
- [ ] Test all authentication flows
- [ ] Validate JWT token handling
- [ ] Test MFA enforcement
- [ ] Security audit

### **Phase 4: Production (Week 5)**
- [ ] Deploy to production
- [ ] Configure production connections
- [ ] Enable monitoring and logging
- [ ] User acceptance testing

---

## SUPPORT & RESOURCES

**Auth0 Documentation:**
- [Quickstart: ASP.NET Core](https://auth0.com/docs/quickstart/webapp/aspnet-core)
- [HIPAA Compliance Guide](https://auth0.com/docs/compliance/hipaa)
- [Management API Reference](https://auth0.com/docs/api/management/v2)

**Support Channels:**
- Auth0 Community Forum
- Professional Plan: Email support
- Enterprise: 24/7 phone support

---

**END OF AUTH0 INTEGRATION DOCUMENTATION**
