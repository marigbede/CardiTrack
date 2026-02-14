# CardiTrack - Mobile App Screen Specifications

## Project Overview

**Product:** CardiTrack - Remote health monitoring for elderly family members
**Platform:** iOS (iPhone 12+) & Android (10+)
**Orientation:** Portrait primary, landscape supported
**Target Users:** Adult children (30-55) monitoring elderly parents' wearable health data
**Document Version:** 3.0
**Last Updated:** February 14, 2026

---

## Release Strategy

35 screens across 3 MVPs. Each MVP is a fully functional, shippable release.

| Release | Screens | Theme | User Gets |
|---------|---------|-------|-----------|
| **MVP 1** | 20 | Solo Monitoring | Sign up, connect device(s), monitor one parent, view trends, get alerts, configure notifications, manage devices, manage profile and subscription, export health data (HL7, FHIR) |
| **MVP 2** | 8 | Family & Multi-Member | Invite family, share notes, manage multiple CardiMembers, all alert types, scan test results with AI medical insights, export data in LOINC/CCD/SNOMED CT |
| **MVP 3** | 7 | Native & Offline | Biometric setup and login, offline support, push notification actions, home screen widget, native sharing |

---

## Screen Index

| ID | Screen | Release |
|----|--------|---------|
| M1-01 | Splash Screen | MVP 1 |
| M1-02 | Welcome / Landing | MVP 1 |
| M1-03 | Sign Up | MVP 1 |
| M1-04 | Add First CardiMember | MVP 1 |
| M1-05 | Device Connection - Selection | MVP 1 |
| M1-06 | Device Connection - OAuth | MVP 1 |
| M1-07 | Device Connection - Success | MVP 1 |
| M1-08 | Baseline Learning Info | MVP 1 |
| M1-09 | Main Dashboard | MVP 1 |
| M1-10 | Alerts List | MVP 1 |
| M1-11 | Alert Detail - Activity | MVP 1 |
| M1-12 | Alert Detail - Critical | MVP 1 |
| M1-13 | Settings Main | MVP 1 |
| M1-14 | Subscription Management | MVP 1 |
| M1-15 | Trend Charts | MVP 1 |
| M1-16 | Notification Settings | MVP 1 |
| M1-17 | CardiMember Detail | MVP 1 |
| M1-18 | Edit CardiMember | MVP 1 |
| M1-19 | Device Management | MVP 1 |
| M1-20 | Health Data Export | MVP 1 |
| M2-01 | Alert Detail - Heart Rate | MVP 2 |
| M2-02 | Family Members List | MVP 2 |
| M2-03 | Invite Family Modal | MVP 2 |
| M2-04 | Multi-Member Dashboard | MVP 2 |
| M2-05 | Shared Notes Feed | MVP 2 |
| M2-06 | Add / Edit Note | MVP 2 |
| M2-07 | Test Results Scanner | MVP 2 |
| M2-08 | Test Results Detail | MVP 2 |
| M3-01 | Biometric Setup | MVP 3 |
| M3-02 | Biometric Login | MVP 3 |
| M3-03 | Offline Mode Indicator | MVP 3 |
| M3-04 | Offline Data Cache Settings | MVP 3 |
| M3-05 | Push Notifications | MVP 3 |
| M3-06 | Home Screen Widget | MVP 3 |
| M3-07 | Share Sheet Integration | MVP 3 |

---

## User Flows

### Flow 1: First-Time Onboarding (MVP 1)

```
[M1-01 Splash] → [M1-02 Welcome] → [M1-03 Sign Up] → [M1-04 Add CardiMember]
                       │                                        │
                       │ "Sign In"                              │ "Skip"
                       ▼                                        ▼
                 (Login screen)                           [M1-09 Dashboard]
                                                          (empty state)

                       [M1-04 Add CardiMember]
                               │
                               ▼
                       [M1-05 Device Selection]
                               │
                               ▼
                       [M1-06 OAuth Permission]
                               │
                          ┌────┴────┐
                          │ Success │ Failure
                          ▼         ▼
                       [M1-07 Success] → Retry / Help
                          │
                          ▼
                       [M1-08 Baseline Info]
                          │
                          ▼
                       [M1-09 Dashboard]
```

### Flow 2: Daily Monitoring (MVP 1)

```
[App Launch] → [M1-09 Dashboard]
                     │
           ┌─────────┼───────────┐
           ▼         ▼           ▼
      [Call/SMS] [M1-10 Alerts] [M1-15 Trends]
                     │
                ┌────┴────┐
                ▼         ▼
           [M1-11]    [M1-12]
           Activity   Critical
                │         │
                ▼         ▼
           [Acknowledge] [Call Now]
```

### Flow 3: Settings & Management (MVP 1)

```
[Tab: Settings] → [M1-13 Settings Main]
                         │
               ┌─────────┼──────────┬──────────┐
               ▼         ▼          ▼          ▼
         [M1-14 Sub] [M1-16 Notif] [M1-17 Detail] [M1-19 Devices]
                                        │
                                        ▼
                                   [M1-18 Edit]
```

### Flow 3b: Data Export (MVP 1)

```
[M1-15 Trends] → Export icon → [M1-20 Health Data Export]
[M1-13 Settings] → "Export Health Data" → [M1-20 Health Data Export]
[M1-17 Detail] → "Export Data" → [M1-20 Health Data Export]
                                        │
                                   ┌────┴────┐
                                   ▼         ▼
                              [Save/Share] [Email]
```

### Flow 4: Family Collaboration (MVP 2)

```
[Tab: Family] → [M2-02 Family List]
                       │
                 ┌─────┴─────┐
                 ▼           ▼
           [M2-03 Invite] [M2-05 Notes]
                               │
                               ▼
                          [M2-06 Add Note]
```

### Flow 5: Multi-Member (MVP 2)

```
[M2-04 Multi-Dashboard] → tap member → [M1-09 Single Dashboard]
```

### Flow 6: Test Results (MVP 2)

```
[M2-07 Scanner] → Camera/Upload → OCR Processing
                                        │
                                        ▼
                                  [M2-08 Results Detail]
                                        │
                              ┌─────────┼──────────┐
                              ▼         ▼          ▼
                         [AI Insights] [Export]  [Share]
                                        │
                                        ▼
                                  [M1-20 Export]
```

---

## Navigation Structure

### Bottom Tab Bar (always visible)

```
┌────────────────────────────────────┐
│                                    │
│          Content Area              │
│                                    │
├──────────┬──────────┬─────┬────────┤
│ Dashboard│  Alerts  │Family│Settings│
└──────────┴──────────┴─────┴────────┘
```

- Badge count on Alerts tab for unread alerts
- Badge count on Family tab for pending invites (MVP 2)
- Family tab shows placeholder / "Coming Soon" in MVP 1, or can be hidden

### Flyout Menu (swipe from left edge)

- User profile header (photo, name, email)
- Dashboard
- CardiMembers (badge: member count)
- Alerts (badge: unread count)
- Family & Sharing (MVP 2)
- Subscription
- Settings
- Help & Support
- Sign Out

### Gesture Patterns

| Gesture | Context | Action |
|---------|---------|--------|
| Pull down | Any scrollable screen | Refresh data |
| Swipe left | List items | Reveal quick actions |
| Swipe right | List items | Reveal secondary actions |
| Swipe right (edge) | Any screen | Open flyout menu |
| Pinch | Chart views | Zoom in/out |
| Long press | Chart data points | Show tooltip |
| Long press | CardiMember photo | Change photo |

---

## MVP 1 — Solo Monitoring (20 screens)

A single user can sign up, add one CardiMember, connect one or more devices, monitor their health, view trends, receive and manage alerts, configure notification preferences, manage the CardiMember's profile, manage connected devices, export health data in HL7 and FHIR formats, and handle their subscription.

---

### M1-01: Splash Screen
**User Story:** 1.1-1.3 Onboarding
**Entry:** App launch
**Exit:** → M1-02 Welcome (first launch) | → M1-09 Dashboard (returning user) | → M3-02 Biometric Login (MVP 3, if enabled)

**Duration:** 2-3 seconds while app initializes

**Layout:**
- Full-screen gradient background (CardiTrack brand colors)
- Large CardiTrack logo (centered)
- App name beneath logo
- Loading spinner (bottom third)
- Version number (bottom, small text)

**States:**
- **Default:** Logo + spinner animation
- **Error:** If initialization fails → "Something went wrong. Tap to retry" with retry button

---

### M1-02: Welcome / Landing Screen
**User Story:** 1.1 First-Time Registration
**Entry:** ← M1-01 Splash (first launch only)
**Exit:** → M1-03 Sign Up | → Login screen ("Sign In")

**Layout:**

**Header (top 20%):**
- CardiTrack logo (top-left, small)
- "Sign In" link (top-right)

**Hero Carousel (middle 50%):**
- 3 swipeable slides with pagination dots:

| Slide | Illustration | Headline | Subtext |
|-------|-------------|----------|---------|
| 1 | Happy elderly person with smartwatch | "Peace of Mind for Your Family" | "Monitor loved ones' health from anywhere" |
| 2 | Phone showing health dashboard | "Works with Devices They Own" | "Fitbit, Apple Watch, Garmin & more" |
| 3 | Family members on phones | "Stay Connected as a Family" | "Share caregiving with siblings" |

**CTA Section (bottom 30%):**
- Primary button (full width, bold): "Start Free 30-Day Trial"
- Pricing text (small, muted): "Then $8/month - Cancel anytime"
- Secondary button (text style, subtle): "Sign In"
- Legal link (small): "By continuing, you agree to Terms & Privacy"

**Interactions:**
- Carousel auto-advances every 4 seconds, pauses on touch
- Pagination dots indicate current slide
- Swipe left/right between slides

---

### M1-03: Sign Up Screen
**User Story:** 1.1 Account Creation
**Entry:** ← M1-02 Welcome ("Start Free Trial")
**Exit:** → M1-04 Add CardiMember (success) | ← M1-02 Welcome (back) | → Login screen ("Already have an account?")

**Header:**
- Back button
- Title: "Create Account"
- Progress indicator: "Step 1 of 4"

**Form (scrollable):**

**Email & Password Section:**
- Label: "Email Address"
  - Text input, email keyboard, autocapitalization off
- Label: "Password"
  - Password input (masked)
  - Password strength bar below (weak → medium → strong)
  - Strength label: "Weak" / "Medium" / "Strong"
- Label: "Confirm Password"
  - Password input (masked)

**Divider:** Horizontal line with "OR" centered

**Social Login:**
- Button: "Continue with Google" (white background, Google logo)
- Button: "Continue with Apple" (black background, Apple logo)

**Terms:**
- Checkbox + label: "I agree to Terms of Service and Privacy Policy"
- "Terms of Service" and "Privacy Policy" are tappable links

**CTA:**
- Primary button: "Create Account" (disabled until form is valid)
- Error message area (hidden until error occurs)

**Bottom:**
- Link: "Already have an account? Sign In"

**Validation Rules:**
- Email: valid format, real-time feedback
- Password: min 8 characters, 1 uppercase, 1 number
- Confirm password: must match
- Terms checkbox: must be checked
- Button enables only when all validations pass

**States:**
- **Default:** Empty form
- **Validating:** Inline error messages appear beneath invalid fields
- **Loading:** Button shows spinner, form disabled
- **Error:** Error banner at top (e.g., "Email already registered")

---

### M1-04: Add First CardiMember
**User Story:** 1.2 Adding First CardiMember
**Entry:** ← M1-03 Sign Up (success)
**Exit:** → M1-05 Device Selection ("Continue") | → M1-09 Dashboard ("Skip for Now")

**Header:**
- Back button
- Title: "Add CardiMember"
- Progress indicator: "Step 2 of 4"

**Introduction:**
- Icon: person silhouette
- Text: "Who would you like to monitor?"
- Subtext: "We'll help you set up monitoring in just a few steps"

**Photo Section:**
- Circular photo placeholder (large)
- "Add Photo" button below
- Tap opens choice: Camera or Gallery

**Required Fields:**
- "Full Name *" — text input
- "Date of Birth *" — date picker (format: MM/DD/YYYY)
- "Relationship *" — dropdown picker:
  - Parent, Grandparent, Spouse, Sibling, Other

**Optional Fields (collapsible section):**
- Toggle: "Add More Details (Optional)"
- When expanded:
  - "Medical Notes" — multi-line text (max 500 chars, character counter shown)
    - Encrypted indicator icon visible
  - "Emergency Contact Name" — text input
  - "Emergency Contact Phone" — phone keyboard input

**Privacy Notice:**
- Info card with lock icon
- Text: "Your parent will be notified and can provide consent"

**CTA:**
- Primary button: "Continue"
- Text link: "Skip for Now"

**States:**
- **Default:** Empty form with photo placeholder
- **Photo added:** Shows uploaded image in circle
- **Loading:** Button shows spinner on submit

---

### M1-05: Device Connection - Selection
**User Story:** 1.3 Device Connection Wizard
**Entry:** ← M1-04 Add CardiMember ("Continue")
**Exit:** → M1-06 OAuth Permission (device selected)

**Header:**
- Back button
- Title: "Connect Device"
- Progress indicator: "Step 3 of 4"

**Introduction:**
- Text: "What device does [Name] use?"
- Subtext: "We support all major fitness trackers"

**Device Grid (2 columns on phone, 3 on tablet):**

Each device card:
- Rounded frame with shadow
- Device logo (medium)
- Device name (bold)
- "Supported" badge (checkmark)
- Entire card is tappable, highlights on selection

**Supported Devices:**

| Device | Models |
|--------|--------|
| Fitbit | Charge, Versa, Sense series |
| Apple Watch | Series 4+ |
| Garmin | Venu, Forerunner, etc. |
| Samsung Galaxy Watch | All models |
| Withings | ScanWatch, Move |
| Other / Manual Entry | Limited features |

**Bottom:**
- Link: "Don't see your device? Contact support"

**Interactions:**
- Single selection — tap to select, tap again to deselect
- Selected card shows highlighted border + checkmark overlay
- Selecting a device automatically proceeds to M1-06

---

### M1-06: Device Connection - OAuth Permission
**User Story:** 1.3 OAuth Flow
**Entry:** ← M1-05 Device Selection (device chosen)
**Exit:** → M1-07 Success (authorization complete) | ← M1-05 Device Selection ("Cancel")

**Header:**
- Back button
- Title: "[Device Name] Connection"

**Visual Connection (centered):**
- Large device logo
- Arrow/connection icon
- Large CardiTrack logo

**Permission List:**
- Label: "CardiTrack needs access to:"
- Each permission in its own row:

| Icon | Permission | Info Tooltip |
|------|-----------|-------------|
| Heart | Heart Rate Data | "Used to detect unusual patterns" |
| Shoe | Activity & Steps | "To monitor daily movement" |
| Moon | Sleep Data | "To spot rest pattern changes" |

- Each row has an (i) info button that shows the tooltip on tap

**Privacy Notice:**
- Card with light background
- Lock icon + text: "We never sell your data. Your information stays private and secure."

**CTA:**
- Primary button: "Authorize [Device Name]"
  - Tap opens device's OAuth login in a browser/webview
- Text link: "Cancel"

**States:**
- **Default:** Permission list visible
- **Authorizing:** Loading overlay with "Connecting to [Device]..." message
- **Error:** "Authorization failed. Please try again." with retry button

---

### M1-07: Device Connection - Success
**User Story:** 1.3 Connection Success
**Entry:** ← M1-06 OAuth (authorization complete)
**Exit:** → M1-08 Baseline Info ("Continue to Dashboard") | → M1-05 Device Selection ("Add Another Device")

**Animation:**
- Animated checkmark (plays once on entry)

**Success Message:**
- Heading: "Connected Successfully!"
- Text: "We're syncing [Name]'s data from [Device]"
- Subtext: "This may take a few minutes"

**Data Preview Card:**
- Title: "Latest Data"
- Rows:
  - Steps Today: 4,250
  - Last Synced: Just now
  - Heart Rate: 72 bpm

**Options:**
- Outlined button: "+ Add Another Device"
- Helper text: "You can connect multiple devices for [Name]"

**CTA:**
- Primary button: "Continue to Dashboard"

**States:**
- **Syncing:** Preview card shows shimmer/skeleton loading
- **Synced:** Preview card shows real data
- **Partial sync:** Some values show, others show "Syncing..."

---

### M1-08: Baseline Learning Info
**User Story:** 1.3 Baseline Setup
**Entry:** ← M1-07 Device Success
**Exit:** → M1-09 Dashboard ("Go to Dashboard")

**Header:**
- Title: "Learning Phase"
- Progress indicator: "Step 4 of 4"

**Illustration:**
- Animated graphic: brain with gears (learning concept)

**Explanation:**
- Heading: "CardiTrack is Learning [Name]'s Patterns"
- Body: "Over the next 30 days, our AI will learn what's normal for [Name]:"
- Bullet list:
  - "Typical wake/sleep times"
  - "Average daily activity levels"
  - "Resting heart rate baseline"

**Progress:**
- Progress bar: "Day 1 of 30"
- Label: "3% Complete"

**Options Card:**
- Toggle switch: "Use basic alerts while learning"
- Description: "Get simple threshold alerts (e.g., heart rate > 100)"

**CTA:**
- Primary button: "Go to Dashboard"

**MVP 2 addition:** Text link "Invite Family Members First" → M2-03

---

### M1-09: Main Dashboard (Single CardiMember)
**User Story:** 2.1 Daily Health Overview
**Entry:** Tab bar (Home) | ← M1-08 Baseline Info (first time)
**Exit:** → M1-10 Alerts List | → M1-15 Trend Charts | → M1-17 CardiMember Detail | → Phone call / SMS

**Header (fixed):**
- Greeting: "Good Morning, [User First Name]"
- Notification bell icon (with unread badge count)
- Refresh icon (pull-to-refresh also supported)

**Status Hero Card:**
- Large card with gradient background colored by status
- CardiMember photo (circular, large)
- Name and age: "[Name], 78"
- Large status indicator:

| Status | Label | Icon |
|--------|-------|------|
| Normal | "All Good!" | Checkmark |
| Caution | "Needs Attention" | Warning triangle |
| Urgent | "Action Recommended" | Lightning bolt |
| Critical | "Urgent" | Siren |

- Last synced: "Updated 10 minutes ago"
- Tap sync icon for manual refresh

**Quick Actions Row (3 horizontal buttons):**
- "Call [Name]" (phone icon) → initiates phone call
- "Send Message" (SMS icon) → opens SMS
- "View Details" (chart icon) → navigates to M1-17

**Key Metrics (3 cards in a row):**

**Card 1: Activity**
- Icon: shoe
- Large value: "4,250 steps"
- Visual progress bar (current vs. goal)
- Comparison text: "85% of normal" with trend arrow (up/down)
- Mini 7-day sparkline chart

**Card 2: Heart Rate**
- Icon: heart
- Large value: "72 bpm"
- Status: "Normal range"
- Range text: "68-75 bpm typical"
- Mini sparkline

**Card 3: Sleep**
- Icon: moon
- Large value: "7.2 hours"
- Quality: "Good" (4 out of 5 stars)
- Comparison: "Better than average"
- Mini sparkline

**Recent Alerts (conditional — only shown if alerts exist):**
- Section heading: "Recent Alerts"
- Horizontal scrollable alert cards
- Each card: icon, title, time, status
- Tap any card → M1-11 or M1-12 Alert Detail

**Bottom:**
- Button: "View Trends & History" → M1-15

**Interactions:**
- Pull-to-refresh triggers data sync
- Swipe left on metric card → see detail view
- Long-press on photo → change photo option

**States:**
- **Loading:** Skeleton/shimmer cards
- **Normal:** Full data displayed
- **Stale data:** Banner: "Data is X hours old. Pull to refresh."
- **No device connected:** Prompt card: "Connect a device to start monitoring" → M1-05
- **Baseline learning:** Shows progress bar instead of "% of normal" comparisons

**MVP 2 change:** When user has multiple CardiMembers, Home tab shows M2-04 instead

---

### M1-10: Alerts List
**User Story:** 3.1 Alert Management
**Entry:** Tab bar (Alerts) | ← M1-09 Dashboard (Recent Alerts)
**Exit:** → M1-11 Alert Detail (Activity) | → M1-12 Alert Detail (Critical) | → Phone call

**Header:**
- Title: "Alerts"
- Filter icon (funnel)
- Settings icon (gear) → M1-16 Notification Settings

**Filter Chips (horizontal scroll):**
- [All] [Unread] [Critical] [Today] [This Week]

**Alert List (grouped by date):**

Section headers: "Today" / "Yesterday" / "This Week" / "Older"

**Alert Card Layout:**
- Left border colored by severity
- Top row:
  - Severity badge: "CRITICAL" / "URGENT" / "INFO"
  - Timestamp: "2 hours ago"
  - Unread dot (if unread)
- Content:
  - CardiMember name + small photo (inline)
  - Alert title (bold): e.g., "Low Activity Detected"
  - Preview text (2 lines max): "Dad hasn't moved this morning. Typical wake time..."
- Bottom row:
  - Status label: "New" / "Acknowledged" / "Resolved"
  - Quick action icons: Call (phone) | Acknowledge (checkmark) | Expand (chevron)

**Swipe Actions:**
- Swipe right → "Acknowledge"
- Swipe left → "Call"

**Bottom:**
- Link: "View Archived Alerts"

**States:**
- **Default:** Grouped alert list
- **Empty:** Large bell icon (muted) + "No Alerts" + "We'll notify you if anything needs attention"
- **Filtered empty:** "No alerts match this filter"
- **Loading:** Skeleton cards

**MVP 2 addition:** Heart rate alerts tap → M2-01

---

### M1-11: Alert Detail - Activity
**User Story:** 11.1 Activity Decline
**Entry:** ← M1-10 Alerts List (tap alert card)
**Exit:** ← M1-10 Alerts List (back) | → Phone call | → SMS | → M1-15 Trend Charts

**Header:**
- Back button
- Title: "Alert Details"
- Share button

**Alert Header Card:**
- Caution-level severity banner
- Warning icon
- Title: "Low Activity Alert"
- CardiMember photo + name
- Timestamp: "January 10, 2026 at 11:30 AM"

**Description:**
- Card with icon
- Large readable text: "Dad's activity has been lower than usual"

**Mini Trend Chart:**
- 2-week activity trend showing declining line
- Baseline range shaded, current data overlaid

**Comparison Card (2-column grid):**

| Current | Normal |
|---------|--------|
| "Recent Average" | "Normal Average" |
| 2,500 steps/day | 5,000 steps/day |

- Full-width highlighted row: "-50% below normal"

**Context Card:**
- Lightbulb icon
- "This could indicate:"
  - Illness or fatigue
  - Pain or discomfort
  - Low mood or depression

**Recommended Actions (full-width button list):**
1. "Call to Check In" (primary, phone icon)
2. "Send a Message" (secondary, SMS icon)
3. "Schedule Doctor Visit" (secondary, calendar icon)

**More Options (collapsible):**
- "Adjust Baseline" (if this is a new normal)
- "Add Note About This Alert"
- "Share with Family"

**Acknowledgment Section:**
- If unread: Button "Mark as Acknowledged"
- If acknowledged: "Acknowledged by Sarah, 30 min ago" + any notes

**Bottom:**
- Button: "View Detailed Activity Data" → M1-15

---

### M1-12: Alert Detail - Critical (No Movement)
**User Story:** 11.3 No Morning Activity
**Entry:** ← M1-10 Alerts List | Push notification (direct)
**Exit:** ← M1-10 Alerts List (back) | → Phone call | → Note input

This is the most safety-critical screen in the app. Design for urgency and immediate action.

**Alert Header:**
- Full-width critical severity banner (pulsing animation)
- Large siren icon
- Title: "CRITICAL: No Movement Detected"
- CardiMember photo + name
- Timestamp

**Urgent Message Card (thick border, critical severity):**
- Large text: "Dad hasn't moved today"
- Details:
  - "Typical wake time: 7:00 AM"
  - "Current time: 11:00 AM"
  - "No activity for 4 hours"

**Last Known Activity Card:**
- "Last Movement Detected:"
- "Yesterday, 10:30 PM"
- "Location: Bedroom (based on device)"

**Immediate Actions (large, prominent buttons):**
1. **"CALL NOW"** — critical severity, oversized, phone icon, one-tap to dial, shows phone number
2. **"I'M CHECKING IN PERSON"** — urgent severity, large — updates status and notifies family immediately

**Dismissal Option:**
- Button: "He Told Me He'd Sleep In"
  - Opens a note field for context
  - Dismisses alert with explanation logged

**Family Notification Card:**
- "Who else has been notified:"
  - Sarah (via SMS) — timestamp
  - John (via Push) — timestamp

**Event Timeline:**
- Vertical timeline:
  - 10:30 PM — Last movement
  - 7:00 AM — Expected wake time
  - 9:00 AM — Alert threshold reached
  - 11:30 AM — You were notified

---

### M1-13: Settings Main
**User Story:** 6.1, 6.2 Settings
**Entry:** Tab bar (Settings) | Flyout menu
**Exit:** → M1-14 Subscription | → M1-16 Notification Settings | → M1-17 CardiMember Detail | → M1-19 Device Management | → M1-20 Health Data Export

**User Profile Section (top card):**
- Profile photo (large, tappable to edit)
- Name: "[User Name]"
- Email: "[user@email.com]"
- Edit button (pencil icon)

**Settings Groups (grouped list):**

**Account**
- My Profile →
- Subscription & Billing → (badge: current plan name)
- Family & Sharing → (MVP 2)

**CardiMembers**
- Manage CardiMembers →
- Connected Devices → M1-19
- Export Health Data → M1-20

**Health Records (MVP 2)**
- Scan Test Results → M2-07

**Notifications**
- Alert Settings →
- Notification Preferences →
- Quiet Hours →

**Security**
- Change Password →
- Biometric Login (inline toggle switch)
- Privacy Settings →

**Support**
- Help Center →
- Contact Support →
- Terms & Privacy →

**About**
- App Version (value: "1.0.0")
- Check for Updates

**Danger Zone (separated visually):**
- "Sign Out" (destructive text)
- "Delete Account" (destructive text)

**MVP 2 addition:** Family & Sharing → M2-02

---

### M1-14: Subscription Management
**User Story:** 6.1 Subscription
**Entry:** ← M1-13 Settings ("Subscription & Billing")
**Exit:** ← M1-13 Settings (back) | → Payment method change | → Plan change

**Current Plan Card (gradient background):**
- Badge: "COMPLETE CARE"
- Price: "$15/month"
- Renewal date: "Renews Feb 10, 2026"
- Button: "Manage Subscription"

**Included Features (checklist):**
- Unlimited CardiMembers
- Advanced ML Alerts
- Family Sharing (5 members)
- 90-day data retention
- Priority support

**Usage Section:**
- Progress bars with labels:
  - CardiMembers: 2 of unlimited
  - Family Members: 3 of 5
  - Data retention: 45 days of 90

**Plan Comparison (horizontal swipeable cards):**
- 3 plan cards, swipe to compare
- Each card:
  - Plan name + price/month
  - "Current Plan" badge (if active)
  - Condensed feature list
  - Button: "Current Plan" (disabled) / "Upgrade" / "Downgrade"

**Annual Discount Banner:**
- "Save 15% with Annual Billing"
- "Switch to Annual" button

**Billing Section:**
- Payment method: "Visa ---- 1234" (with card icon)
- "Change" button
- "Billing History" button

---

### M1-15: Trend Charts
**User Story:** 2.3 Historical Data
**Entry:** ← M1-09 Dashboard ("View Trends") | ← M1-11 Alert Detail ("View Detailed Data")
**Exit:** ← Previous screen (back) | → M1-20 Health Data Export

**Header:**
- Back button
- Title: "[Name]'s Trends"
- Export/share icon

**Time Range Selector (segmented control):**
- [7D] [30D] [90D] [Custom]
- Custom opens a date range picker modal

**Metric Tabs (horizontal scroll):**
- [Activity] [Heart Rate] [Sleep] [All]

**Chart Area:**
- Line chart:
  - X-axis: dates
  - Y-axis: metric values
  - Shaded area: baseline/normal range
  - Line: actual data
  - Markers: alert events on timeline
- Pinch to zoom
- Double-tap to reset zoom

**Interactive Tooltip (long-press on data point):**
- Popup showing:
  - Date/time
  - Exact value
  - "120% above baseline"
  - Note icon (if notes exist for that date)

**Timeline Annotations (below chart, horizontal scroll):**
- Alert markers with icons
- Note markers with text preview
- Tap to expand details

**Summary Stats Card (bottom):**
- Average: "4,500 steps"
- High: "8,200 (Jan 5)"
- Low: "1,200 (Jan 8)"
- Trend: "Declining 15%" with down arrow

**Export Options (via share icon):**
- Export to PDF
- Export to CSV
- Share screenshot
- Send to email

---

### M1-16: Notification Settings
**User Story:** 3.2 Alert Preferences
**Entry:** ← M1-13 Settings | ← M1-10 Alerts List (gear icon)
**Exit:** ← Previous screen (back)

**CardiMember Selector (if multiple members):**
- Dropdown: "Settings for: [Dad]"

**Alert Type Groups (each with enable toggle):**

**Activity Alerts**
- Toggle: enabled/disabled
- Sensitivity slider: Low | Medium | High
- Description: "Alert when activity is 30% below normal"

**Heart Rate Alerts**
- Toggle: enabled/disabled
- Sensitivity slider: Low | Medium | High
- Description: "Alert when HR exceeds baseline by 20%"

**Sleep Alerts**
- Toggle: enabled/disabled
- Checkboxes:
  - Poor sleep quality
  - Unusual sleep patterns

**Pattern Break Alerts**
- Toggle: always on (cannot disable)
- Label: "Required for emergency detection"

**Notification Channels (per alert type):**
- Multi-select chips: [Email] [SMS] [Push] [All]

**Quiet Hours (collapsible):**
- Toggle: "Enable Quiet Hours"
- Time pickers: From 10:00 PM → To 7:00 AM
- Exception toggle: "Still alert for Critical events"

**Family Routing (MVP 2):**
- "Also notify these family members:"
- Checkboxes with severity chips:
  - Sarah Johnson — [High Severity] [Critical]
  - John Doe — [Critical Only]

**Test Section:**
- "Send Test Push Notification" button
- "Send Test Email" button
- "Send Test SMS" button

---

### M1-17: CardiMember Detail
**Entry:** ← M1-09 Dashboard ("View Details") | ← M1-13 Settings ("Manage CardiMembers")
**Exit:** ← Previous screen (back) | → M1-18 Edit CardiMember | → M1-09 Dashboard | → M1-10 Alerts

**Profile Section (centered):**
- Large photo (prominent, centered)
- Name (large text)
- Age & relationship: "78 years old - Dad"

**Contact Info Card:**
- Emergency contact: name, phone (tappable to call), relationship

**Medical Info Card (encrypted):**
- Lock icon in card header
- Collapsible: "Medical Notes"
- Viewing requires biometric authentication
- "Edit" also requires biometric auth

**Monitoring Info Card:**
- Connected devices: "2 devices"
- Monitoring since: "Jan 1, 2026"
- Baseline status: "Learning (15 days)" or "Established"

**Action Buttons:**
- "View Dashboard" → M1-09
- "View Alerts" → M1-10
- "Manage Devices" → M1-19

**Danger Zone (separated):**
- "Pause Monitoring" button (warning treatment)
- "Remove CardiMember" button (destructive treatment)

---

### M1-18: Edit CardiMember
**Entry:** ← M1-17 CardiMember Detail (edit button)
**Exit:** ← M1-17 CardiMember Detail (cancel or save)

**Header:**
- Cancel button
- Title: "Edit [Name]"
- Save button (enabled when changes exist)

**Form (scrollable):**

**Photo:** Large circular image + "Change Photo" button

**Basic Info:**
- "Full Name" — text input
- "Date of Birth" — date picker
- "Relationship" — dropdown picker

**Optional Info:**
- "Medical Notes" — multi-line (encrypted)
- "Emergency Contact Name" — text input
- "Emergency Contact Phone" — phone input

**Monitoring Preferences:**
- Toggle: "Enable Monitoring"
- Dropdown: "Alert Sensitivity" — Low / Medium / High

**CTA:**
- Primary button: "Save Changes"

**Behavior:**
- Tracks unsaved changes
- "Unsaved changes" warning if navigating away without saving

---

### M1-19: Device Management
**User Story:** 6.2 Devices
**Entry:** ← M1-13 Settings ("Connected Devices") | ← M1-17 CardiMember Detail ("Manage Devices")
**Exit:** ← Previous screen (back) | → M1-05 Device Selection ("Add Device")

**Header:**
- Back button
- Title: "Connected Devices"
- "+ Add Device" button

**Devices List (grouped by CardiMember):**

**Group Header:** CardiMember name + photo

**Device Card:**
- Device logo (small, left)
- Device info:
  - Name: "Dad's Fitbit Charge 5"
  - Status badge:
    - Normal: "Active" (synced 10m ago)
    - Caution: "Token Expiring Soon"
    - Critical: "Disconnected"
  - Data sources: "Activity, HR, Sleep"
  - Primary device star (if designated)
- Menu icon (three dots)

**Context Menu:**
- Refresh Connection
- Set as Primary (toggle)
- View Sync History
- Remove Device (destructive text)

**Expanded Detail (tap card to expand):**
- Last sync: "10 minutes ago"
- Next sync: "In 20 minutes"
- Data synced today: "4 updates"
- Battery: "75%" (if available from device)

**Troubleshooting (bottom, collapsible):**
- "Device Not Syncing?"
  - Check Bluetooth
  - Reconnect OAuth
  - Contact support

---

### M1-20: Health Data Export
**User Story:** 6.3 Data Export
**Entry:** ← M1-15 Trend Charts (Export icon) | ← M1-13 Settings ("Export Health Data") | ← M1-17 CardiMember Detail ("Export Data")
**Exit:** ← Previous screen (back) | → Share sheet / email

**Header:**
- Back button
- Title: "Export Health Data"

**CardiMember Selector:**
- Dropdown: "Export data for: [Dad]"

**Date Range:**
- "From" date picker
- "To" date picker
- Quick presets: [Last 7 Days] [Last 30 Days] [Last 90 Days] [All Data]

**Data Selection (checkboxes):**
- Activity & Steps
- Heart Rate
- Sleep Data
- Alerts & Events
- Notes (if any)

**Export Format (radio buttons):**

| Format | Description | Use Case |
|--------|-------------|----------|
| PDF Report | Human-readable summary with charts | Sharing with family or personal records |
| CSV | Raw data spreadsheet | Personal analysis |
| HL7 v2 | Health Level Seven messaging format | Hospital system integration |
| FHIR (R4) | Fast Healthcare Interoperability Resources | Modern EHR integration, patient portals |

**MVP 2 addition:** LOINC, CCD, and SNOMED CT formats added (see M2-08)

**Format Info (expandable per format):**
- Tap info icon next to HL7/FHIR → explains format, typical recipients (hospitals, clinics, patient portals)

**Delivery Method:**
- "Save to Device" (default)
- "Email to..." — email input with autocomplete (self, doctor, family)
- "Share via..." — opens native share sheet

**Preview Section:**
- "Preview Export" button — shows first page / sample of export
- Estimated file size: "~2.4 MB"

**CTA:**
- Primary button: "Export Data"
- Progress: "Generating export..." with progress bar
- Success: "Export complete!" with option to share or save

**States:**
- **Default:** Format and date selection
- **Generating:** Progress bar with cancel option
- **Complete:** Success message with share/save actions
- **Error:** "Export failed. Please try again." with retry

---

## MVP 2 — Family, Multi-Member & Clinical (8 screens)

Adds family collaboration: invite siblings to share caregiving, shared notes, manage multiple CardiMembers from a dedicated dashboard, all alert detail types, test results scanning with medical inference, and expanded data export formats (LOINC, CCD, SNOMED CT).

**Prerequisite:** MVP 1 must be complete. MVP 2 extends existing screens (noted as "MVP 2 addition/change" in MVP 1 specs).

---

### M2-01: Alert Detail - Heart Rate
**User Story:** 11.2 Elevated HR
**Entry:** ← M1-10 Alerts List
**Exit:** ← M1-10 Alerts List (back) | → Phone call | → M1-15 Trend Charts

**Alert Header:**
- Urgent severity banner
- Lightning bolt icon
- Title: "Elevated Heart Rate Alert"
- CardiMember photo + name + timestamp

**Description:**
- "Mom's resting heart rate has been elevated for 3 consecutive days"

**Chart:**
- 7-day heart rate chart
- Shaded normal range (68-75 bpm)
- Elevated portion highlighted with urgent severity treatment

**Comparison Grid:**

| Current | Normal | Difference |
|---------|--------|-----------|
| 88 bpm | 68 bpm | +29% above baseline |

**Context Card:**
- "Possible causes:"
  - Infection or illness
  - Stress or anxiety
  - Dehydration
  - Medication side effects

**Recommended Actions:**
1. "Recommend Doctor Visit" (primary, urgent treatment)
2. "Monitor for 2 More Days" (secondary)
3. "Call to Check Symptoms" (secondary)

**Medical History (collapsible):**
- "Related Health Info"
- Shows medications, conditions from CardiMember profile

---

### M2-02: Family Members List
**User Story:** 4.1 Family Management
**Entry:** Tab bar (Family) | ← M1-13 Settings ("Family & Sharing")
**Exit:** → M2-03 Invite Modal | → Role management | → M2-05 Shared Notes

**Header:**
- Title: "Family & Sharing"
- "+ Invite" button

**Tabs:**
- [Active Members] [Pending Invites]

**Active Members List:**

Each member card:
- Profile photo (circular, small)
- Name: "Sarah Johnson"
- Email: "sarah@email.com"
- Role badge: "ADMIN" / "STAFF" / "VIEWER"
- Last active: "Active 2 hours ago"
- Menu icon (three dots)

**Context Menu (on three-dot tap):**
- Change Role → role picker
- View Activity Log
- Remove Access (destructive text)

**Pending Invites Tab:**
- Email shown
- Role assigned
- Sent date: "2 days ago"
- "Resend" button
- "Revoke" button (destructive text)

**Empty State (pending tab):** "No pending invitations"

**Floating Action Button (bottom-right):**
- "+" icon → opens M2-03 Invite Modal

---

### M2-03: Invite Family Modal
**User Story:** 4.1 Inviting Members
**Entry:** ← M2-02 Family List ("+" or "Invite") | ← M1-08 Baseline Info ("Invite Family Members First")
**Exit:** ← M2-02 Family List (close/cancel) | → Success confirmation

**Presentation:** Bottom sheet or full-screen modal

**Header:**
- Close button (X)
- Title: "Invite Family Member"

**Form:**

**Email Input:**
- Label: "Email Address"
- Email keyboard
- Inline validation indicator

**Role Selection (segmented control):**
- [Admin] [Staff] [Viewer]
- Selected role shows description:

| Role | Description |
|------|-------------|
| Admin | Can view, modify settings, invite others |
| Staff | Can view and acknowledge alerts |
| Viewer | Can only view health data |

**Permission Details (collapsible):**
- Table showing what each role can/cannot do

**Personal Message (optional):**
- Label: "Add a message (optional)"
- Multi-line input
- Placeholder: "Hi Sarah, I'd like to share Dad's health monitoring with you..."

**CTA:**
- Primary button: "Send Invitation"
- Text link: "Cancel"

---

### M2-04: Multi-Member Dashboard
**User Story:** 2.2 Multi-Member View
**Entry:** Tab bar (Home) — replaces M1-09 when user has multiple CardiMembers
**Exit:** → M1-09 Single Dashboard (tap member) | → M1-04 Add CardiMember ("+ Add")

**Header:**
- Title: "My CardiMembers"
- Filter icon → opens filter sheet
- "+ Add" button

**Filter Bar (collapsible, horizontal scroll):**
- Chips: [All] [Alerts Only] [Good Status]
- Sort button: "Sort by Status"

**CardiMember Cards (vertical scroll):**

Each card:
- Left: Circular photo (medium) with status badge overlay
- Middle: Name (bold), Age & relationship, Status text, Last synced
- Right: Chevron + alert count badge (if any)

**Swipe Actions:**
- Swipe left → "Call" button
- Swipe right → "Details" button

**Floating Action Button (bottom-right):**
- "+" icon → Add CardiMember flow

**States:**
- **Default:** Member cards listed
- **Empty:** Illustration + "No CardiMembers Yet" + "Add Your First CardiMember" button

---

### M2-05: Shared Notes Feed
**User Story:** 4.2 Coordination
**Entry:** ← M2-02 Family List | Tab bar (Family) → Notes sub-tab
**Exit:** → M2-06 Add Note | ← Previous screen (back)

**Header:**
- Back button
- Title: "Family Notes"
- Filter dropdown: "All Notes"

**Add Note Input (top):**
- User photo + text input: "Add a note for the family..."
- Tap → opens M2-06 full composer

**Notes Feed:**

Each note card:
- Author photo (small) + author name + timestamp ("2 hours ago")
- Menu (three dots) — only shown if you're the author
- Note text content (with @mentions highlighted)
- Attachments (if any)
- CardiMember tag: "About: Dad" (if associated)
- Footer: Reply button + count | Like button + count

**Threaded Replies (expandable):**
- Tap reply count → expands inline
- Indented reply cards
- "Load more replies" if more than 3

**Filter Options (dropdown):**
- All Notes / About Dad / About Mom / My Notes Only / Mentions Me

---

### M2-06: Add / Edit Note
**User Story:** 4.2 Shared Notes
**Entry:** ← M2-05 Notes Feed (tap input or "+" button)
**Exit:** ← M2-05 Notes Feed (cancel or post)

**Presentation:** Full-screen modal

**Header:**
- Cancel button
- Title: "New Note"
- Post button (enabled when content exists)

**Note Input:**
- Multi-line text editor (expands with content)
- Placeholder: "Share an update with family..."
- Character counter: "0 / 500"
- Typing "@" triggers a mention picker overlay listing family members

**CardiMember Association:**
- Label: "About (optional)"
- Dropdown: None (General) / Dad / Mom

**Attachments:**
- Button: "+ Attach Photo"
- Shows thumbnail grid when photos added
- Maximum 3 attachments

**Visibility:**
- Label: "Who can see this"
- Default: "All family members"

**CTA:**
- Primary button: "Post Note"
- Success → toast confirmation + return to feed

---

### M2-07: Test Results Scanner
**User Story:** 7.1 Lab Results Capture
**Entry:** ← M1-13 Settings ("Scan Test Results") | ← M1-17 CardiMember Detail ("Add Test Results") | Tab bar (dedicated entry point)
**Exit:** → M2-08 Test Results Detail (scan complete) | ← Previous screen (cancel)

**Header:**
- Close button (X)
- Title: "Scan Test Results"

**CardiMember Selector:**
- Dropdown: "Scan for: [Dad]"

**Capture Options (2 large cards):**

| Option | Icon | Description |
|--------|------|-------------|
| Camera Scan | Camera icon | "Take a photo of a lab report or test result" |
| Upload File | Document icon | "Upload a PDF or image from your files" |

**Camera View (after selecting Camera Scan):**
- Full-screen camera viewfinder
- Guide overlay: document frame outline with corner markers
- Instructions: "Align the test results within the frame"
- Capture button (large, centered bottom)
- Flash toggle (top-right)
- Gallery shortcut (bottom-left)

**Processing State (after capture/upload):**
- Document thumbnail (showing captured image)
- Progress animation: "Analyzing results..."
- Steps indicator:
  1. "Extracting text..." (OCR)
  2. "Identifying test values..."
  3. "Cross-referencing medical standards..."
- Cancel button: "Cancel Analysis"

**Multi-Page Support:**
- After first page captured: "Add Another Page" button
- Page indicator: "Page 1 of 1"
- Swipe between captured pages

**Error Handling:**
- Blurry image: "Image is too blurry. Please retake."
- Unreadable: "Could not read the document. Try better lighting or upload a PDF."
- Partial read: "Some values couldn't be identified. You can review and correct them."

**States:**
- **Default:** Capture options
- **Camera active:** Viewfinder with guide overlay
- **Processing:** Analysis progress animation
- **Error:** Error message with retry/retake options

---

### M2-08: Test Results Detail
**User Story:** 7.2 Results Analysis
**Entry:** ← M2-07 Test Results Scanner (analysis complete) | ← M1-17 CardiMember Detail ("View Test Results")
**Exit:** ← Previous screen (back) | → M1-20 Health Data Export | → Share

**Header:**
- Back button
- Title: "Test Results"
- Share icon | Export icon

**Result Summary Card:**
- CardiMember photo + name
- Test date: "February 10, 2026"
- Source: "Scanned lab report" | "Uploaded PDF"
- Lab name (if detected): "City Medical Lab"

**Parsed Results Table:**

Each result row:
- Test name (bold): e.g., "Hemoglobin A1c"
- Value: "6.2%"
- Reference range: "4.0 - 5.6%"
- Status indicator:
  - Normal: within range
  - High: above range (with severity indication)
  - Low: below range (with severity indication)
- Edit icon (pencil) — allows manual correction of OCR errors

**AI Medical Insights Card:**
- Lightbulb icon + "Medical Insights"
- Disclaimer banner: "These insights are informational only and do not constitute medical advice. Always consult a healthcare professional."
- AI-generated observations:
  - "Hemoglobin A1c is slightly elevated, suggesting pre-diabetic range"
  - "Cholesterol levels are within normal range"
  - "Consider discussing Vitamin D supplementation with a doctor"
- Each insight can be expanded for more detail
- "Learn More" links to relevant health information

**Trend Comparison (if previous results exist):**
- Side-by-side comparison with last test
- Trend arrows (improving / worsening / stable)
- Mini chart showing value over time

**Corrections Section (collapsible):**
- "Review & Correct Values"
- Editable fields for each parsed value
- "Mark as Verified" button

**Export & Sharing:**
- "Export Results" → M1-20 Health Data Export
- "Share with Doctor" → pre-formatted email/share
- "Add to Health Record" → saves to CardiMember profile

**Data Standards (MVP 2 export formats):**
- Results are encoded using:
  - **LOINC** — standardized lab test codes (e.g., Hemoglobin A1c = LOINC 4548-4)
  - **SNOMED CT** — clinical terminology for conditions and findings
  - **CCD** — Continuity of Care Document for structured clinical summaries
- These formats are available in M1-20 Health Data Export as additional export options

**States:**
- **Default:** Parsed results with insights
- **Editing:** Inline editing mode for value corrections
- **No previous results:** Trend section hidden
- **Loading insights:** Skeleton loading for AI insights section

---

## MVP 3 — Native & Offline (7 screens)

Adds platform-native polish: biometric security (setup and login), offline data access with sync queue, rich push notifications with inline actions, home screen widgets for at-a-glance monitoring, and native share sheet for exporting data to doctors or family.

**Prerequisite:** MVP 2 must be complete.

---

### M3-01: Biometric Setup
**User Story:** 10.2 Biometric Login
**Entry:** ← M1-13 Settings (Security section)
**Exit:** ← M1-13 Settings | → Skip ("Set Up Later")

**Header:**
- "Skip" link (top-right)
- Title: "Secure Your Account"

**Biometric Icon (centered, large):**
- iOS: Face ID icon
- Android: Fingerprint icon

**Explanation:**
- Heading: "Enable [Face ID / Fingerprint]"
- Body: "Quickly and securely access health data"
- Benefits:
  - Login in seconds
  - Extra security layer
  - Required for sensitive actions

**CTA:**
- Primary button: "Enable [Biometric]" — triggers device biometric enrollment
- Text link: "Set Up Later"

---

### M3-02: Biometric Login
**User Story:** 10.2 Biometric Auth
**Entry:** ← M1-01 Splash (when biometric enabled via M3-01)
**Exit:** → M1-09 Dashboard (success) | → Password fallback

Replaces password entry on app launch when biometric is enabled.

- CardiTrack logo + user name/photo
- Platform biometric prompt (Face ID on iOS, fingerprint on Android)
- "Scan to unlock" label
- Fallback: "Use Password" link → password entry field
- Configurable biometric requirements: app launch, viewing alerts, acknowledging alerts, changing settings

---

### M3-03: Offline Mode Indicator
**User Story:** 10.1 Offline Support
**Entry:** Automatic — appears when device loses connectivity
**Exit:** Automatic — disappears when connection restored

**Offline Banner (top of screen, persistent):**
- Warning-level background
- Crossed-out signal icon
- Text: "Offline Mode"
- Subtext: "Last updated 2 hours ago"
- Closable (X) but reappears on navigation

**Dashboard Modifications (offline state):**
- All data shown is cached/stale
- Sync icons greyed out
- "Syncing disabled" tooltip on refresh attempt

**Alert Queue Card:**
- "2 alerts pending sync"
- Shows queued alert cards with "Not yet synced" badge
- "Will sync when connected"

**Behavior:**
- Read-only mode: no POST operations
- Actions queued for sync when reconnected

**Connection Restored:**
- Toast: "Back online!"
- Syncing animation with progress
- Success message: "All data synced"

---

### M3-04: Offline Data Cache Settings
**User Story:** 10.1 Cache Management
**Entry:** ← M1-13 Settings
**Exit:** ← M1-13 Settings (back)

**Cache Info Card:**
- "Cached Data Size: 45 MB"
- "Last synced: 10 minutes ago"

**Settings:**
- Slider: "Days to cache" — 1 day | 7 days | 30 days
- Toggle: "Auto-download charts"
- Toggle: "Cache photos"

**Actions:**
- "Clear Cache" button — confirmation dialog: "You'll need internet to view data"
- "Sync Now" button

---

### M3-05: Push Notifications
**User Story:** 5.1

Designs for system-level notification UI.

**Lock Screen (compact):** App icon + "[Name] - Critical Alert" + body preview + timestamp
**Lock Screen (expanded on long press):** Full alert text + action buttons: "Call" / "View" / "Acknowledge"
**In-App Banner:** Slides from top, shows summary, tap to navigate, swipe up to dismiss
**Notification Center:** Grouped by CardiMember with expandable lists + app badge count

---

### M3-06: Home Screen Widget
**User Story:** 5.2

**Small Widget (2x2):** Logo + CardiMember photo + status indicator + name + last synced
**Medium Widget (4x2):** 2 CardiMembers side-by-side with photo, name, status, key metric
**Large Widget (4x4, iOS):** Up to 4 CardiMembers with mini dashboards (photo, name, status, 3 metrics, alert badge)
**Configuration:** Long-press → select CardiMembers, choose metrics, set update frequency

---

### M3-07: Share Sheet Integration

Native share sheet triggered from charts and alerts.

**Options:** Export PDF / Export CSV / Share screenshot / Email to self / Share to family member / Save to Files
**Custom Actions:** "Send to Doctor" (pre-configured email) / "Add to Health App" (iOS HealthKit)

---

## Design System (Designer Deliverable)

The design system is **yours to define**. The following are functional requirements and constraints — not visual prescriptions.

### What You Need to Define
- Color palette (brand colors, semantic colors, status colors)
- Typography scale (font family, sizes, weights)
- Spacing and layout grid
- Component library (buttons, cards, inputs, badges, etc.)
- Iconography style
- Motion and animation language
- Dark mode (if applicable)

### Functional Requirements

**4 severity levels must be visually distinct from each other:**

| Level | Meaning | Must Convey |
|-------|---------|-------------|
| Normal | Everything is fine | Calm, reassuring |
| Caution | Something to be aware of | Mild concern, not urgent |
| Urgent | Action recommended soon | Clear importance, time-sensitive |
| Critical | Immediate action needed | Emergency, cannot be missed |

**Visual hierarchy needs:**
- Primary actions must be clearly distinguishable from secondary and tertiary actions
- Destructive actions (delete, remove) must be visually distinct from standard actions
- Unread/new states must be clearly differentiated from read/acknowledged states
- Data comparison (current vs. baseline) needs clear visual treatment

### Constraints

**Accessibility (non-negotiable):**
- WCAG AA minimum contrast (4.5:1 for text, 3:1 for large text)
- Status must never rely on color alone — always pair with icon, text, or pattern
- Minimum 48x48dp touch targets on all tappable elements
- Dynamic font sizing support (user system preferences)
- All interactive elements must have screen reader labels
- Form labels must be programmatically associated with inputs

**Platform:**
- iOS: Follow Human Interface Guidelines (SF Symbols for icons, safe area insets, native modal drag handles)
- Android: Follow Material Design 3 conventions (FABs, bottom sheets, system back button)

**User context:**
- Primary users are 30-55 year old adults, but they may hand the phone to elderly parents (70+) — consider readability
- Critical alerts may be viewed in high-stress moments — design for quick scanning and large tap targets
- The app will be used in varied lighting conditions (bedside at night, outdoors)

---

**Total Screens:** 35
**MVP 1:** 20 screens — Solo Monitoring (design first)
**MVP 2:** 8 screens — Family, Multi-Member & Clinical
**MVP 3:** 7 screens — Native & Offline
