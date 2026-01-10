# CardiTrack Blazor Web UI Screen Descriptions

Detailed UI specifications for the Blazor Server web application.

**Platform:** Blazor Server (.NET 8)
**Target Browsers:** Chrome, Firefox, Safari, Edge (latest 2 versions)
**Responsive Breakpoints:** Mobile (< 768px), Tablet (768-1024px), Desktop (> 1024px)
**Document Version:** 1.0
**Last Updated:** January 10, 2026

---

## Table of Contents

1. [Web App Architecture](#web-app-architecture)
2. [Authentication & Onboarding](#authentication--onboarding)
3. [Dashboard Screens](#dashboard-screens)
4. [Alert Management](#alert-management)
5. [CardiMember Management](#cardimember-management)
6. [Family Collaboration](#family-collaboration)
7. [Analytics & Reporting](#analytics--reporting)
8. [Settings & Administration](#settings--administration)
9. [Enterprise Features](#enterprise-features)
10. [Component Library](#component-library)

---

## Web App Architecture

### Layout Structure
```
┌─────────────────────────────────────────┐
│            Top Navigation Bar            │
├──────┬──────────────────────────────────┤
│      │                                   │
│ Side │      Main Content Area            │
│ Nav  │      (Blazor Page Content)        │
│      │                                   │
│      │                                   │
└──────┴──────────────────────────────────┘
```

### Navigation Components
- **Top Bar:** Logo, search, notifications, user menu
- **Side Navigation:** Collapsible sidebar with main menu
- **Breadcrumbs:** Current location path
- **SignalR:** Real-time updates for alerts and data

### Responsive Behavior
- **Desktop (> 1024px):** Sidebar always visible, multi-column layouts
- **Tablet (768-1024px):** Collapsible sidebar, 2-column layouts
- **Mobile (< 768px):** Hamburger menu, single column, bottom nav

---

## Authentication & Onboarding

### Screen 1.1: Landing Page (Public)
**URL:** `/`
**User Story:** 1.1 - Value Proposition

**Layout:**

**Navigation Bar (Fixed):**
```
┌────────────────────────────────────────────────────┐
│ [CardiTrack Logo]           [Pricing] [Login] [Start Free Trial] │
└────────────────────────────────────────────────────┘
```

**Hero Section (Full viewport height):**
```
┌─────────────────────────────────────────┐
│  [Left: 50%]              [Right: 50%]   │
│                                          │
│  Peace of Mind for        [Hero Image:   │
│  Your Family              Elderly person │
│                           with smartwatch│
│  Monitor elderly loved    + caring       │
│  ones' health from        family]        │
│  anywhere using devices                  │
│  they already own.                       │
│                                          │
│  ✓ Works with Fitbit,                   │
│    Apple Watch, Garmin                   │
│  ✓ AI-powered health                    │
│    alerts                                │
│  ✓ $8/month - 70% less                  │
│    than medical alert                    │
│    systems                               │
│                                          │
│  [Start Free 30-Day Trial]              │
│  No credit card required                 │
│                                          │
└─────────────────────────────────────────┘
```

**How It Works Section:**
```
┌──────────────────────────────────────┐
│       How CardiTrack Works           │
│                                      │
│  [1. Connect]  [2. Monitor]  [3. Alert] │
│   [Icon]        [Icon]        [Icon]    │
│   Connect       We learn      Get       │
│   wearable      patterns      notified  │
│   devices       & monitor     of changes│
│                                      │
│  [Watch Demo Video (2 min)]          │
└──────────────────────────────────────┘
```

**Pricing Cards:**
```
┌─────────────────────────────────────────────────┐
│              Choose Your Plan                    │
│                                                  │
│ [Basic]        [Complete Care]   [Guardian Plus]│
│  $8/mo         $15/mo ⭐         $30/mo          │
│                POPULAR                           │
│  • 1 member    • Unlimited       • Unlimited    │
│  • Basic       • Advanced ML     • Predictive   │
│    alerts      • 5 family        • Unlimited    │
│  • 30 days       members           family       │
│    data        • 90 days data    • 1 year data  │
│                                  • Phone support│
│  [Start Free]  [Start Free]     [Start Free]    │
└─────────────────────────────────────────────────┘
```

**Social Proof:**
```
"CardiTrack gave me peace of mind knowing my mom
 is safe, even from 500 miles away."
 - Sarah J., Daughter & Caregiver

[More testimonials carousel]
```

**Footer:**
- About | Privacy | Terms | Contact | Blog
- © 2026 CardiTrack

**Blazor Components:**
```razor
@page "/"
<LandingPageHero />
<HowItWorksSection />
<PricingCards />
<TestimonialsCarousel />
<Footer />
```

---

### Screen 1.2: Sign Up Page
**URL:** `/signup`
**User Story:** 1.1 - Account Creation

**Layout (Centered card):**
```
┌─────────────────────────────────────┐
│    [CardiTrack Logo]                │
│                                     │
│    Create Your Account              │
│    Start your free 30-day trial     │
│                                     │
│    Email Address                    │
│    [____________________________]   │
│                                     │
│    Password                         │
│    [____________________________]   │
│    [Strength: ████░░░░ Strong]      │
│                                     │
│    Confirm Password                 │
│    [____________________________]   │
│                                     │
│    ☑ I agree to Terms of Service   │
│       and Privacy Policy            │
│                                     │
│    [Create Account]                 │
│                                     │
│    ────────── OR ──────────         │
│                                     │
│    [⚪ Continue with Google]        │
│    [⚫ Continue with Apple]          │
│                                     │
│    Already have an account? Sign In │
└─────────────────────────────────────┘
```

**Validation (Real-time):**
- Email format validation
- Password strength indicator (updates as typing)
- Confirm password match
- Terms checkbox required

**Blazor Components:**
```razor
@page "/signup"
<EditForm Model="@signupModel" OnValidSubmit="@HandleSignup">
  <DataAnnotationsValidator />
  <ValidationSummary />

  <InputText @bind-Value="signupModel.Email" />
  <InputText type="password" @bind-Value="signupModel.Password" />
  <PasswordStrengthIndicator Password="@signupModel.Password" />
  <InputCheckbox @bind-Value="signupModel.AgreeToTerms" />

  <button type="submit">Create Account</button>
</EditForm>

<SocialLoginButtons />
```

**After Signup:**
- Redirect to `/onboarding/add-member`
- Email verification sent (background)

---

### Screen 1.3: Onboarding - Add CardiMember
**URL:** `/onboarding/add-member`
**User Story:** 1.2 - Adding First CardiMember

**Layout:**
```
┌──────────────────────────────────────────┐
│  CardiTrack Setup                        │
│  [━━━━━━━━░░░░] Step 1 of 3             │
│                                          │
│  Who would you like to monitor?          │
│                                          │
│  ┌──────────────────────────────┐       │
│  │                              │       │
│  │    [Upload Photo Area]       │       │
│  │    [  📷 Add Photo  ]        │       │
│  │                              │       │
│  └──────────────────────────────┘       │
│                                          │
│  Full Name *                             │
│  [________________________________]      │
│                                          │
│  Date of Birth *                         │
│  [MM] / [DD] / [YYYY]                   │
│                                          │
│  Relationship *                          │
│  [▼ Select relationship ──────]         │
│      Parent                              │
│      Grandparent                         │
│      Spouse                              │
│      Other                               │
│                                          │
│  ▶ Add More Details (Optional)          │
│  └─ Medical Notes (Encrypted) 🔒        │
│     [_____________________________]      │
│                                          │
│  └─ Emergency Contact                   │
│     Name: [____________________]         │
│     Phone: [___________________]         │
│                                          │
│  💡 Your parent will be notified and     │
│     can provide consent                  │
│                                          │
│  [Skip for Now]        [Continue →]     │
└──────────────────────────────────────────┘
```

**Blazor Components:**
```razor
@page "/onboarding/add-member"

<ProgressBar CurrentStep="1" TotalSteps="3" />

<EditForm Model="@cardiMember" OnValidSubmit="@SaveMember">
  <InputFile OnChange="@HandlePhotoUpload" accept="image/*" />
  <InputText @bind-Value="cardiMember.Name" />
  <InputDate @bind-Value="cardiMember.DateOfBirth" />
  <InputSelect @bind-Value="cardiMember.Relationship">
    <option value="Parent">Parent</option>
    <option value="Grandparent">Grandparent</option>
    <!-- ... -->
  </InputSelect>

  <Collapsible Title="Add More Details (Optional)">
    <InputTextArea @bind-Value="cardiMember.MedicalNotes" />
    <InputText @bind-Value="cardiMember.EmergencyContactName" />
    <InputText @bind-Value="cardiMember.EmergencyContactPhone" />
  </Collapsible>

  <button type="button" @onclick="Skip">Skip for Now</button>
  <button type="submit">Continue</button>
</EditForm>
```

---

### Screen 1.4: Onboarding - Connect Device
**URL:** `/onboarding/connect-device`
**User Story:** 1.3 - Device Connection

**Layout:**
```
┌──────────────────────────────────────────┐
│  CardiTrack Setup                        │
│  [━━━━━━━━━━━━░░] Step 2 of 3           │
│                                          │
│  Connect [Dad]'s Device                  │
│                                          │
│  Select the wearable device:             │
│                                          │
│  ┌─────────┐ ┌─────────┐ ┌─────────┐   │
│  │ [Fitbit]│ │[Apple  ]│ │[Garmin ]│   │
│  │  Logo   │ │Watch    │ │  Logo   │   │
│  │         │ │ Logo    │ │         │   │
│  │[Connect]│ │[Connect]│ │[Connect]│   │
│  └─────────┘ └─────────┘ └─────────┘   │
│                                          │
│  ┌─────────┐ ┌─────────┐ ┌─────────┐   │
│  │[Samsung]│ │[Withings│ │[Other  ]│   │
│  │ Galaxy  │ │  Logo   │ │ Manual  │   │
│  │  Watch  │ │         │ │  Entry  │   │
│  │[Connect]│ │[Connect]│ │[Setup  ]│   │
│  └─────────┘ └─────────┘ └─────────┘   │
│                                          │
│  Don't see your device? Contact support │
│                                          │
│  [← Back]                  [Skip →]     │
└──────────────────────────────────────────┘
```

**After clicking "Connect":**
```
┌──────────────────────────────────────────┐
│  Connect Fitbit                          │
│                                          │
│  ┌────────┐                              │
│  │ Fitbit │ ──→ ┌──────────┐            │
│  │  Logo  │     │CardiTrack│            │
│  └────────┘     │   Logo   │            │
│                 └──────────┘             │
│                                          │
│  CardiTrack needs access to:             │
│                                          │
│  ❤️ Heart Rate Data      [Why?] ℹ️      │
│     To detect unusual patterns           │
│                                          │
│  👟 Activity & Steps     [Why?] ℹ️      │
│     To monitor daily movement            │
│                                          │
│  😴 Sleep Data           [Why?] ℹ️      │
│     To spot rest pattern changes         │
│                                          │
│  🔒 We never sell your data. Your        │
│     information stays private.           │
│                                          │
│  [Cancel]     [Authorize Fitbit →]      │
└──────────────────────────────────────────┘
```

**OAuth Flow:**
- Click "Authorize" → Opens OAuth window (popup or new tab)
- After authorization → Returns to success screen

**Success Screen:**
```
┌──────────────────────────────────────────┐
│  ✅ Connected Successfully!              │
│                                          │
│  We're syncing Dad's data from Fitbit    │
│  This may take a few minutes...          │
│                                          │
│  Latest Data Preview:                    │
│  ┌────────────────────────────┐         │
│  │ Steps Today:    4,250       │         │
│  │ Heart Rate:     72 bpm      │         │
│  │ Last Synced:    Just now    │         │
│  └────────────────────────────┘         │
│                                          │
│  [+ Add Another Device]                  │
│                                          │
│  [Continue to Dashboard →]               │
└──────────────────────────────────────────┘
```

**Blazor Components:**
```razor
@page "/onboarding/connect-device"
@inject NavigationManager Navigation

<DeviceGrid Devices="@supportedDevices" OnDeviceSelected="@HandleDeviceSelection" />

@if (showOAuthConsent)
{
  <OAuthConsentModal
    DeviceType="@selectedDevice"
    OnAuthorize="@HandleOAuthAuthorize"
    OnCancel="@CancelOAuth" />
}

@if (connectionSuccess)
{
  <ConnectionSuccessScreen
    DeviceName="@selectedDevice"
    PreviewData="@syncedData"
    OnContinue="@NavigateToDashboard" />
}
```

**OAuth Implementation:**
```csharp
private async Task HandleOAuthAuthorize()
{
    var authUrl = await _deviceService.GetOAuthUrl(selectedDevice);
    // Open popup window
    await JSRuntime.InvokeVoidAsync("window.open", authUrl, "_blank");
    // Listen for OAuth callback
}
```

---

## Dashboard Screens

### Screen 2.1: Main Dashboard (Authenticated)
**URL:** `/dashboard`
**User Story:** 2.1 - Daily Health Overview

**Layout (Desktop):**
```
┌────────────────────────────────────────────────────────┐
│ [CardiTrack]  [Search...]  [🔔3] [User ▼]             │
├──────┬────────────────────────────────────────────────┤
│      │  Dashboard > Overview                          │
│ 🏠   │                                                │
│ Dash │  Good Morning, Sarah                          │
│      │                                                │
│ 👥   │  ┌─────────────────────────────────────┐      │
│ Carди│  │  [Dad Photo]     All Good! ✓         │      │
│Membersкак│  │  Dad, 78          🟢                │      │
│      │  │                                       │      │
│ 🔔   │  │  Last synced: 10 minutes ago         │      │
│ Alerts│  │  📞 Call  💬 Message  📊 Details    │      │
│  (3) │  └─────────────────────────────────────┘      │
│      │                                                │
│ 👨‍👩‍👧│  ┌──────────┐ ┌──────────┐ ┌──────────┐    │
│ Family│  │👟 STEPS  │ │❤️ HEART  │ │😴 SLEEP  │    │
│      │  │4,250     │ │72 bpm    │ │7.2 hrs   │    │
│ ⚙️   │  │━━━━━░85% │ │Normal ✓  │ │Good ⭐⭐⭐│    │
│ Set- │  │↓15% below│ │68-75 bpm │ │Better    │    │
│tings │  │[Chart──] │ │[Chart──] │ │[Chart──] │    │
│      │  └──────────┘ └──────────┘ └──────────┘    │
│      │                                                │
│      │  Recent Alerts                                │
│      │  ┌──────────────────────────────────────┐    │
│      │  │ ⚠️ Low Activity  │ Yesterday, 2pm    │    │
│      │  │ Acknowledged by you                   │    │
│      │  └──────────────────────────────────────┘    │
│      │                                                │
│      │  [View Full Trends & History →]              │
└──────┴────────────────────────────────────────────────┘
```

**Layout (Tablet/Mobile - Responsive):**
- Sidebar collapses to hamburger menu
- Cards stack vertically
- Bottom navigation bar appears

**Blazor Components:**
```razor
@page "/dashboard"
@attribute [Authorize]
@inject ICardiMemberService MemberService
@inject IAlertService AlertService
@inherits LayoutComponentBase

<PageTitle>Dashboard - CardiTrack</PageTitle>

<div class="dashboard-container">
  <WelcomeHeader UserName="@currentUser.FirstName" />

  @if (primaryMember != null)
  {
    <StatusHeroCard
      Member="@primaryMember"
      Status="@memberStatus"
      LastSynced="@lastSyncTime"
      OnCall="@HandleCall"
      OnMessage="@HandleMessage"
      OnViewDetails="@NavigateToDetails" />
  }

  <div class="metrics-grid">
    <MetricCard
      Icon="steps"
      Title="STEPS"
      Value="@activityData.Steps"
      Comparison="@activityData.ComparisonText"
      ChartData="@activityData.SparklineData"
      Status="@activityData.Status" />

    <MetricCard
      Icon="heart"
      Title="HEART RATE"
      Value="@heartRateData.CurrentBpm"
      Comparison="@heartRateData.Status"
      ChartData="@heartRateData.SparklineData"
      Status="@heartRateData.Status" />

    <MetricCard
      Icon="sleep"
      Title="SLEEP"
      Value="@sleepData.Hours"
      Comparison="@sleepData.Quality"
      ChartData="@sleepData.SparklineData"
      Status="@sleepData.Status" />
  </div>

  @if (recentAlerts.Any())
  {
    <RecentAlertsSection Alerts="@recentAlerts" />
  }

  <NavLink href="/trends">
    <button class="btn-secondary">View Full Trends & History</button>
  </NavLink>
</div>

@code {
  private CardiMember primaryMember;
  private HealthStatus memberStatus;
  private DateTime lastSyncTime;
  private List<Alert> recentAlerts;

  protected override async Task OnInitializedAsync()
  {
    primaryMember = await MemberService.GetPrimaryMemberAsync();
    memberStatus = await MemberService.GetCurrentStatusAsync(primaryMember.Id);
    recentAlerts = await AlertService.GetRecentAlertsAsync(primaryMember.Id, 5);
  }
}
```

**Real-Time Updates (SignalR):**
```razor
@implements IAsyncDisposable
@inject NavigationManager Navigation
@inject IJSRuntime JS

@code {
  private HubConnection hubConnection;

  protected override async Task OnInitializedAsync()
  {
    hubConnection = new HubConnectionBuilder()
      .WithUrl(Navigation.ToAbsoluteUri("/healthHub"))
      .Build();

    hubConnection.On<HealthDataUpdate>("ReceiveHealthUpdate", async (update) =>
    {
      // Update UI with new data
      await InvokeAsync(StateHasChanged);
      // Show toast notification
      await JS.InvokeVoidAsync("showToast", "New data synced!");
    });

    await hubConnection.StartAsync();
  }

  public async ValueTask DisposeAsync()
  {
    if (hubConnection is not null)
    {
      await hubConnection.DisposeAsync();
    }
  }
}
```

---

### Screen 2.2: Multi-Member Dashboard
**URL:** `/cardimembers`
**User Story:** 2.2 - Multi-Member View

**Layout:**
```
┌────────────────────────────────────────────────────────┐
│ Dashboard > CardiMembers                               │
│                                                        │
│ Your CardiMembers                 [Filter ▼] [+ Add]  │
│                                                        │
│ [All] [Alerts Only] [Good Status]    Sort: Status ▼  │
│                                                        │
│ ┌──────────────────────────────────────────────────┐ │
│ │ [Photo] Dad, 78 • Parent           All Good  🟢  │ │
│ │         4,250 steps | 72 bpm | 7h sleep          │ │
│ │         Last synced: 10 min ago       [Details→]│ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ ┌──────────────────────────────────────────────────┐ │
│ │ [Photo] Mom, 75 • Parent        Needs Attn ⚠️ 🟡 │ │
│ │         2,100 steps | 68 bpm | 6h sleep  [!2]   │ │
│ │         Last synced: 5 min ago        [Details→]│ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ ┌──────────────────────────────────────────────────┐ │
│ │ [Photo] Grandma, 82 • Grandparent  All Good 🟢   │ │
│ │         3,500 steps | 70 bpm | 8h sleep          │ │
│ │         Last synced: 2 hours ago      [Details→]│ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
└────────────────────────────────────────────────────────┘
```

**Filter Options (Dropdown):**
```
┌───────────────────┐
│ ☑ Show All        │
│ ☐ Alerts Only     │
│ ☐ Good Status     │
│ ☐ Not Synced Today│
├───────────────────┤
│ Sort By:          │
│ ○ Status          │
│ ○ Name            │
│ ○ Last Synced     │
│ ○ Alert Count     │
└───────────────────┘
```

**Blazor Components:**
```razor
@page "/cardimembers"
@attribute [Authorize]

<PageTitle>CardiMembers - CardiTrack</PageTitle>

<div class="page-header">
  <h1>Your CardiMembers</h1>
  <div class="actions">
    <FilterDropdown @bind-Value="@currentFilter" Options="@filterOptions" />
    <button class="btn-primary" @onclick="@AddCardiMember">+ Add</button>
  </div>
</div>

<FilterChips @bind-SelectedFilter="@currentFilter" />

<div class="sort-bar">
  <SortDropdown @bind-Value="@currentSort" Options="@sortOptions" />
</div>

<div class="member-list">
  @foreach (var member in filteredMembers)
  {
    <CardiMemberCard
      Member="@member"
      Status="@GetMemberStatus(member.Id)"
      LatestMetrics="@GetLatestMetrics(member.Id)"
      AlertCount="@GetAlertCount(member.Id)"
      OnViewDetails="@(() => NavigateToDetails(member.Id))" />
  }
</div>

@if (!filteredMembers.Any())
{
  <EmptyState
    Icon="user-plus"
    Message="No CardiMembers match your filter"
    ActionText="Clear Filters"
    OnAction="@ClearFilters" />
}

@code {
  private List<CardiMember> allMembers = new();
  private List<CardiMember> filteredMembers = new();
  private FilterOption currentFilter = FilterOption.All;
  private SortOption currentSort = SortOption.Status;

  protected override async Task OnInitializedAsync()
  {
    allMembers = await MemberService.GetAllMembersAsync();
    ApplyFiltersAndSort();
  }

  private void ApplyFiltersAndSort()
  {
    filteredMembers = allMembers
      .Where(m => FilterPredicate(m, currentFilter))
      .OrderBy(m => SortKey(m, currentSort))
      .ToList();
  }
}
```

---

### Screen 2.3: Trends & Historical Data
**URL:** `/trends/{memberId}`
**User Story:** 2.3 - Trend Charts

**Layout:**
```
┌────────────────────────────────────────────────────────┐
│ Dashboard > Dad > Trends                  [Export ▼]   │
│                                                        │
│ Dad's Health Trends                                    │
│                                                        │
│ [7 Days] [30 Days] [90 Days] [Custom Range]          │
│                                                        │
│ [Activity] [Heart Rate] [Sleep] [All Metrics]         │
│                                                        │
│ ┌──────────────────────────────────────────────────┐ │
│ │ Activity Trends - Last 30 Days                   │ │
│ │                                                  │ │
│ │ 8000 ┐                                          │ │
│ │      │     ╱╲    ╱╲                             │ │
│ │ 6000 ┤────█──█──█──█─────[Baseline Range]──    │ │
│ │      │   ╱    ╲╱    ╲   🔴🔴                   │ │
│ │ 4000 ┤──█            █──                        │ │
│ │      │ ╱              ╲                         │ │
│ │ 2000 ┤█                ▼                        │ │
│ │      └────────────────────────────────────      │ │
│ │       Jan 1    Jan 15    Jan 30                 │ │
│ │                                                  │ │
│ │ Legend: ─ Actual  ░░░ Normal Range  🔴 Alert   │ │
│ │                                                  │ │
│ │ Hover for details                               │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ Timeline Annotations                                   │
│ ┌──────────────────────────────────────────────────┐ │
│ │ Jan 8  🔴 Low activity alert                     │ │
│ │ Jan 15 📝 "Dad had a cold" - Sarah               │ │
│ │ Jan 22 ✅ Activity improving                     │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ Summary Statistics                                     │
│ ┌─────────────────┬─────────────────┬──────────────┐ │
│ │ Average:        │ High:           │ Low:         │ │
│ │ 4,500 steps     │ 8,200 (Jan 5)   │ 1,200 (Jan 8)│ │
│ │                 │                 │              │ │
│ │ Trend: ↓ Declining 15% over 30 days             │ │
│ └───────────────────────────────────────────────────┘ │
│                                                        │
│ [📄 Export PDF] [📊 Export CSV] [🖼️ Screenshot]     │
└────────────────────────────────────────────────────────┘
```

**Interactive Chart Features:**
- **Hover:** Shows tooltip with exact values
- **Click point:** Shows detail modal
- **Zoom:** Mouse wheel or pinch (touch)
- **Pan:** Click and drag
- **Annotations:** Click to expand notes

**Export Modal:**
```
┌─────────────────────────────────┐
│ Export Health Report            │
│                                 │
│ Date Range:                     │
│ [Jan 1, 2026] to [Jan 30, 2026]│
│                                 │
│ Include:                        │
│ ☑ Summary Statistics            │
│ ☑ Trend Charts                  │
│ ☑ Alert History                 │
│ ☑ Family Notes                  │
│ ☐ Raw Data Table                │
│                                 │
│ Format:                         │
│ ● PDF (Recommended)             │
│ ○ Excel (.xlsx)                 │
│ ○ CSV (data only)               │
│                                 │
│ [Cancel]  [Generate Report →]  │
└─────────────────────────────────┘
```

**Blazor Components:**
```razor
@page "/trends/{MemberId:guid}"
@using Blazor.Charts (or Syncfusion/Radzen)

<PageTitle>Trends - @member.Name - CardiTrack</PageTitle>

<div class="trends-header">
  <h1>@member.Name's Health Trends</h1>
  <ExportButton OnExport="@ShowExportModal" />
</div>

<TimeRangeSelector @bind-Value="@selectedRange" />
<MetricTabs @bind-SelectedMetric="@selectedMetric" />

<div class="chart-container">
  @if (selectedMetric == MetricType.Activity || selectedMetric == MetricType.All)
  {
    <TrendChart
      Title="Activity Trends"
      Data="@activityData"
      BaselineRange="@activityBaseline"
      Annotations="@activityAnnotations"
      OnPointClick="@ShowPointDetails" />
  }

  @if (selectedMetric == MetricType.HeartRate || selectedMetric == MetricType.All)
  {
    <TrendChart
      Title="Heart Rate Trends"
      Data="@heartRateData"
      BaselineRange="@heartRateBaseline"
      Annotations="@heartRateAnnotations"
      OnPointClick="@ShowPointDetails" />
  }

  <!-- Similar for Sleep -->
</div>

<TimelineAnnotations Annotations="@allAnnotations" />

<SummaryStatistics
  Data="@currentMetricData"
  TimeRange="@selectedRange" />

<div class="export-actions">
  <button class="btn-secondary" @onclick="@(() => Export(ExportFormat.PDF))">
    📄 Export PDF
  </button>
  <button class="btn-secondary" @onclick="@(() => Export(ExportFormat.CSV))">
    📊 Export CSV
  </button>
  <button class="btn-secondary" @onclick="@TakeScreenshot">
    🖼️ Screenshot
  </button>
</div>

@code {
  [Parameter] public Guid MemberId { get; set; }

  private CardiMember member;
  private TimeRange selectedRange = TimeRange.Last30Days;
  private MetricType selectedMetric = MetricType.Activity;
  private List<ChartDataPoint> activityData;
  private BaselineRange activityBaseline;

  protected override async Task OnInitializedAsync()
  {
    member = await MemberService.GetByIdAsync(MemberId);
    await LoadChartData();
  }

  private async Task LoadChartData()
  {
    var startDate = selectedRange.GetStartDate();
    var endDate = DateTime.Now;

    activityData = await HealthDataService.GetActivityTrendAsync(
      MemberId, startDate, endDate);
    activityBaseline = await BaselineService.GetActivityBaselineAsync(MemberId);
    activityAnnotations = await AnnotationService.GetAnnotationsAsync(
      MemberId, startDate, endDate);
  }

  private async Task Export(ExportFormat format)
  {
    var reportData = await ReportService.GenerateReportAsync(
      MemberId, selectedRange, exportOptions);

    if (format == ExportFormat.PDF)
    {
      var pdfBytes = await PdfGenerator.GenerateAsync(reportData);
      await JSRuntime.InvokeVoidAsync("downloadFile",
        $"{member.Name}_Health_Report.pdf",
        Convert.ToBase64String(pdfBytes));
    }
  }
}
```

---

## Alert Management

### Screen 3.1: Alerts List
**URL:** `/alerts`
**User Story:** 3.1 - Alert Overview

**Layout:**
```
┌────────────────────────────────────────────────────────┐
│ Dashboard > Alerts                     [Settings ⚙️]   │
│                                                        │
│ Alerts                                [Mark All Read]  │
│                                                        │
│ [All] [Unread] [Critical] [High] [Today] [This Week]  │
│                                                        │
│ TODAY                                                  │
│ ┌──────────────────────────────────────────────────┐ │
│ │ 🚨 CRITICAL                         2 hours ago  │ │
│ │ [Dad Photo] Dad • No Movement Detected           │ │
│ │ Dad hasn't moved this morning. Typical wake time:│ │
│ │ 7am. Current time: 11am                          │ │
│ │ [📞 Call Now] [✓ Acknowledge] [View Details →]  │ │
│ │ Status: 🔴 NEW                                   │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ ┌──────────────────────────────────────────────────┐ │
│ │ ⚡ URGENT                           5 hours ago  │ │
│ │ [Mom Photo] Mom • Elevated Heart Rate            │ │
│ │ Mom's resting HR: 88 bpm. Normal: 68 bpm. Elevated│ │
│ │ for 3 consecutive days.                          │ │
│ │ [📞 Call] [✓ Acknowledge] [View Details →]      │ │
│ │ Status: 🟠 ACKNOWLEDGED by Sarah, 3h ago        │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ YESTERDAY                                              │
│ ┌──────────────────────────────────────────────────┐ │
│ │ ⚠️ INFO                          Yesterday, 2pm  │ │
│ │ [Dad Photo] Dad • Low Activity                   │ │
│ │ Dad's steps: 2,500/day. Normal: 5,000/day (-50%)│ │
│ │ [📞 Call] [✓] [View Details →]                   │ │
│ │ Status: ✅ RESOLVED by you                       │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ [Load More Alerts]                                     │
└────────────────────────────────────────────────────────┘
```

**Blazor Components:**
```razor
@page "/alerts"
@attribute [Authorize]
@implements IDisposable

<PageTitle>Alerts - CardiTrack</PageTitle>

<div class="alerts-header">
  <h1>Alerts</h1>
  <div class="actions">
    <NavLink href="/alerts/settings">
      <button class="btn-icon">⚙️ Settings</button>
    </NavLink>
    @if (unreadCount > 0)
    {
      <button class="btn-secondary" @onclick="@MarkAllAsRead">
        Mark All Read
      </button>
    }
  </div>
</div>

<FilterChips @bind-SelectedFilter="@currentFilter" />

<div class="alerts-list">
  @foreach (var group in groupedAlerts)
  {
    <div class="alert-group">
      <h3 class="group-header">@group.Key</h3>

      @foreach (var alert in group.Value)
      {
        <AlertCard
          Alert="@alert"
          OnCall="@(() => InitiateCall(alert))"
          OnAcknowledge="@(() => AcknowledgeAlert(alert))"
          OnViewDetails="@(() => NavigateToAlertDetail(alert.Id))" />
      }
    </div>
  }
</div>

@if (hasMore)
{
  <button class="btn-load-more" @onclick="@LoadMore">
    Load More Alerts
  </button>
}

@code {
  private List<Alert> alerts = new();
  private Dictionary<string, List<Alert>> groupedAlerts = new();
  private AlertFilter currentFilter = AlertFilter.All;
  private int unreadCount;
  private bool hasMore;
  private HubConnection hubConnection;

  protected override async Task OnInitializedAsync()
  {
    await LoadAlerts();
    await ConnectToSignalR();
  }

  private async Task LoadAlerts()
  {
    alerts = await AlertService.GetAlertsAsync(currentFilter);
    groupedAlerts = alerts
      .GroupBy(a => a.GroupLabel) // "Today", "Yesterday", "This Week"
      .ToDictionary(g => g.Key, g => g.ToList());
    unreadCount = alerts.Count(a => a.Status == AlertStatus.New);
  }

  private async Task ConnectToSignalR()
  {
    hubConnection = new HubConnectionBuilder()
      .WithUrl(Navigation.ToAbsoluteUri("/alertHub"))
      .Build();

    hubConnection.On<Alert>("ReceiveNewAlert", async (newAlert) =>
    {
      // Add to list and show notification
      alerts.Insert(0, newAlert);
      await InvokeAsync(StateHasChanged);
      await JSRuntime.InvokeVoidAsync("showAlertToast", newAlert);
    });

    await hubConnection.StartAsync();
  }

  private async Task AcknowledgeAlert(Alert alert)
  {
    await AlertService.AcknowledgeAsync(alert.Id);
    alert.Status = AlertStatus.Acknowledged;
    await InvokeAsync(StateHasChanged);
  }

  public void Dispose()
  {
    _ = hubConnection?.DisposeAsync();
  }
}
```

---

### Screen 3.2: Alert Detail
**URL:** `/alerts/{alertId}`
**User Stories:** 11.1, 11.2, 11.3 - Specific alerts

**Layout (Activity Alert Example):**
```
┌────────────────────────────────────────────────────────┐
│ Alerts > Alert Details                   [Share] [×]   │
│                                                        │
│ ┌──────────────────────────────────────────────────┐ │
│ │ ⚠️ LOW ACTIVITY ALERT                            │ │
│ │ [Dad Photo] Dad, 78 • January 10, 2026, 11:30 AM │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ Description                                            │
│ ┌──────────────────────────────────────────────────┐ │
│ │ 💡 Dad's activity has been lower than usual      │ │
│ │                                                  │ │
│ │ His recent average is significantly below his    │ │
│ │ normal baseline for the past 2 weeks.            │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ Activity Trend (Last 14 Days)                          │
│ ┌──────────────────────────────────────────────────┐ │
│ │ 6000 ┐                                          │ │
│ │      │ ████─────                                 │ │
│ │ 4000 ┤─────────██                               │ │
│ │      │          ╲╲                              │ │
│ │ 2000 ┤            ██──🔴                        │ │
│ │      └──────────────────────                    │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ Comparison                                             │
│ ┌──────────────────────────────────────────────────┐ │
│ │ Recent Average        Normal Average   Difference│ │
│ │ 2,500 steps/day  ↔   5,000 steps/day   -50% ↓  │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ Context                                                │
│ ┌──────────────────────────────────────────────────┐ │
│ │ 💡 This could indicate:                          │ │
│ │  • Illness or fatigue                            │ │
│ │  • Pain or discomfort                            │ │
│ │  • Low mood or depression                        │ │
│ │  • Changes in routine                            │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ Recommended Actions                                    │
│ [📞 Call to Check In]  [💬 Send Message]             │
│ [📅 Schedule Doctor Visit]                            │
│                                                        │
│ ▼ More Options                                         │
│   • Adjust Baseline (if this is new normal)           │
│   • Add Note About This Alert                         │
│   • Share with Family Members                         │
│                                                        │
│ ┌──────────────────────────────────────────────────┐ │
│ │ Status: ✅ Acknowledged                          │ │
│ │ By: Sarah Johnson, 30 minutes ago                │ │
│ │                                                  │ │
│ │ Notes:                                           │ │
│ │ "Called Dad, he mentioned he had a cold this    │ │
│ │  week and has been resting more. Will monitor." │ │
│ │                                                  │ │
│ │ [Mark as Resolved]                               │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ [View Detailed Activity Data →]                       │
└────────────────────────────────────────────────────────┘
```

**Blazor Component:**
```razor
@page "/alerts/{AlertId:guid}"
@attribute [Authorize]

<PageTitle>Alert Details - CardiTrack</PageTitle>

<div class="alert-detail">
  <AlertHeader
    Alert="@alert"
    OnShare="@ShareAlert"
    OnClose="@NavigateBack" />

  <DescriptionSection Description="@alert.Description" />

  @if (alert.Type == AlertType.Activity)
  {
    <TrendChart
      Title="Activity Trend (Last 14 Days)"
      Data="@activityTrendData"
      ShowBaseline="true" />

    <ComparisonCard
      Current="@alert.CurrentValue"
      Normal="@alert.BaselineValue"
      Difference="@alert.DifferencePercentage" />
  }
  else if (alert.Type == AlertType.HeartRate)
  {
    <HeartRateAlertDetails Alert="@alert" />
  }
  else if (alert.Type == AlertType.PatternBreak)
  {
    <PatternBreakAlertDetails Alert="@alert" />
  }

  <ContextSection
    PossibleCauses="@alert.PossibleCauses" />

  <RecommendedActions>
    <button class="btn-primary" @onclick="@(() => InitiateCall(alert.CardiMemberId))">
      📞 Call to Check In
    </button>
    <button class="btn-secondary" @onclick="@(() => SendMessage(alert.CardiMemberId))">
      💬 Send Message
    </button>
    <button class="btn-secondary" @onclick="@ScheduleDoctorVisit">
      📅 Schedule Doctor Visit
    </button>
  </RecommendedActions>

  <Collapsible Title="More Options">
    <button class="btn-text" @onclick="@AdjustBaseline">
      Adjust Baseline (if this is new normal)
    </button>
    <button class="btn-text" @onclick="@ShowNoteModal">
      Add Note About This Alert
    </button>
    <button class="btn-text" @onclick="@ShareWithFamily">
      Share with Family Members
    </button>
  </Collapsible>

  @if (alert.Status != AlertStatus.New)
  {
    <AcknowledgmentSection
      Alert="@alert"
      OnResolve="@ResolveAlert" />
  }
  else
  {
    <AcknowledgeAlertForm
      AlertId="@alert.Id"
      OnAcknowledge="@HandleAcknowledge" />
  }

  <NavLink href="@($"/trends/{alert.CardiMemberId}")">
    <button class="btn-link">View Detailed Activity Data →</button>
  </NavLink>
</div>

@code {
  [Parameter] public Guid AlertId { get; set; }

  private Alert alert;
  private List<ChartDataPoint> activityTrendData;

  protected override async Task OnInitializedAsync()
  {
    alert = await AlertService.GetAlertByIdAsync(AlertId);

    if (alert.Type == AlertType.Activity)
    {
      activityTrendData = await HealthDataService.GetActivityTrendAsync(
        alert.CardiMemberId,
        DateTime.Now.AddDays(-14),
        DateTime.Now);
    }
  }

  private async Task HandleAcknowledge(AcknowledgmentData data)
  {
    await AlertService.AcknowledgeAsync(AlertId, data);
    alert.Status = AlertStatus.Acknowledged;
    await InvokeAsync(StateHasChanged);
  }

  private async Task ResolveAlert()
  {
    await AlertService.ResolveAsync(AlertId);
    Navigation.NavigateTo("/alerts");
  }
}
```

---

### Screen 3.3: Alert Settings
**URL:** `/alerts/settings`
**User Story:** 3.2 - Notification Preferences

**Layout:**
```
┌────────────────────────────────────────────────────────┐
│ Alerts > Settings                                      │
│                                                        │
│ Notification Preferences                               │
│                                                        │
│ Settings for: [Dad ▼]                                 │
│                                                        │
│ Alert Types                                            │
│ ┌──────────────────────────────────────────────────┐ │
│ │ Activity Alerts                            [ON]   │ │
│ │ ▼ Alert when activity is below normal             │ │
│ │                                                  │ │
│ │   Sensitivity: Low ───●─── Medium ────── High   │ │
│ │   Threshold: 30% below baseline                 │ │
│ │                                                  │ │
│ │   Notify via: [✓Email] [✓SMS] [✓Push] [✓All]   │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ ┌──────────────────────────────────────────────────┐ │
│ │ Heart Rate Alerts                          [ON]   │ │
│ │ ▼ Alert when heart rate exceeds baseline          │ │
│ │                                                  │ │
│ │   Sensitivity: Low ────── Medium ─●── High       │ │
│ │   Threshold: 20% above baseline                  │ │
│ │                                                  │ │
│ │   Notify via: [✓Email] [✓SMS] [✓Push] [✓All]   │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ ┌──────────────────────────────────────────────────┐ │
│ │ Sleep Alerts                               [ON]   │ │
│ │ ▼ Alert for unusual sleep patterns                │ │
│ │                                                  │ │
│ │   ☑ Poor sleep quality                           │ │
│ │   ☑ Significantly less sleep than usual          │ │
│ │   ☑ Irregular sleep schedule                     │ │
│ │                                                  │ │
│ │   Notify via: [✓Email] [ ]SMS] [✓Push]          │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ ┌──────────────────────────────────────────────────┐ │
│ │ Pattern Break Alerts            [ON] (Required)  │ │
│ │ Critical safety alerts for emergency detection   │ │
│ │                                                  │ │
│ │   Notify via: [✓Email] [✓SMS] [✓Push] [✓All]   │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ Quiet Hours                                            │
│ ┌──────────────────────────────────────────────────┐ │
│ │ Enable Quiet Hours                         [ON]   │ │
│ │                                                  │ │
│ │ Don't send notifications during:                 │ │
│ │ From: [10:00 PM] To: [7:00 AM]                  │ │
│ │                                                  │ │
│ │ ☑ Still alert for Critical (Red) events          │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ Family Notification Routing                            │
│ ┌──────────────────────────────────────────────────┐ │
│ │ Also notify these family members:                │ │
│ │                                                  │ │
│ │ ☑ Sarah Johnson                                  │ │
│ │   Notify for: [✓High Severity] [✓Critical]      │ │
│ │                                                  │ │
│ │ ☑ John Doe                                       │ │
│ │   Notify for: [ ]High Severity] [✓Critical Only]│ │
│ │                                                  │ │
│ │ ☐ Mary Smith (Viewer - cannot acknowledge)       │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ Test Notifications                                     │
│ [Send Test Push] [Send Test Email] [Send Test SMS]   │
│                                                        │
│ [Cancel]                                    [Save]    │
└────────────────────────────────────────────────────────┘
```

**Blazor Components:**
```razor
@page "/alerts/settings"
@attribute [Authorize]

<PageTitle>Alert Settings - CardiTrack</PageTitle>

<div class="alert-settings">
  <h1>Notification Preferences</h1>

  @if (cardiMembers.Count > 1)
  {
    <MemberSelector @bind-Value="@selectedMemberId" Members="@cardiMembers" />
  }

  <EditForm Model="@settings" OnValidSubmit="@SaveSettings">
    <h2>Alert Types</h2>

    <AlertTypeSettings
      Label="Activity Alerts"
      @bind-Enabled="@settings.ActivityAlertsEnabled"
      @bind-Sensitivity="@settings.ActivitySensitivity"
      @bind-Threshold="@settings.ActivityThreshold"
      @bind-Channels="@settings.ActivityChannels" />

    <AlertTypeSettings
      Label="Heart Rate Alerts"
      @bind-Enabled="@settings.HeartRateAlertsEnabled"
      @bind-Sensitivity="@settings.HeartRateSensitivity"
      @bind-Threshold="@settings.HeartRateThreshold"
      @bind-Channels="@settings.HeartRateChannels" />

    <AlertTypeSettings
      Label="Sleep Alerts"
      @bind-Enabled="@settings.SleepAlertsEnabled"
      Options="@sleepAlertOptions"
      @bind-Channels="@settings.SleepChannels" />

    <AlertTypeSettings
      Label="Pattern Break Alerts"
      Enabled="true"
      Disabled="true"
      RequiredNote="Required for emergency detection"
      @bind-Channels="@settings.PatternBreakChannels" />

    <h2>Quiet Hours</h2>
    <QuietHoursSettings
      @bind-Enabled="@settings.QuietHoursEnabled"
      @bind-StartTime="@settings.QuietHoursStart"
      @bind-EndTime="@settings.QuietHoursEnd"
      @bind-AllowCritical="@settings.AllowCriticalDuringQuietHours" />

    <h2>Family Notification Routing</h2>
    <FamilyNotificationSettings
      FamilyMembers="@familyMembers"
      @bind-Routing="@settings.FamilyRouting" />

    <h2>Test Notifications</h2>
    <div class="test-buttons">
      <button type="button" class="btn-secondary" @onclick="@SendTestPush">
        Send Test Push
      </button>
      <button type="button" class="btn-secondary" @onclick="@SendTestEmail">
        Send Test Email
      </button>
      <button type="button" class="btn-secondary" @onclick="@SendTestSMS">
        Send Test SMS
      </button>
    </div>

    <div class="form-actions">
      <button type="button" class="btn-text" @onclick="@Cancel">Cancel</button>
      <button type="submit" class="btn-primary">Save Settings</button>
    </div>
  </EditForm>
</div>

@code {
  private List<CardiMember> cardiMembers = new();
  private List<FamilyMember> familyMembers = new();
  private Guid selectedMemberId;
  private AlertSettings settings = new();

  protected override async Task OnInitializedAsync()
  {
    cardiMembers = await MemberService.GetAllMembersAsync();
    selectedMemberId = cardiMembers.FirstOrDefault()?.Id ?? Guid.Empty;
    familyMembers = await FamilyService.GetFamilyMembersAsync();
    await LoadSettings();
  }

  private async Task LoadSettings()
  {
    settings = await AlertSettingsService.GetSettingsAsync(selectedMemberId);
  }

  private async Task SaveSettings()
  {
    await AlertSettingsService.SaveSettingsAsync(selectedMemberId, settings);
    await JSRuntime.InvokeVoidAsync("showToast", "Settings saved successfully!");
  }

  private async Task SendTestPush()
  {
    await NotificationService.SendTestNotificationAsync(NotificationChannel.Push);
    await JSRuntime.InvokeVoidAsync("showToast", "Test push notification sent!");
  }
}
```

---

---

## CardiMember Management

### Screen 4.1: CardiMembers List
**URL:** `/cardimembers`
**User Story:** Multi-member management

**Layout:**
```
┌────────────────────────────────────────────────────────┐
│ Dashboard > CardiMembers                               │
│                                                        │
│ Your CardiMembers                          [+ Add New] │
│                                                        │
│ ┌──────────────────────────────────────────────────┐ │
│ │ [Photo]  Dad, 78                    Status: 🟢   │ │
│ │          Parent                                   │ │
│ │                                                  │ │
│ │ Monitoring since: Jan 1, 2026                    │ │
│ │ Devices: 2 connected                             │ │
│ │ Baseline: Established (30 days)                  │ │
│ │                                                  │ │
│ │ [📊 Dashboard] [⚙️ Settings] [👁️ View Profile]  │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ ┌──────────────────────────────────────────────────┐ │
│ │ [Photo]  Mom, 75                    Status: 🟡   │ │
│ │          Parent                      2 alerts    │ │
│ │                                                  │ │
│ │ Monitoring since: Jan 5, 2026                    │ │
│ │ Devices: 1 connected                             │ │
│ │ Baseline: Learning (Day 20 of 30)                │ │
│ │                                                  │ │
│ │ [📊 Dashboard] [⚙️ Settings] [👁️ View Profile]  │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ ┌──────────────────────────────────────────────────┐ │
│ │ Empty State (if no members)                      │ │
│ │                                                  │ │
│ │      [Icon: User with heart]                     │ │
│ │                                                  │ │
│ │      Start Monitoring a Loved One                │ │
│ │                                                  │ │
│ │      Add a CardiMember to begin monitoring       │ │
│ │      their health and receive AI-powered alerts. │ │
│ │                                                  │ │
│ │      [+ Add Your First CardiMember]              │ │
│ └──────────────────────────────────────────────────┘ │
└────────────────────────────────────────────────────────┘
```

**Blazor Components:**
```razor
@page "/cardimembers"
@attribute [Authorize]

<PageTitle>CardiMembers - CardiTrack</PageTitle>

<div class="page-header">
  <h1>Your CardiMembers</h1>
  <NavLink href="/cardimembers/add">
    <button class="btn-primary">+ Add New</button>
  </NavLink>
</div>

<div class="cardimember-list">
  @if (cardiMembers.Any())
  {
    @foreach (var member in cardiMembers)
    {
      <CardiMemberListCard
        Member="@member"
        Status="@GetMemberStatus(member.Id)"
        OnViewDashboard="@(() => NavigateToDashboard(member.Id))"
        OnViewProfile="@(() => NavigateToProfile(member.Id))"
        OnSettings="@(() => NavigateToSettings(member.Id))" />
    }
  }
  else
  {
    <EmptyState
      Icon="user-heart"
      Title="Start Monitoring a Loved One"
      Message="Add a CardiMember to begin monitoring their health and receive AI-powered alerts."
      ActionText="+ Add Your First CardiMember"
      OnAction="@NavigateToAddMember" />
  }
</div>

@code {
  private List<CardiMember> cardiMembers = new();

  protected override async Task OnInitializedAsync()
  {
    cardiMembers = await MemberService.GetAllMembersAsync();
  }

  private HealthStatus GetMemberStatus(Guid memberId)
  {
    return StatusCache.GetOrAdd(memberId,
      async id => await MemberService.GetCurrentStatusAsync(id));
  }
}
```

---

### Screen 4.2: CardiMember Profile
**URL:** `/cardimembers/{memberId}/profile`
**User Story:** View/Edit CardiMember details

**Layout:**
```
┌────────────────────────────────────────────────────────┐
│ CardiMembers > Dad > Profile               [Edit] [⋮] │
│                                                        │
│ ┌──────────────────────────────────────────────────┐ │
│ │            [Large Profile Photo]                  │ │
│ │                                                  │ │
│ │                 Dad Smith                        │ │
│ │              78 years old • Parent                │ │
│ │                                                  │ │
│ │              Status: All Good 🟢                 │ │
│ │         Last synced: 10 minutes ago              │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ Personal Information                                   │
│ ┌──────────────────────────────────────────────────┐ │
│ │ Full Name:        David Smith                    │ │
│ │ Date of Birth:    March 15, 1948                 │ │
│ │ Age:              78 years                       │ │
│ │ Relationship:     Parent                         │ │
│ │ Added by:         Sarah Johnson                  │ │
│ │ Monitoring since: January 1, 2026                │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ Emergency Contact                                      │
│ ┌──────────────────────────────────────────────────┐ │
│ │ Name:             Mary Smith                     │ │
│ │ Relationship:     Spouse                         │ │
│ │ Phone:            📞 (555) 123-4567              │ │
│ │ Email:            mary.smith@email.com           │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ Medical Information (Encrypted) 🔒                    │
│ ┌──────────────────────────────────────────────────┐ │
│ │ [Expand to view]                                 │ │
│ │ (Requires biometric authentication on mobile)    │ │
│ │                                                  │ │
│ │ Notes:                                           │ │
│ │ "History of high blood pressure. Takes Lisinopril│ │
│ │  daily. Mild arthritis in knees."                │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ Monitoring Status                                      │
│ ┌──────────────────────────────────────────────────┐ │
│ │ Connected Devices:    2 devices                  │ │
│ │ • Fitbit Charge 5 (Primary) ✓                   │ │
│ │ • Apple Watch Series 7                           │ │
│ │                                                  │ │
│ │ Baseline Status:      Established                │ │
│ │ Learning completed:   30 days ago                │ │
│ │                                                  │ │
│ │ Alert Settings:       Custom (Modified)          │ │
│ │ Sensitivity:          Medium                     │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ Family Access                                          │
│ ┌──────────────────────────────────────────────────┐ │
│ │ 3 family members can view this CardiMember:      │ │
│ │ • Sarah Johnson (Admin)                          │ │
│ │ • John Doe (Staff)                               │ │
│ │ • Mary Smith (Viewer)                            │ │
│ │                                                  │ │
│ │ [Manage Family Access]                           │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ Actions                                                │
│ [📊 View Dashboard] [📈 View Trends] [⚙️ Settings]   │
│                                                        │
│ Danger Zone                                            │
│ [⏸️ Pause Monitoring] [🗑️ Remove CardiMember]        │
└────────────────────────────────────────────────────────┘
```

**Blazor Components:**
```razor
@page "/cardimembers/{MemberId:guid}/profile"
@attribute [Authorize]

<PageTitle>@member.Name - Profile - CardiTrack</PageTitle>

<div class="profile-header">
  <h1>@member.Name's Profile</h1>
  <div class="actions">
    <NavLink href="@($"/cardimembers/{MemberId}/edit")">
      <button class="btn-secondary">Edit</button>
    </NavLink>
    <DropdownMenu Icon="more-vertical">
      <DropdownItem OnClick="@ExportProfile">Export Profile</DropdownItem>
      <DropdownItem OnClick="@ViewActivityLog">View Activity Log</DropdownItem>
      <DropdownItem OnClick="@ShareProfile">Share Profile</DropdownItem>
    </DropdownMenu>
  </div>
</div>

<ProfileHeroCard
  Member="@member"
  Status="@currentStatus"
  LastSynced="@lastSyncTime" />

<InfoSection Title="Personal Information">
  <InfoRow Label="Full Name" Value="@member.FullName" />
  <InfoRow Label="Date of Birth" Value="@member.DateOfBirth.ToString("MMMM dd, yyyy")" />
  <InfoRow Label="Age" Value="@member.Age years" />
  <InfoRow Label="Relationship" Value="@member.Relationship" />
  <InfoRow Label="Added by" Value="@addedBy.FullName" />
  <InfoRow Label="Monitoring since" Value="@member.CreatedAt.ToString("MMMM dd, yyyy")" />
</InfoSection>

@if (member.EmergencyContact != null)
{
  <InfoSection Title="Emergency Contact">
    <InfoRow Label="Name" Value="@member.EmergencyContact.Name" />
    <InfoRow Label="Relationship" Value="@member.EmergencyContact.Relationship" />
    <InfoRow Label="Phone">
      <a href="tel:@member.EmergencyContact.Phone">
        📞 @member.EmergencyContact.Phone
      </a>
    </InfoRow>
    <InfoRow Label="Email" Value="@member.EmergencyContact.Email" />
  </InfoSection>
}

<EncryptedSection
  Title="Medical Information (Encrypted) 🔒"
  Content="@member.MedicalNotes"
  RequireAuth="@true" />

<InfoSection Title="Monitoring Status">
  <div class="monitoring-info">
    <h4>Connected Devices: @connectedDevices.Count devices</h4>
    <ul>
      @foreach (var device in connectedDevices)
      {
        <li>
          @device.DeviceName @(device.IsPrimary ? "(Primary)" : "")
          @(device.IsConnected ? "✓" : "⚠️")
        </li>
      }
    </ul>

    <InfoRow Label="Baseline Status" Value="@baselineStatus" />
    <InfoRow Label="Learning completed" Value="@baselineCompletedDate" />
    <InfoRow Label="Alert Settings" Value="@alertSettings" />
    <InfoRow Label="Sensitivity" Value="@sensitivity" />
  </div>
</InfoSection>

<InfoSection Title="Family Access">
  <p>@familyMembers.Count family members can view this CardiMember:</p>
  <ul>
    @foreach (var family in familyMembers)
    {
      <li>@family.Name (@family.Role)</li>
    }
  </ul>
  <NavLink href="@($"/family?member={MemberId}")">
    <button class="btn-secondary">Manage Family Access</button>
  </NavLink>
</InfoSection>

<div class="action-buttons">
  <NavLink href="@($"/dashboard?member={MemberId}")">
    <button class="btn-primary">📊 View Dashboard</button>
  </NavLink>
  <NavLink href="@($"/trends/{MemberId}")">
    <button class="btn-secondary">📈 View Trends</button>
  </NavLink>
  <NavLink href="@($"/cardimembers/{MemberId}/settings")">
    <button class="btn-secondary">⚙️ Settings</button>
  </NavLink>
</div>

<DangerZone>
  <button class="btn-warning" @onclick="@ShowPauseMonitoringModal">
    ⏸️ Pause Monitoring
  </button>
  <button class="btn-danger" @onclick="@ShowRemoveMemberModal">
    🗑️ Remove CardiMember
  </button>
</DangerZone>

@code {
  [Parameter] public Guid MemberId { get; set; }

  private CardiMember member;
  private HealthStatus currentStatus;
  private DateTime lastSyncTime;
  private List<ConnectedDevice> connectedDevices;
  private List<FamilyMember> familyMembers;

  protected override async Task OnInitializedAsync()
  {
    member = await MemberService.GetByIdAsync(MemberId);
    currentStatus = await MemberService.GetCurrentStatusAsync(MemberId);
    connectedDevices = await DeviceService.GetDevicesForMemberAsync(MemberId);
    familyMembers = await FamilyService.GetMembersWithAccessAsync(MemberId);
  }
}
```

---

### Screen 4.3: Edit CardiMember
**URL:** `/cardimembers/{memberId}/edit`
**User Story:** Update CardiMember information

**Layout:**
```
┌────────────────────────────────────────────────────────┐
│ CardiMembers > Dad > Edit Profile      [Cancel] [Save] │
│                                                        │
│ Edit Dad's Profile                                     │
│                                                        │
│ ┌──────────────────────────────────────────────────┐ │
│ │         [Current Photo]                          │ │
│ │                                                  │ │
│ │         [Change Photo] [Remove]                  │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ Personal Information                                   │
│ ┌──────────────────────────────────────────────────┐ │
│ │ Full Name *                                      │ │
│ │ [David Smith__________________________]         │ │
│ │                                                  │ │
│ │ Date of Birth *                                  │ │
│ │ [03] / [15] / [1948]                            │ │
│ │                                                  │ │
│ │ Relationship *                                   │ │
│ │ [▼ Parent                              ]         │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ Emergency Contact                                      │
│ ┌──────────────────────────────────────────────────┐ │
│ │ Contact Name                                     │ │
│ │ [Mary Smith__________________________]          │ │
│ │                                                  │ │
│ │ Relationship                                     │ │
│ │ [Spouse_____________________________]          │ │
│ │                                                  │ │
│ │ Phone Number                                     │ │
│ │ [(555) 123-4567____________________]           │ │
│ │                                                  │ │
│ │ Email Address                                    │ │
│ │ [mary.smith@email.com______________]           │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ Medical Information (Encrypted) 🔒                    │
│ ┌──────────────────────────────────────────────────┐ │
│ │ Medical Notes                                    │ │
│ │ ┌────────────────────────────────────────────┐  │ │
│ │ │ History of high blood pressure.            │  │ │
│ │ │ Takes Lisinopril daily.                    │  │ │
│ │ │ Mild arthritis in knees.                   │  │ │
│ │ │                                            │  │ │
│ │ │                                            │  │ │
│ │ └────────────────────────────────────────────┘  │ │
│ │ 450 / 500 characters                             │ │
│ │                                                  │ │
│ │ ℹ️ This information is encrypted end-to-end     │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ Monitoring Preferences                                 │
│ ┌──────────────────────────────────────────────────┐ │
│ │ Alert Sensitivity                                │ │
│ │ ○ Low    ● Medium    ○ High                     │ │
│ │                                                  │ │
│ │ Preferred Contact Method                         │ │
│ │ [▼ Phone Call                          ]         │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ * Required fields                                      │
│                                                        │
│ [Cancel]                           [Save Changes]     │
└────────────────────────────────────────────────────────┘
```

**Blazor Components:**
```razor
@page "/cardimembers/{MemberId:guid}/edit"
@attribute [Authorize]

<PageTitle>Edit @member.Name - CardiTrack</PageTitle>

<div class="page-header">
  <h1>Edit @member.Name's Profile</h1>
  <div class="actions">
    <button class="btn-text" @onclick="@Cancel">Cancel</button>
    <button class="btn-primary" @onclick="@SaveChanges" disabled="@(!isValid)">
      Save
    </button>
  </div>
</div>

<EditForm Model="@editModel" OnValidSubmit="@HandleValidSubmit">
  <DataAnnotationsValidator />
  <ValidationSummary />

  <SectionCard Title="Photo">
    <div class="photo-upload">
      <img src="@GetPhotoUrl()" alt="@member.Name" class="profile-photo-large" />
      <div class="photo-actions">
        <InputFile OnChange="@HandlePhotoUpload" accept="image/*">
          <button class="btn-secondary">Change Photo</button>
        </InputFile>
        @if (!string.IsNullOrEmpty(member.PhotoUrl))
        {
          <button class="btn-text" @onclick="@RemovePhoto">Remove</button>
        }
      </div>
    </div>
  </SectionCard>

  <SectionCard Title="Personal Information">
    <FormField Label="Full Name" Required="true">
      <InputText @bind-Value="editModel.FullName" class="form-control" />
    </FormField>

    <FormField Label="Date of Birth" Required="true">
      <InputDate @bind-Value="editModel.DateOfBirth" class="form-control" />
    </FormField>

    <FormField Label="Relationship" Required="true">
      <InputSelect @bind-Value="editModel.Relationship" class="form-control">
        <option value="Parent">Parent</option>
        <option value="Grandparent">Grandparent</option>
        <option value="Spouse">Spouse</option>
        <option value="Sibling">Sibling</option>
        <option value="Other">Other</option>
      </InputSelect>
    </FormField>
  </SectionCard>

  <SectionCard Title="Emergency Contact">
    <FormField Label="Contact Name">
      <InputText @bind-Value="editModel.EmergencyContactName" class="form-control" />
    </FormField>

    <FormField Label="Relationship">
      <InputText @bind-Value="editModel.EmergencyContactRelationship" class="form-control" />
    </FormField>

    <FormField Label="Phone Number">
      <InputText @bind-Value="editModel.EmergencyContactPhone"
                 type="tel" class="form-control" />
    </FormField>

    <FormField Label="Email Address">
      <InputText @bind-Value="editModel.EmergencyContactEmail"
                 type="email" class="form-control" />
    </FormField>
  </SectionCard>

  <SectionCard Title="Medical Information (Encrypted) 🔒">
    <FormField Label="Medical Notes">
      <InputTextArea @bind-Value="editModel.MedicalNotes"
                     rows="6" maxlength="500" class="form-control" />
      <span class="character-counter">
        @(editModel.MedicalNotes?.Length ?? 0) / 500 characters
      </span>
    </FormField>
    <p class="info-text">
      ℹ️ This information is encrypted end-to-end
    </p>
  </SectionCard>

  <SectionCard Title="Monitoring Preferences">
    <FormField Label="Alert Sensitivity">
      <div class="radio-group">
        <label>
          <InputRadio @bind-Value="editModel.AlertSensitivity" Value="Sensitivity.Low" />
          Low
        </label>
        <label>
          <InputRadio @bind-Value="editModel.AlertSensitivity" Value="Sensitivity.Medium" />
          Medium
        </label>
        <label>
          <InputRadio @bind-Value="editModel.AlertSensitivity" Value="Sensitivity.High" />
          High
        </label>
      </div>
    </FormField>

    <FormField Label="Preferred Contact Method">
      <InputSelect @bind-Value="editModel.PreferredContactMethod" class="form-control">
        <option value="PhoneCall">Phone Call</option>
        <option value="SMS">SMS/Text</option>
        <option value="Email">Email</option>
      </InputSelect>
    </FormField>
  </SectionCard>

  <div class="form-actions">
    <button type="button" class="btn-text" @onclick="@Cancel">Cancel</button>
    <button type="submit" class="btn-primary">Save Changes</button>
  </div>
</EditForm>

@code {
  [Parameter] public Guid MemberId { get; set; }

  private CardiMember member;
  private CardiMemberEditModel editModel = new();
  private bool isValid = true;

  protected override async Task OnInitializedAsync()
  {
    member = await MemberService.GetByIdAsync(MemberId);
    editModel = MapToEditModel(member);
  }

  private async Task HandleValidSubmit()
  {
    var updatedMember = MapFromEditModel(editModel);
    await MemberService.UpdateAsync(MemberId, updatedMember);
    await JSRuntime.InvokeVoidAsync("showToast", "Profile updated successfully!");
    Navigation.NavigateTo($"/cardimembers/{MemberId}/profile");
  }

  private async Task HandlePhotoUpload(InputFileChangeEventArgs e)
  {
    var file = e.File;
    if (file.Size > 5 * 1024 * 1024) // 5MB limit
    {
      await JSRuntime.InvokeVoidAsync("showToast", "File too large. Max 5MB.");
      return;
    }

    var imageUrl = await PhotoService.UploadAsync(file);
    editModel.PhotoUrl = imageUrl;
  }

  private void Cancel()
  {
    Navigation.NavigateTo($"/cardimembers/{MemberId}/profile");
  }
}
```

---

## Family Collaboration

### Screen 5.1: Family Members Management
**URL:** `/family`
**User Story:** 4.1 - Family Management

**Layout:**
```
┌────────────────────────────────────────────────────────┐
│ Dashboard > Family & Sharing               [+ Invite]  │
│                                                        │
│ Family Members                                         │
│                                                        │
│ Tabs: [Active Members] [Pending Invitations]          │
│                                                        │
│ ACTIVE MEMBERS (3)                                     │
│ ┌──────────────────────────────────────────────────┐ │
│ │ [Photo] Sarah Johnson (You)             ADMIN    │ │
│ │         sarah@email.com                          │ │
│ │         Last active: Just now                    │ │
│ │                                                  │ │
│ │         Access: All CardiMembers                 │ │
│ │         • Dad, Mom                               │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ ┌──────────────────────────────────────────────────┐ │
│ │ [Photo] John Doe                         STAFF   │ │
│ │         john@email.com                    [⋮]    │ │
│ │         Last active: 2 hours ago                 │ │
│ │                                                  │ │
│ │         Access: Dad only                         │ │
│ │         Can acknowledge alerts                   │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ ┌──────────────────────────────────────────────────┐ │
│ │ [Photo] Mary Smith                      VIEWER   │ │
│ │         mary@email.com                    [⋮]    │ │
│ │         Last active: Yesterday                   │ │
│ │                                                  │ │
│ │         Access: All CardiMembers (View only)     │ │
│ │         Cannot modify settings                   │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ Permission Matrix                  [View Full Matrix] │
│ ┌──────────────────────────────────────────────────┐ │
│ │              Admin    Staff    Viewer            │ │
│ │ View data      ✓        ✓        ✓              │ │
│ │ Acknowledge    ✓        ✓        ✗              │ │
│ │ Add members    ✓        ✗        ✗              │ │
│ │ Invite family  ✓        ✗        ✗              │ │
│ │ Billing        ✓        ✗        ✗              │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ [View Activity Log] (HIPAA Compliance)                │
└────────────────────────────────────────────────────────┘
```

**Pending Invitations Tab:**
```
┌────────────────────────────────────────────────────────┐
│ PENDING INVITATIONS (2)                                │
│                                                        │
│ ┌──────────────────────────────────────────────────┐ │
│ │ 📧 jane@email.com                        VIEWER  │ │
│ │    Sent: 2 days ago                              │ │
│ │    Access: All CardiMembers (View only)          │ │
│ │                                                  │ │
│ │    [Resend Invitation] [Revoke]                  │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ ┌──────────────────────────────────────────────────┐ │
│ │ 📧 bob@email.com                          STAFF  │ │
│ │    Sent: 5 days ago                              │ │
│ │    Access: Mom only                              │ │
│ │                                                  │ │
│ │    [Resend Invitation] [Revoke]                  │ │
│ └──────────────────────────────────────────────────┘ │
└────────────────────────────────────────────────────────┘
```

**Blazor Components:**
```razor
@page "/family"
@attribute [Authorize(Roles = "Admin,Staff")]

<PageTitle>Family & Sharing - CardiTrack</PageTitle>

<div class="page-header">
  <h1>Family Members</h1>
  <button class="btn-primary" @onclick="@ShowInviteModal">+ Invite</button>
</div>

<Tabs @bind-ActiveTab="@activeTab">
  <Tab Name="active" Title="Active Members">
    <div class="members-list">
      <h3>ACTIVE MEMBERS (@activeMembers.Count)</h3>

      @foreach (var member in activeMembers)
      {
        <FamilyMemberCard
          Member="@member"
          IsCurrentUser="@(member.Id == currentUserId)"
          OnChangeRole="@(() => ShowChangeRoleModal(member))"
          OnRemove="@(() => ShowRemoveMemberModal(member))"
          OnViewActivity="@(() => ShowActivityLog(member))" />
      }
    </div>

    <PermissionMatrixSummary OnViewFull="@ShowPermissionMatrix" />

    <button class="btn-secondary" @onclick="@ShowActivityLog">
      View Activity Log (HIPAA Compliance)
    </button>
  </Tab>

  <Tab Name="pending" Title="Pending Invitations">
    <div class="invitations-list">
      <h3>PENDING INVITATIONS (@pendingInvitations.Count)</h3>

      @if (pendingInvitations.Any())
      {
        @foreach (var invitation in pendingInvitations)
        {
          <InvitationCard
            Invitation="@invitation"
            OnResend="@(() => ResendInvitation(invitation.Id))"
            OnRevoke="@(() => RevokeInvitation(invitation.Id))" />
        }
      }
      else
      {
        <p>No pending invitations</p>
      }
    </div>
  </Tab>
</Tabs>

@if (showInviteModal)
{
  <InviteFamilyMemberModal
    CardiMembers="@cardiMembers"
    OnInvite="@HandleInvite"
    OnClose="@(() => showInviteModal = false)" />
}

@code {
  private List<FamilyMember> activeMembers = new();
  private List<Invitation> pendingInvitations = new();
  private List<CardiMember> cardiMembers = new();
  private string activeTab = "active";
  private bool showInviteModal;
  private Guid currentUserId;

  protected override async Task OnInitializedAsync()
  {
    currentUserId = await AuthService.GetCurrentUserIdAsync();
    await LoadData();
  }

  private async Task LoadData()
  {
    activeMembers = await FamilyService.GetActiveMembersAsync();
    pendingInvitations = await InvitationService.GetPendingAsync();
    cardiMembers = await MemberService.GetAllMembersAsync();
  }

  private async Task HandleInvite(InvitationModel model)
  {
    await InvitationService.SendInvitationAsync(model);
    await LoadData();
    showInviteModal = false;
    await JSRuntime.InvokeVoidAsync("showToast", $"Invitation sent to {model.Email}");
  }

  private async Task ResendInvitation(Guid invitationId)
  {
    await InvitationService.ResendAsync(invitationId);
    await JSRuntime.InvokeVoidAsync("showToast", "Invitation resent!");
  }

  private async Task RevokeInvitation(Guid invitationId)
  {
    var confirmed = await JSRuntime.InvokeAsync<bool>("confirm",
      "Are you sure you want to revoke this invitation?");

    if (confirmed)
    {
      await InvitationService.RevokeAsync(invitationId);
      await LoadData();
      await JSRuntime.InvokeVoidAsync("showToast", "Invitation revoked");
    }
  }
}
```

---

### Screen 5.2: Invite Family Member Modal
**User Story:** 4.1 - Inviting Members

**Modal Layout:**
```
┌─────────────────────────────────────┐
│ Invite Family Member            [×] │
│                                     │
│ Email Address *                     │
│ [jane@email.com______________]     │
│                                     │
│ Access Level *                      │
│ ┌─────────┬─────────┬─────────┐   │
│ │  ADMIN  │  STAFF  │ VIEWER  │   │
│ │ (Full)  │(Support)│(Monitor)│   │
│ └─────────┴─────────┴─────────┘   │
│     ○         ○         ●          │
│                                     │
│ Selected: VIEWER                    │
│ • Can view health data              │
│ • Cannot acknowledge alerts         │
│ • Cannot modify settings            │
│                                     │
│ [View Permission Details ▼]         │
│                                     │
│ CardiMember Access                  │
│ Which CardiMembers can they see?    │
│ ● All CardiMembers                  │
│ ○ Specific CardiMembers:            │
│   ☐ Dad                             │
│   ☐ Mom                             │
│                                     │
│ Personal Message (Optional)         │
│ ┌─────────────────────────────┐   │
│ │ Hi Jane,                    │   │
│ │                             │   │
│ │ I'd like to share Dad's     │   │
│ │ health monitoring with you. │   │
│ │                             │   │
│ └─────────────────────────────┘   │
│                                     │
│ [Cancel]     [Send Invitation]     │
└─────────────────────────────────────┘
```

**Blazor Component:**
```razor
<Modal Title="Invite Family Member" OnClose="@OnClose">
  <EditForm Model="@inviteModel" OnValidSubmit="@HandleSubmit">
    <DataAnnotationsValidator />
    <ValidationSummary />

    <FormField Label="Email Address" Required="true">
      <InputText @bind-Value="inviteModel.Email"
                 type="email"
                 placeholder="jane@email.com"
                 class="form-control" />
    </FormField>

    <FormField Label="Access Level" Required="true">
      <div class="role-selector">
        <RoleCard
          Role="Admin"
          Title="ADMIN"
          Subtitle="(Full Control)"
          Selected="@(inviteModel.Role == Role.Admin)"
          OnSelect="@(() => inviteModel.Role = Role.Admin)" />

        <RoleCard
          Role="Staff"
          Title="STAFF"
          Subtitle="(Support)"
          Selected="@(inviteModel.Role == Role.Staff)"
          OnSelect="@(() => inviteModel.Role = Role.Staff)" />

        <RoleCard
          Role="Viewer"
          Title="VIEWER"
          Subtitle="(Monitor Only)"
          Selected="@(inviteModel.Role == Role.Viewer)"
          OnSelect="@(() => inviteModel.Role = Role.Viewer)" />
      </div>

      <RoleDescription Role="@inviteModel.Role" />
    </FormField>

    <Collapsible Title="View Permission Details">
      <PermissionMatrix ShowRole="@inviteModel.Role" />
    </Collapsible>

    <FormField Label="CardiMember Access">
      <p>Which CardiMembers can they see?</p>
      <div class="radio-group">
        <label>
          <InputRadio @bind-Value="inviteModel.AccessType"
                      Value="AccessType.All" />
          All CardiMembers
        </label>
        <label>
          <InputRadio @bind-Value="inviteModel.AccessType"
                      Value="AccessType.Specific" />
          Specific CardiMembers:
        </label>
      </div>

      @if (inviteModel.AccessType == AccessType.Specific)
      {
        <div class="member-checkboxes">
          @foreach (var member in CardiMembers)
          {
            <label>
              <input type="checkbox"
                     checked="@inviteModel.SelectedMemberIds.Contains(member.Id)"
                     @onchange="@(e => ToggleMember(member.Id, e))" />
              @member.Name
            </label>
          }
        </div>
      }
    </FormField>

    <FormField Label="Personal Message (Optional)">
      <InputTextArea @bind-Value="inviteModel.PersonalMessage"
                     rows="4"
                     placeholder="Add a personal message..."
                     class="form-control" />
    </FormField>

    <ModalActions>
      <button type="button" class="btn-text" @onclick="@OnClose">
        Cancel
      </button>
      <button type="submit" class="btn-primary">
        Send Invitation
      </button>
    </ModalActions>
  </EditForm>
</Modal>

@code {
  [Parameter] public List<CardiMember> CardiMembers { get; set; }
  [Parameter] public EventCallback<InvitationModel> OnInvite { get; set; }
  [Parameter] public EventCallback OnClose { get; set; }

  private InvitationModel inviteModel = new() { Role = Role.Viewer };

  private async Task HandleSubmit()
  {
    await OnInvite.InvokeAsync(inviteModel);
  }

  private void ToggleMember(Guid memberId, ChangeEventArgs e)
  {
    var isChecked = (bool)e.Value;
    if (isChecked)
    {
      inviteModel.SelectedMemberIds.Add(memberId);
    }
    else
    {
      inviteModel.SelectedMemberIds.Remove(memberId);
    }
  }
}
```

---

### Screen 5.3: Shared Notes Feed
**URL:** `/family/notes`
**User Story:** 4.2 - Coordination

**Layout:**
```
┌────────────────────────────────────────────────────────┐
│ Dashboard > Family > Notes                  [Filter ▼] │
│                                                        │
│ Family Notes                                           │
│                                                        │
│ ┌──────────────────────────────────────────────────┐ │
│ │ [Your Photo] Add a note for the family...       │ │
│ │ (Click to expand)                                │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ Filter: [All Notes ▼]  Sort: [Recent First ▼]        │
│                                                        │
│ TODAY                                                  │
│ ┌──────────────────────────────────────────────────┐ │
│ │ [Sarah] Sarah Johnson            2 hours ago     │ │
│ │                                           [⋮]     │ │
│ │ Called Dad this morning about his cold. He's    │ │
│ │ feeling better but still resting. Activity low  │ │
│ │ is expected this week.                           │ │
│ │                                                  │ │
│ │ About: 👤 Dad                                    │ │
│ │                                                  │ │
│ │ 💬 2 replies    ❤️ 3 likes                       │ │
│ │                                                  │ │
│ │ ▼ Show replies                                   │ │
│ │   [John] Thanks for the update! - 1h ago        │ │
│ │   [Mary] Feel better, Dad! - 30m ago            │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ ┌──────────────────────────────────────────────────┐ │
│ │ [John] John Doe                  5 hours ago     │ │
│ │                                           [⋮]     │ │
│ │ @Sarah Johnson - Did you see the elevated HR   │ │
│ │ alert for Mom yesterday? Should we call?        │ │
│ │                                                  │ │
│ │ About: 👤 Mom                                    │ │
│ │                                                  │ │
│ │ 💬 1 reply    ❤️ 1 like                          │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ YESTERDAY                                              │
│ ┌──────────────────────────────────────────────────┐ │
│ │ [Mary] Mary Smith                Yesterday, 3pm  │ │
│ │                                           [⋮]     │ │
│ │ Visited Dad today. He showed me his new         │ │
│ │ Fitbit - he's excited about tracking his steps!│ │
│ │                                                  │ │
│ │ [Photo: Dad with Fitbit]                        │ │
│ │                                                  │ │
│ │ About: 👤 Dad                                    │ │
│ │                                                  │ │
│ │ 💬 0 replies    ❤️ 5 likes                       │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ [Load More Notes]                                     │
└────────────────────────────────────────────────────────┘
```

**Add/Edit Note (Expanded):**
```
┌──────────────────────────────────────────────────┐
│ Add a Note                               [×]     │
│                                                  │
│ ┌────────────────────────────────────────────┐  │
│ │ Share an update with the family...        │  │
│ │                                            │  │
│ │ @Sarah (mention autocomplete)             │  │
│ │                                            │  │
│ │                                            │  │
│ └────────────────────────────────────────────┘  │
│ 245 / 500 characters                             │
│                                                  │
│ About (optional):                                │
│ [▼ Select CardiMember             ]              │
│    None (General)                                │
│    Dad                                           │
│    Mom                                           │
│                                                  │
│ Attachments:                                     │
│ [📎 Attach Photo]  [📄 Attach File]             │
│                                                  │
│ Who can see this:                                │
│ ● All family members                             │
│ ○ Specific members (coming soon)                │
│                                                  │
│ [Cancel]                    [Post Note]         │
└──────────────────────────────────────────────────┘
```

**Blazor Components:**
```razor
@page "/family/notes"
@attribute [Authorize]
@implements IAsyncDisposable

<PageTitle>Family Notes - CardiTrack</PageTitle>

<div class="page-header">
  <h1>Family Notes</h1>
  <FilterDropdown @bind-Value="@currentFilter" Options="@filterOptions" />
</div>

<div class="add-note-section">
  <AddNoteQuickInput OnExpand="@ShowAddNoteModal" />
</div>

<div class="notes-filters">
  <FilterDropdown @bind-Value="@currentFilter" Label="Filter" />
  <SortDropdown @bind-Value="@currentSort" Label="Sort" />
</div>

<div class="notes-feed">
  @foreach (var group in groupedNotes)
  {
    <div class="note-group">
      <h3 class="group-header">@group.Key</h3>

      @foreach (var note in group.Value)
      {
        <NoteCard
          Note="@note"
          CurrentUserId="@currentUserId"
          OnReply="@(() => ShowReplyModal(note))"
          OnLike="@(() => LikeNote(note.Id))"
          OnEdit="@(() => ShowEditModal(note))"
          OnDelete="@(() => DeleteNote(note.Id))" />
      }
    </div>
  }
</div>

@if (hasMore)
{
  <button class="btn-load-more" @onclick="@LoadMore">
    Load More Notes
  </button>
}

@if (showAddNoteModal)
{
  <AddNoteModal
    CardiMembers="@cardiMembers"
    OnPost="@HandlePostNote"
    OnClose="@(() => showAddNoteModal = false)" />
}

@code {
  private List<FamilyNote> notes = new();
  private Dictionary<string, List<FamilyNote>> groupedNotes = new();
  private List<CardiMember> cardiMembers = new();
  private Guid currentUserId;
  private bool showAddNoteModal;
  private bool hasMore;
  private HubConnection hubConnection;

  protected override async Task OnInitializedAsync()
  {
    currentUserId = await AuthService.GetCurrentUserIdAsync();
    cardiMembers = await MemberService.GetAllMembersAsync();
    await LoadNotes();
    await ConnectToSignalR();
  }

  private async Task LoadNotes()
  {
    notes = await FamilyNoteService.GetNotesAsync(currentFilter, currentSort);
    groupedNotes = notes
      .GroupBy(n => n.GroupLabel) // "Today", "Yesterday", etc.
      .ToDictionary(g => g.Key, g => g.ToList());
  }

  private async Task ConnectToSignalR()
  {
    hubConnection = new HubConnectionBuilder()
      .WithUrl(Navigation.ToAbsoluteUri("/familyHub"))
      .Build();

    hubConnection.On<FamilyNote>("ReceiveNewNote", async (newNote) =>
    {
      notes.Insert(0, newNote);
      await InvokeAsync(StateHasChanged);
      await JSRuntime.InvokeVoidAsync("showToast",
        $"New note from {newNote.AuthorName}");
    });

    await hubConnection.StartAsync();
  }

  private async Task HandlePostNote(FamilyNoteModel model)
  {
    await FamilyNoteService.PostNoteAsync(model);
    await LoadNotes();
    showAddNoteModal = false;
  }

  private async Task LikeNote(Guid noteId)
  {
    await FamilyNoteService.LikeAsync(noteId);
    var note = notes.FirstOrDefault(n => n.Id == noteId);
    if (note != null)
    {
      note.LikeCount++;
      await InvokeAsync(StateHasChanged);
    }
  }

  public async ValueTask DisposeAsync()
  {
    if (hubConnection is not null)
    {
      await hubConnection.DisposeAsync();
    }
  }
}
```

---

## Analytics & Reporting

### Screen 6.1: Health Reports
**URL:** `/reports`
**User Story:** 9.2 - Printable Reports

**Layout:**
```
┌────────────────────────────────────────────────────────┐
│ Dashboard > Reports                                    │
│                                                        │
│ Health Reports                                         │
│                                                        │
│ Generate Report                                        │
│ ┌──────────────────────────────────────────────────┐ │
│ │ CardiMember *                                    │ │
│ │ [▼ Dad                                ]          │ │
│ │                                                  │ │
│ │ Report Type *                                    │ │
│ │ ● Health Summary (Comprehensive)                 │ │
│ │ ○ Activity Report                                │ │
│ │ ○ Heart Rate Analysis                            │ │
│ │ ○ Sleep Quality Report                           │ │
│ │ ○ Alert History                                  │ │
│ │                                                  │ │
│ │ Date Range *                                     │ │
│ │ From: [01/01/2026] To: [01/30/2026]             │ │
│ │ Quick select: [7 days] [30 days] [90 days]      │ │
│ │                                                  │ │
│ │ Include Sections:                                │ │
│ │ ☑ Summary Statistics                             │ │
│ │ ☑ Trend Charts                                   │ │
│ │ ☑ Alert History                                  │ │
│ │ ☑ Family Notes                                   │ │
│ │ ☐ Raw Data Table                                 │ │
│ │ ☑ Medication List (if available)                 │ │
│ │                                                  │ │
│ │ Format:                                          │ │
│ │ ● PDF (Recommended for doctors)                  │ │
│ │ ○ Excel (.xlsx)                                  │ │
│ │ ○ CSV (Data only)                                │ │
│ │                                                  │ │
│ │ [Preview Report]        [Generate & Download]   │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ Recent Reports                                         │
│ ┌──────────────────────────────────────────────────┐ │
│ │ Dad - Health Summary                             │ │
│ │ Jan 1-30, 2026 • Generated 2 days ago           │ │
│ │ [📄 Download PDF] [📧 Email]                     │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ ┌──────────────────────────────────────────────────┐ │
│ │ Mom - Activity Report                            │ │
│ │ Dec 1-31, 2025 • Generated 1 week ago           │ │
│ │ [📄 Download PDF] [📧 Email]                     │ │
│ └──────────────────────────────────────────────────┘ │
└────────────────────────────────────────────────────────┘
```

**Report Preview (Modal or New Page):**
```
┌────────────────────────────────────────────────────────┐
│ Report Preview                      [Print] [Download] │
│                                                        │
│ ┌──────────────────────────────────────────────────┐ │
│ │                  CardiTrack                      │ │
│ │          Health Summary Report                   │ │
│ │                                                  │ │
│ │ CardiMember: Dad Smith (78 years)                │ │
│ │ Report Period: January 1-30, 2026                │ │
│ │ Generated: January 31, 2026                      │ │
│ │                                                  │ │
│ │ CONFIDENTIAL HEALTH INFORMATION                  │ │
│ │════════════════════════════════════════════════│ │
│ │                                                  │ │
│ │ SUMMARY STATISTICS                               │ │
│ │                                                  │ │
│ │ Activity:                                        │ │
│ │   Average Steps:     4,250 / day                 │ │
│ │   Goal Achievement:  85%                         │ │
│ │   Trend:             ↓ Declining 15%             │ │
│ │                                                  │ │
│ │ Heart Rate:                                      │ │
│ │   Avg Resting HR:    72 bpm                      │ │
│ │   Range:             68-88 bpm                   │ │
│ │   Status:            Normal                      │ │
│ │                                                  │ │
│ │ Sleep:                                           │ │
│ │   Average:           7.2 hours                   │ │
│ │   Quality:           Good (78%)                  │ │
│ │   Pattern:           Consistent                  │ │
│ │                                                  │ │
│ │ [Activity Chart]                                 │ │
│ │ [Heart Rate Chart]                               │ │
│ │ [Sleep Chart]                                    │ │
│ │                                                  │ │
│ │ ALERT HISTORY (5 alerts)                         │ │
│ │ Jan 8:  Low Activity (Resolved)                  │ │
│ │ Jan 15: Sleep Pattern Change (Acknowledged)      │ │
│ │ ...                                              │ │
│ │                                                  │ │
│ │ FAMILY NOTES (3 notes)                           │ │
│ │ Jan 10: "Called Dad, had a cold..." - Sarah      │ │
│ │ ...                                              │ │
│ │                                                  │ │
│ │════════════════════════════════════════════════│ │
│ │ Page 1 of 4     Generated by CardiTrack         │ │
│ │                 HIPAA Confidential               │ │
│ └──────────────────────────────────────────────────┘ │
└────────────────────────────────────────────────────────┘
```

**Blazor Components:**
```razor
@page "/reports"
@attribute [Authorize]
@inject IReportService ReportService
@inject IPdfGenerator PdfGenerator

<PageTitle>Health Reports - CardiTrack</PageTitle>

<h1>Health Reports</h1>

<SectionCard Title="Generate Report">
  <EditForm Model="@reportConfig" OnValidSubmit="@GenerateReport">
    <DataAnnotationsValidator />

    <FormField Label="CardiMember" Required="true">
      <InputSelect @bind-Value="reportConfig.CardiMemberId" class="form-control">
        <option value="">Select CardiMember...</option>
        @foreach (var member in cardiMembers)
        {
          <option value="@member.Id">@member.Name</option>
        }
      </InputSelect>
    </FormField>

    <FormField Label="Report Type" Required="true">
      <div class="radio-group-vertical">
        <label>
          <InputRadio @bind-Value="reportConfig.ReportType"
                      Value="ReportType.HealthSummary" />
          Health Summary (Comprehensive)
        </label>
        <label>
          <InputRadio @bind-Value="reportConfig.ReportType"
                      Value="ReportType.Activity" />
          Activity Report
        </label>
        <label>
          <InputRadio @bind-Value="reportConfig.ReportType"
                      Value="ReportType.HeartRate" />
          Heart Rate Analysis
        </label>
        <label>
          <InputRadio @bind-Value="reportConfig.ReportType"
                      Value="ReportType.Sleep" />
          Sleep Quality Report
        </label>
        <label>
          <InputRadio @bind-Value="reportConfig.ReportType"
                      Value="ReportType.AlertHistory" />
          Alert History
        </label>
      </div>
    </FormField>

    <FormField Label="Date Range" Required="true">
      <div class="date-range">
        From: <InputDate @bind-Value="reportConfig.StartDate" />
        To: <InputDate @bind-Value="reportConfig.EndDate" />
      </div>
      <div class="quick-select">
        <button type="button" @onclick="@(() => SetDateRange(7))">7 days</button>
        <button type="button" @onclick="@(() => SetDateRange(30))">30 days</button>
        <button type="button" @onclick="@(() => SetDateRange(90))">90 days</button>
      </div>
    </FormField>

    <FormField Label="Include Sections">
      <div class="checkbox-group">
        <label>
          <InputCheckbox @bind-Value="reportConfig.IncludeSummaryStats" />
          Summary Statistics
        </label>
        <label>
          <InputCheckbox @bind-Value="reportConfig.IncludeCharts" />
          Trend Charts
        </label>
        <label>
          <InputCheckbox @bind-Value="reportConfig.IncludeAlerts" />
          Alert History
        </label>
        <label>
          <InputCheckbox @bind-Value="reportConfig.IncludeNotes" />
          Family Notes
        </label>
        <label>
          <InputCheckbox @bind-Value="reportConfig.IncludeRawData" />
          Raw Data Table
        </label>
        <label>
          <InputCheckbox @bind-Value="reportConfig.IncludeMedications" />
          Medication List (if available)
        </label>
      </div>
    </FormField>

    <FormField Label="Format">
      <div class="radio-group">
        <label>
          <InputRadio @bind-Value="reportConfig.Format" Value="ReportFormat.PDF" />
          PDF (Recommended for doctors)
        </label>
        <label>
          <InputRadio @bind-Value="reportConfig.Format" Value="ReportFormat.Excel" />
          Excel (.xlsx)
        </label>
        <label>
          <InputRadio @bind-Value="reportConfig.Format" Value="ReportFormat.CSV" />
          CSV (Data only)
        </label>
      </div>
    </FormField>

    <div class="form-actions">
      <button type="button" class="btn-secondary" @onclick="@PreviewReport">
        Preview Report
      </button>
      <button type="submit" class="btn-primary">
        Generate & Download
      </button>
    </div>
  </EditForm>
</SectionCard>

<SectionCard Title="Recent Reports">
  @if (recentReports.Any())
  {
    <div class="recent-reports-list">
      @foreach (var report in recentReports)
      {
        <ReportCard
          Report="@report"
          OnDownload="@(() => DownloadReport(report.Id))"
          OnEmail="@(() => EmailReport(report.Id))" />
      }
    </div>
  }
  else
  {
    <p>No reports generated yet</p>
  }
</SectionCard>

@if (showPreview)
{
  <ReportPreviewModal
    ReportData="@previewData"
    OnClose="@(() => showPreview = false)"
    OnPrint="@PrintReport"
    OnDownload="@DownloadPreviewedReport" />
}

@code {
  private List<CardiMember> cardiMembers = new();
  private List<GeneratedReport> recentReports = new();
  private ReportConfiguration reportConfig = new();
  private bool showPreview;
  private ReportData previewData;

  protected override async Task OnInitializedAsync()
  {
    cardiMembers = await MemberService.GetAllMembersAsync();
    recentReports = await ReportService.GetRecentReportsAsync();
  }

  private void SetDateRange(int days)
  {
    reportConfig.EndDate = DateTime.Now;
    reportConfig.StartDate = DateTime.Now.AddDays(-days);
  }

  private async Task PreviewReport()
  {
    previewData = await ReportService.GenerateReportDataAsync(reportConfig);
    showPreview = true;
  }

  private async Task GenerateReport()
  {
    var reportData = await ReportService.GenerateReportDataAsync(reportConfig);

    byte[] fileBytes;
    string fileName;
    string contentType;

    switch (reportConfig.Format)
    {
      case ReportFormat.PDF:
        fileBytes = await PdfGenerator.GenerateAsync(reportData);
        fileName = $"{reportData.MemberName}_Health_Report.pdf";
        contentType = "application/pdf";
        break;

      case ReportFormat.Excel:
        fileBytes = await ExcelGenerator.GenerateAsync(reportData);
        fileName = $"{reportData.MemberName}_Health_Report.xlsx";
        contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        break;

      case ReportFormat.CSV:
        fileBytes = await CsvGenerator.GenerateAsync(reportData);
        fileName = $"{reportData.MemberName}_Health_Report.csv";
        contentType = "text/csv";
        break;
    }

    await JSRuntime.InvokeVoidAsync("downloadFile",
      fileName,
      Convert.ToBase64String(fileBytes),
      contentType);

    // Save report record
    await ReportService.SaveReportRecordAsync(reportConfig, fileName);
    await LoadRecentReports();
  }

  private async Task DownloadReport(Guid reportId)
  {
    var reportBytes = await ReportService.GetReportBytesAsync(reportId);
    var report = recentReports.First(r => r.Id == reportId);
    await JSRuntime.InvokeVoidAsync("downloadFile",
      report.FileName,
      Convert.ToBase64String(reportBytes),
      report.ContentType);
  }

  private async Task EmailReport(Guid reportId)
  {
    // Show email modal or send directly
    await ReportService.EmailReportAsync(reportId);
    await JSRuntime.InvokeVoidAsync("showToast", "Report emailed successfully!");
  }
}
```

---

## Settings & Administration

### Screen 7.1: Account Settings
**URL:** `/settings/account`
**User Story:** User profile management

**Layout:**
```
┌────────────────────────────────────────────────────────┐
│ Settings > Account                                     │
│                                                        │
│ Account Settings                                       │
│                                                        │
│ Profile                                                │
│ ┌──────────────────────────────────────────────────┐ │
│ │ [Profile Photo]                                  │ │
│ │                                                  │ │
│ │ Full Name                                        │ │
│ │ [Sarah Johnson_____________________]            │ │
│ │                                                  │ │
│ │ Email Address                                    │ │
│ │ [sarah@email.com___________________]            │ │
│ │ ✓ Verified                                       │ │
│ │                                                  │ │
│ │ Phone Number (Optional)                          │ │
│ │ [(555) 123-4567____________________]            │ │
│ │                                                  │ │
│ │ Timezone                                         │ │
│ │ [▼ (GMT-5) Eastern Time            ]            │ │
│ │                                                  │ │
│ │ [Save Changes]                                   │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ Security                                               │
│ ┌──────────────────────────────────────────────────┐ │
│ │ Password                                         │ │
│ │ Last changed: 30 days ago                        │ │
│ │ [Change Password]                                │ │
│ │                                                  │ │
│ │ Two-Factor Authentication                        │ │
│ │ Status: ⚠️ Not Enabled (Recommended)            │ │
│ │ [Enable 2FA]                                     │ │
│ │                                                  │ │
│ │ Active Sessions                                  │ │
│ │ • Web Browser (Current)                          │ │
│ │   Chrome on Windows • Just now                   │ │
│ │ • Mobile App                                     │ │
│ │   iPhone • 2 hours ago        [Sign Out]        │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ Email Preferences                                      │
│ ┌──────────────────────────────────────────────────┐ │
│ │ ☑ Alert notifications                            │ │
│ │ ☑ Weekly digest                                  │ │
│ │ ☑ Product updates                                │ │
│ │ ☐ Marketing communications                       │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ Danger Zone                                            │
│ ┌──────────────────────────────────────────────────┐ │
│ │ Delete Account                                   │ │
│ │ Permanently delete your account and all data.    │ │
│ │ This action cannot be undone.                    │ │
│ │                                                  │ │
│ │ [Delete My Account]                              │ │
│ └──────────────────────────────────────────────────┘ │
└────────────────────────────────────────────────────────┘
```

---

## Enterprise Features

### Screen 8.1: Enterprise Dashboard (Assisted Living)
**URL:** `/enterprise/dashboard`
**User Story:** 8.1 - Multi-Resident Overview

**Layout:**
```
┌────────────────────────────────────────────────────────┐
│ Enterprise Dashboard                    [Export] [⚙️]  │
│                                                        │
│ Sunny Acres Assisted Living                            │
│ 52 Residents • 3 Alerts • 48 Good Status               │
│                                                        │
│ [Search residents...] [Filter ▼] [Sort ▼] [Grid/List] │
│                                                        │
│ Filters: [All] [Alerts] [Floor 1] [Floor 2]           │
│                                                        │
│ ┌──────────────────────────────────────────────────┐ │
│ │ Room  Name          Age  Status  Last Alert  Staff│ │
│ ├──────────────────────────────────────────────────┤ │
│ │ 101A  Johnson, M.   78   🟢      None        Sarah││ │
│ │ 102B  Smith, R.     82   🟡      2h ago      John │ │
│ │ 103A  Davis, L.     75   🟢      None        Sarah││ │
│ │ 104B  Wilson, P.    80   🔴      Just now    Mary │ │
│ │ 105A  Brown, E.     77   🟢      None        Sarah││ │
│ │ ...                                              │ │
│ └──────────────────────────────────────────────────┘ │
│                                                        │
│ Quick Stats                                            │
│ ┌──────────────────────────────────────────────────┐ │
│ │ Today's Activity                                 │ │
│ │ • 48 residents with normal activity              │ │
│ │ • 3 residents below baseline                     │ │
│ │ • 1 resident no movement (Wilson, P.)            │ │
│ │                                                  │ │
│ │ Heart Rate Alerts: 2                             │ │
│ │ Sleep Alerts: 1                                  │ │
│ └──────────────────────────────────────────────────┘ │
└────────────────────────────────────────────────────────┘
```

---

## Component Library

### Reusable Blazor Components

#### StatusIndicator Component
```razor
@* StatusIndicator.razor *@
<div class="status-indicator status-@Status.ToString().ToLower()">
  @switch (Status)
  {
    case HealthStatus.Good:
      <span class="status-icon">🟢</span>
      <span class="status-text">All Good</span>
      break;
    case HealthStatus.Caution:
      <span class="status-icon">🟡</span>
      <span class="status-text">Needs Attention</span>
      break;
    case HealthStatus.Urgent:
      <span class="status-icon">🟠</span>
      <span class="status-text">Action Recommended</span>
      break;
    case HealthStatus.Critical:
      <span class="status-icon">🔴</span>
      <span class="status-text">Urgent</span>
      break;
  }
</div>

@code {
  [Parameter] public HealthStatus Status { get; set; }
}
```

#### MetricCard Component
```razor
@* MetricCard.razor *@
<div class="metric-card metric-@Icon">
  <div class="metric-header">
    <span class="metric-icon">@GetIcon()</span>
    <span class="metric-title">@Title</span>
  </div>

  <div class="metric-value">@Value</div>

  <div class="metric-comparison">
    @Comparison
    @if (Trend != 0)
    {
      <span class="trend @(Trend > 0 ? "trend-up" : "trend-down")">
        @(Trend > 0 ? "↑" : "↓") @Math.Abs(Trend)%
      </span>
    }
  </div>

  @if (ChartData != null && ChartData.Any())
  {
    <div class="metric-sparkline">
      <SparklineChart Data="@ChartData" />
    </div>
  }

  <div class="metric-status status-@Status">
    @Status
  </div>
</div>

@code {
  [Parameter] public string Icon { get; set; }
  [Parameter] public string Title { get; set; }
  [Parameter] public string Value { get; set; }
  [Parameter] public string Comparison { get; set; }
  [Parameter] public int Trend { get; set; }
  [Parameter] public List<decimal> ChartData { get; set; }
  [Parameter] public string Status { get; set; }

  private string GetIcon()
  {
    return Icon switch
    {
      "steps" => "👟",
      "heart" => "❤️",
      "sleep" => "😴",
      _ => "📊"
    };
  }
}
```

---

**Document Status:** Complete
**Total Screens:** 35+ web screens
**Ready for:** Design mockups and development

---

## Next Steps

1. **Design Phase:**
   - Create high-fidelity mockups in Figma
   - Build interactive prototypes
   - User testing with target audience

2. **Development Phase:**
   - Set up Blazor Server project structure
   - Implement component library
   - Build pages following these specifications

3. **Integration:**
   - Connect to .NET 8 Web API
   - Implement SignalR hubs
   - Set up authentication (Auth0)

4. **Testing:**
   - Unit tests for components
   - Integration tests for pages
   - E2E tests for critical flows