# CardiTrack - Mobile App Screen Specifications

## Project Overview

**Product:** CardiTrack - Remote health monitoring for elderly family members
**Platform:** iOS (iPhone 12+) & Android (10+)
**Orientation:** Portrait primary, landscape supported
**Target Users:** Family caregivers across the US & EU monitoring elderly relatives' wearable health data
**Document Version:** 3.0
**Last Updated:** February 24, 2026

---

## Release Strategy

68 screens across 4 MVPs (counting each state as a screen). Each MVP is a fully functional, shippable release.

| Release | Screens | Theme | User Gets |
|---------|---------|-------|-----------|
| **MVP 1** | 37 | Core Monitoring | Sign up, connect and manage device (fitbit), monitor one to n parent(s), CardiMember profile, device management, view dashboard, receive and manage all alert types, health data export (PDF, CSV, FHIR R4) |
| **MVP 2** | 4 | Management & Settings | Trend charts, notification preferences, personal subscription (Basic & Complete Care), health data export adds HL7, connect and manage device (garmin) |
| **MVP 3** | 14 | Family & Multi-Member | Invite family, share notes, scan test results with CardiTrack medical insights, export data in LOINC/CCD |
| **MVP 4** | 13 | Native & Offline | Biometric setup and login, offline support, push notification actions, home screen widget, native sharing, export data in SNOMED CT |

---

## Screen Index

| ID | Screen | Release | Variations |
|----|--------|---------|------------|
| M1-01 | Splash Screen | MVP 1 | 2 (a–b) |
| M1-02 | Welcome / Landing | MVP 1 | 1 |
| M1-03 | Sign Up | MVP 1 | 4 (a–d) |
| M1-04 | Add First CardiMember | MVP 1 | 3 (a–c) |
| M1-05 | Device Connection - Selection | MVP 1 | 1 |
| M1-06 | Device Connection - OAuth | MVP 1 | 3 (a–c) |
| M1-07 | Device Connection - Success | MVP 1 | 3 (a–c) |
| M1-08 | Baseline Learning Info | MVP 1 | 1 |
| M1-09 | Main Dashboard | MVP 1 | 5 (a–e) |
| M1-10 | Alerts List | MVP 1 | 4 (a–d) |
| M1-11 | Alert Detail - Activity | MVP 1 | 1 |
| M1-12 | Alert Detail - Critical | MVP 1 | 1 |
| M1-13 | CardiMember Detail | MVP 1 | 1 |
| M1-14 | Edit CardiMember | MVP 1 | 1 |
| M1-15 | Device Management | MVP 1 | 1 |
| M1-16 | Alert Detail - Heart Rate | MVP 1 | 1 |
| M2-01 | Settings Main | MVP 2 | 1 |
| M2-02 | Subscription Management | MVP 2 | 1 |
| M2-03 | Trend Charts | MVP 2 | 1 |
| M2-04 | Notification Settings | MVP 2 | 1 |
| M1-17 | Health Data Export | MVP 1 | 4 (a–d) |
| M3-01 | Family Members List | MVP 3 | 1 |
| M3-02 | Invite Family Modal | MVP 3 | 1 |
| M3-03 | Multi-Member Dashboard | MVP 3 | 2 (a–b) |
| M3-04 | Shared Notes Feed | MVP 3 | 1 |
| M3-05 | Add / Edit Note | MVP 3 | 1 |
| M3-06 | Test Results Scanner | MVP 3 | 4 (a–d) |
| M3-07 | Test Results Detail | MVP 3 | 4 (a–d) |
| M4-01 | Biometric Setup | MVP 4 | 1 |
| M4-02 | Biometric Login | MVP 4 | 1 |
| M4-03 | Offline Mode Indicator | MVP 4 | 2 (a–b) |
| M4-04 | Offline Data Cache Settings | MVP 4 | 1 |
| M4-05 | Push Notifications | MVP 4 | 4 (a–d) |
| M4-06 | Home Screen Widget | MVP 4 | 3 (a–c) |
| M4-07 | Share Sheet Integration | MVP 4 | 1 |

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
      [Call/SMS] [M1-10 Alerts] [M2-03 Trends]
                     │
                ┌────┴────┐
                ▼         ▼
           [M1-11]    [M1-12]
           Activity   Critical
                │         │
                ▼         ▼
           [Acknowledge] [Call Now]
```

### Flow 3: Settings & Management (MVP 2)

```
[Tab: Settings] → [M2-01 Settings Main]
                         │
               ┌─────────┼──────────┬──────────┐
               ▼         ▼          ▼          ▼
         [M2-02 Sub] [M2-04 Notif] [M1-13 Detail] [M1-15 Devices]
                                        │
                                        ▼
                                   [M1-14 Edit]
```

### Flow 3b: Data Export (MVP 1)

```
[M2-03 Trends] → Export icon → [M1-17 Health Data Export]
[M2-01 Settings] → "Export Health Data" → [M1-17 Health Data Export]
[M1-13 Detail] → "Export Data" → [M1-17 Health Data Export]
                                        │
                                   ┌────┴────┐
                                   ▼         ▼
                              [Save/Share] [Email]
```

### Flow 4: Family Collaboration (MVP 3)

```
[Tab: Family] → [M3-01 Family List]
                       │
                 ┌─────┴─────┐
                 ▼           ▼
           [M3-02 Invite] [M3-04 Notes]
                               │
                               ▼
                          [M3-05 Add Note]
```

### Flow 5: Multi-Member (MVP 3)

```
[M3-03 Multi-Dashboard] → tap member → [M1-09 Single Dashboard]
```

### Flow 6: Test Results (MVP 3)

```
[M3-06 Scanner] → Camera/Upload → OCR Processing
                                        │
                                        ▼
                                  [M3-07 Results Detail]
                                        │
                              ┌─────────┼──────────┐
                              ▼         ▼          ▼
                         [CardiTrack Insights] [Export]  [Share]
                                        │
                                        ▼
                                  [M1-17 Export]
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

## MVP 1 — Core Monitoring (37 screens)

A single user can sign up, add one or more CardiMembers, connect devices, manage CardiMember profiles, view the health dashboard, receive and manage all alert types, and export health data in PDF, CSV, or FHIR R4 format. This is the essential monitoring loop — everything needed for the app to be useful from day one.

---

### M1-01: Splash Screen
**User Story:** 1.1-1.3 Onboarding
**Entry:** App launch
**Exit:** → M1-02 Welcome (first launch) | → M1-09 Dashboard (returning user) | → M4-02 Biometric Login (MVP 4, if enabled)

**Duration:** 2-3 seconds while app initializes

**Layout:**
- Full-screen gradient background (CardiTrack brand colors)
- Large CardiTrack logo (centered)
- App name beneath logo
- Loading spinner (bottom third)
- Version number (bottom, small text)

**States:**
- **M1-01a — Default:** Logo + spinner animation
- **M1-01b — Error:** If initialization fails → "Hmm, something didn't work. Tap to try again." with retry button

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
| 1 | Happy elderly person with smartwatch | "Know They're Okay" | "Stay close to the people you love — even from far away" |
| 2 | Phone showing health dashboard | "Their Watch, Your Peace of Mind" | "Connects with Fitbit, Apple Watch, Garmin & more" |
| 3 | Family members on phones | "Care Together" | "Share the watch with your siblings — you're not in this alone" |

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
- **M1-03a — Default:** Empty form
- **M1-03b — Validating:** Inline error messages appear beneath invalid fields
- **M1-03c — Loading:** Button shows spinner, form disabled
- **M1-03d — Error:** Error banner at top (e.g., "Email already registered")

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
- Text: "Who would you like to look after?"
- Subtext: "Tell us about your loved one — we'll take it from there"

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
- Text: "[Name] will know you're looking out for them and can give their okay"

**CTA:**
- Primary button: "Continue"
- Text link: "Skip for Now"

**States:**
- **M1-04a — Default:** Empty form with photo placeholder
- **M1-04b — Photo added:** Shows uploaded image in circle
- **M1-04c — Loading:** Button shows spinner on submit

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
- Text: "What does [Name] wear?"
- Subtext: "We'll connect with their device to keep you in the loop"

**Device Grid (2 columns on phone, 3 on tablet):**

Each device card:
- Rounded frame with shadow
- Device logo (medium)
- Device name (bold)
- Badge: "Supported" (checkmark) for active devices; "Coming Soon" (clock) for future devices
- Future device cards are visible but non-tappable (greyed out, no interaction)
- Entire active card is tappable, highlights on selection

**Supported Devices (MVP 1 — Fitbit only; remaining devices shown as Coming Soon):**

| Device | Models | MVP Availability |
|--------|--------|-----------------|
| Fitbit | Charge, Versa, Sense series | **MVP 1** |
| Garmin | Venu, Forerunner, etc. | MVP 2 |
| Apple Watch | Series 4+ | Coming Soon |
| Samsung Galaxy Watch | All models | Coming Soon |
| Withings | ScanWatch, Move | Coming Soon |
| Other / Manual Entry | Limited features | Coming Soon |

**Bottom:**
- Link: "Don't see their device? We can help"

**Interactions:**
- Single selection — tap to select, tap again to deselect
- Only Fitbit cards are selectable in MVP 1; others are greyed out with "Coming Soon" badge
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
- Label: "To look after [Name], CardiTrack needs:"
- Each permission in its own row:

| Icon | Permission | Info Tooltip |
|------|-----------|-------------|
| Heart | Heart Rate Data | "So we can spot if something's off" |
| Shoe | Activity & Steps | "To make sure they're staying active" |
| Moon | Sleep Data | "To know they're resting well" |

- Each row has an (i) info button that shows the tooltip on tap

**Privacy Notice:**
- Card with light background
- Lock icon + text: "Your family's health data stays private — always. We never sell or share it."

**CTA:**
- Primary button: "Authorize [Device Name]"
  - Tap opens device's OAuth login in a browser/webview
- Text link: "Cancel"

**States:**
- **M1-06a — Default:** Permission list visible
- **M1-06b — Authorizing:** Loading overlay with "Connecting to [Name]'s [Device]..." message
- **M1-06c — Error:** "We couldn't connect — let's try that again" with retry button

---

### M1-07: Device Connection - Success
**User Story:** 1.3 Connection Success
**Entry:** ← M1-06 OAuth (authorization complete)
**Exit:** → M1-08 Baseline Info ("Continue to Dashboard") | → M1-05 Device Selection ("Add Another Device")

**Animation:**
- Animated checkmark (plays once on entry)

**Success Message:**
- Heading: "You're all set!"
- Text: "[Name]'s [Device] is now connected"
- Subtext: "We're pulling in their latest data — just a moment"

**Data Preview Card:**
- Title: "Latest Data"
- Rows:
  - Steps Today: 4,250
  - Last Synced: Just now
  - Heart Rate: 72 bpm

**Options:**
- Outlined button: "+ Add Another Device"
- Helper text: "Does [Name] have another device? The more data, the better we can watch over them"

**CTA:**
- Primary button: "Continue to Dashboard"

**States:**
- **M1-07a — Syncing:** Preview card shows shimmer/skeleton loading
- **M1-07b — Synced:** Preview card shows real data
- **M1-07c — Partial sync:** Some values show, others show "Syncing..."

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
- Heading: "Getting to Know [Name]"
- Body: "Over the next 30 days, CardiTrack will learn what a normal day looks like for [Name]:"
- Bullet list:
  - "When they usually wake up and go to sleep"
  - "How active they are day to day"
  - "What their resting heart rate looks like"

**Progress:**
- Progress bar: "Day 1 of 30"
- Label: "3% Complete"

**Options Card:**
- Toggle switch: "Keep me posted while CardiTrack is learning"
- Description: "You'll get basic alerts right away (like heart rate over 100)"

**CTA:**
- Primary button: "Go to Dashboard"

**MVP 3 addition:** Text link "Invite Family Members First" → M3-02

---

### M1-09: Main Dashboard (Single CardiMember)
**User Story:** 2.1 Daily Health Overview
**Entry:** Tab bar (Home) | ← M1-08 Baseline Info (first time)
**Exit:** → M1-10 Alerts List | → M2-03 Trend Charts | → M1-13 CardiMember Detail | → Phone call / SMS

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
| Normal | "[Name] is doing well" | Checkmark |
| Caution | "Something looks a little different" | Warning triangle |
| Urgent | "You should check in" | Lightning bolt |
| Critical | "Reach out to [Name] now" | Siren |

- Last synced: "Updated 10 minutes ago"
- Tap sync icon for manual refresh

**Quick Actions Row (3 horizontal buttons):**
- "Call [Name]" (phone icon) → initiates phone call
- "Send Message" (SMS icon) → opens SMS
- "View Details" (chart icon) → navigates to M1-13

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
- Button: "View Trends & History" → M2-03

**Interactions:**
- Pull-to-refresh triggers data sync
- Swipe left on metric card → see detail view
- Long-press on photo → change photo option

**States:**
- **M1-09a — Loading:** Skeleton/shimmer cards
- **M1-09b — Normal:** Full data displayed
- **M1-09c — Stale data:** Banner: "Last update was X hours ago — pull down to check in"
- **M1-09d — No device connected:** Prompt card: "Connect [Name]'s device so CardiTrack can start watching over them" → M1-05
- **M1-09e — Baseline learning:** Shows progress bar instead of "% of normal" comparisons

**MVP 3 change:** When user has multiple CardiMembers, Home tab shows M3-03 instead

---

### M1-10: Alerts List
**User Story:** 3.1 Alert Management | 3.3 Alert Acknowledgment & Notes
**Entry:** Tab bar (Alerts) | ← M1-09 Dashboard (Recent Alerts)
**Exit:** → M1-11 Alert Detail (Activity) | → M1-12 Alert Detail (Critical) | → Phone call

**Header:**
- Title: "Alerts"
- Filter icon (funnel)
- Settings icon (gear) → M2-04 Notification Settings

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
  - Preview text (2 lines max): "Dad hasn't moved this morning. He usually wakes up around..."
- Bottom row:
  - Status label: "New" / "Acknowledged" / "Resolved"
  - Quick action icons: Call (phone) | Acknowledge (checkmark) | Expand (chevron)

**Swipe Actions:**
- Swipe right → "Acknowledge"
- Swipe left → "Call"

**Bottom:**
- Link: "View Archived Alerts"

**States:**
- **M1-10a — Default:** Grouped alert list
- **M1-10b — Empty:** Large bell icon (muted) + "Nothing to worry about" + "CardiTrack is keeping an eye on things — we'll let you know if anything comes up"
- **M1-10c — Filtered empty:** "No alerts match this filter"
- **M1-10d — Loading:** Skeleton cards

Heart rate alerts tap → M1-16

---

### M1-11: Alert Detail - Activity
**User Story:** 11.1 Activity Decline | 3.3 Alert Acknowledgment & Notes
**Entry:** ← M1-10 Alerts List (tap alert card)
**Exit:** ← M1-10 Alerts List (back) | → Phone call | → SMS | → M2-03 Trend Charts

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
- Large readable text: "Dad hasn't been as active as usual lately"

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
- "Here's what might be going on:"
  - They could be feeling under the weather
  - They might be in pain or uncomfortable
  - They may be feeling low or tired

**Recommended Actions (full-width button list):**
1. "Give Dad a Call" (primary, phone icon)
2. "Send a Quick Message" (secondary, SMS icon)
3. "Book a Doctor Visit" (secondary, calendar icon)

**More Options (collapsible):**
- "Adjust Baseline" (if this is a new normal)
- "Add Note About This Alert"
- "Share with Family"

**Acknowledgment Section:**
- If unread: Button "Mark as Acknowledged"
- If acknowledged: "Acknowledged by Sarah, 30 min ago" + any notes

**Bottom:**
- Button: "View Detailed Activity Data" → M2-03

---

### M1-12: Alert Detail - Critical (No Movement)
**User Story:** 11.3 No Morning Activity | 3.3 Alert Acknowledgment & Notes
**Entry:** ← M1-10 Alerts List | Push notification (direct)
**Exit:** ← M1-10 Alerts List (back) | → Phone call | → Note input

This is the most safety-critical screen in the app. Design for urgency and immediate action.

**Alert Header:**
- Full-width critical severity banner (pulsing animation)
- Large siren icon
- Title: "We haven't seen Dad move today"
- CardiMember photo + name
- Timestamp

**Urgent Message Card (thick border, critical severity):**
- Large text: "Dad hasn't moved this morning"
- Details:
  - "He usually wakes up around 7:00 AM"
  - "It's now 11:00 AM"
  - "That's 4 hours with no movement"

**Last Known Activity Card:**
- "The last time we saw Dad move:"
- "Yesterday at 10:30 PM"
- "Bedroom area (based on his device)"

**Immediate Actions (large, prominent buttons):**
1. **"CALL NOW"** — critical severity, oversized, phone icon, one-tap to dial, shows phone number
2. **"I'M ON MY WAY"** — urgent severity, large — updates status and lets your family know

**Dismissal Option:**
- Button: "It's okay — he told me he'd sleep in"
  - Opens a note field for context
  - Dismisses alert with explanation logged

**Family Notification Card:**
- "Your family has been notified too:"
  - Sarah (via SMS) — timestamp
  - John (via Push) — timestamp

**Event Timeline:**
- Vertical timeline:
  - 10:30 PM — Last movement
  - 7:00 AM — Expected wake time
  - 9:00 AM — Alert threshold reached
  - 11:30 AM — You were notified

---

### M1-13: CardiMember Detail
**User Story:** 1.4 CardiMember Profile Management
**Entry:** ← M1-09 Dashboard ("View Details") | ← M2-01 Settings ("Manage CardiMembers")
**Exit:** ← Previous screen (back) | → M1-14 Edit CardiMember | → M1-09 Dashboard | → M1-10 Alerts

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
- "Manage Devices" → M1-15

**Danger Zone (separated):**
- "Pause Monitoring" button (warning treatment)
- "Remove CardiMember" button (destructive treatment)

---

### M1-14: Edit CardiMember
**User Story:** 1.4 CardiMember Profile Management
**Entry:** ← M1-13 CardiMember Detail (edit button)
**Exit:** ← M1-13 CardiMember Detail (cancel or save)

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

### M1-15: Device Management
**User Story:** 6.2 Devices
**Entry:** ← M2-01 Settings ("Connected Devices") | ← M1-13 CardiMember Detail ("Manage Devices")
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
- "Having trouble?"
  - Make sure Bluetooth is on
  - Try reconnecting the device
  - We're here to help — contact support

---

### M1-16: Alert Detail - Heart Rate
**User Story:** 11.2 Elevated HR
**Entry:** ← M1-10 Alerts List
**Exit:** ← M1-10 Alerts List (back) | → Phone call | → M2-03 Trend Charts

**Alert Header:**
- Urgent severity banner
- Lightning bolt icon
- Title: "Elevated Heart Rate Alert"
- CardiMember photo + name + timestamp

**Description:**
- "Mom's heart rate has been running higher than usual for the past 3 days"

**Chart:**
- 7-day heart rate chart
- Shaded normal range (68-75 bpm)
- Elevated portion highlighted with urgent severity treatment

**Comparison Grid:**

| Current | Normal | Difference |
|---------|--------|-----------|
| 88 bpm | 68 bpm | +29% above baseline |

**Context Card:**
- "Here's what might be going on:"
  - She could be fighting off an illness
  - She might be feeling stressed or anxious
  - She may not be drinking enough water
  - It could be a side effect of her medication

**Recommended Actions:**
1. "Suggest a Doctor Visit" (primary, urgent treatment)
2. "Keep watching for a couple more days" (secondary)
3. "Call Mom to ask how she's feeling" (secondary)

**Medical History (collapsible):**
- "Related Health Info"
- Shows medications, conditions from CardiMember profile

---

### M1-17: Health Data Export
**User Story:** 6.3 Health Data Export
**Entry:** ← M2-03 Trend Charts (Export icon) | ← M2-01 Settings ("Export Health Data") | ← M1-13 CardiMember Detail ("Export Data")
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

| Format | Description | Use Case | Available |
|--------|-------------|----------|-----------|
| PDF Report | Human-readable summary with charts | Sharing with family or personal records | **MVP 1** |
| CSV | Raw data spreadsheet | Personal analysis | **MVP 1** |
| FHIR (R4) | Fast Healthcare Interoperability Resources | Modern EHR integration, patient portals | **MVP 1** |
| HL7 v2 | Health Level Seven messaging format | Hospital system integration | MVP 2 |

**MVP 2 addition:** HL7 v2 format added to this screen — no new screen created

**MVP 3 addition:** LOINC and CCD formats added (see M3-07)

**MVP 4 addition:** SNOMED CT format added (see M3-07)

**Format Info (expandable per format):**
- Tap info icon next to FHIR/HL7 → explains format, typical recipients (hospitals, clinics, patient portals)

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
- **M1-17a — Default:** Format and date selection
- **M1-17b — Generating:** Progress bar with cancel option
- **M1-17c — Complete:** Success message with share/save actions
- **M1-17d — Error:** "That didn't work — let's try again" with retry

---

## MVP 2 — Management & Settings (4 screens)

Extends MVP 1 with account management: view trends and historical data, configure notification preferences, handle personal subscription (Basic & Complete Care), and add HL7 v2 as an additional export format in M1-17. Garmin device support added.

**Prerequisite:** MVP 1 must be complete. MVP 2 adds settings and management screens that supplement the core monitoring loop.

> **Export note:** M1-17 (Health Data Export) ships in MVP 1 with PDF, CSV, and FHIR R4. MVP 2 updates M1-17 to add HL7 v2 to the format picker — no new screen is created.

---

### M2-01: Settings Main
**User Story:** 6.1, 6.2 Settings
**Entry:** Tab bar (Settings) | Flyout menu
**Exit:** → M2-02 Subscription | → M2-04 Notification Settings | → M1-13 CardiMember Detail | → M1-15 Device Management | → M1-17 Health Data Export

**User Profile Section (top card):**
- Profile photo (large, tappable to edit)
- Name: "[User Name]"
- Email: "[user@email.com]"
- Edit button (pencil icon)

**Settings Groups (grouped list):**

**Account**
- My Profile →
- Subscription & Billing → (badge: current plan name)
- Family & Sharing → (MVP 3)

**CardiMembers**
- Manage CardiMembers →
- Connected Devices → M1-15
- Export Health Data → M1-17

**Health Records (MVP 3)**
- Scan Test Results → M3-06

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

**MVP 3 addition:** Family & Sharing → M3-01

---

### M2-02: Subscription Management
**User Story:** 6.1 Subscription
**Entry:** ← M2-01 Settings ("Subscription & Billing")
**Exit:** ← M2-01 Settings (back) | → Payment method change | → Plan change

> **Scope note:** MVP 2 covers personal tiers only (Basic and Complete Care). The Guardian Plus business tier is excluded from MVP and will be addressed in a dedicated business account flow post-MVP.

**Current Plan Card (gradient background):**
- Badge: "COMPLETE CARE"
- Price: "$15/month"
- Renewal date: "Renews Feb 10, 2026"
- Button: "Manage Subscription"

**Included Features (checklist):**
- Up to 3 CardiMembers
- Advanced ML Alerts
- Family Sharing
- 90-day data retention
- Priority support

**Usage Section:**
- Progress bars with labels:
  - CardiMembers: 2 of 3
  - Data retention: 45 days of 90

**Plan Comparison (horizontal swipeable cards):**
- 2 plan cards (Basic and Complete Care), swipe to compare
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

### M2-03: Trend Charts
**User Story:** 2.3 Historical Data
**Entry:** ← M1-09 Dashboard ("View Trends") | ← M1-11 Alert Detail ("View Detailed Data")
**Exit:** ← Previous screen (back) | → M1-17 Health Data Export

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

### M2-04: Notification Settings
**User Story:** 3.2 Alert Preferences
**Entry:** ← M2-01 Settings | ← M1-10 Alerts List (gear icon)
**Exit:** ← Previous screen (back)

**CardiMember Selector (if multiple members):**
- Dropdown: "Settings for: [Dad]"

**Alert Type Groups (each with enable toggle):**

**Activity Alerts**
- Toggle: enabled/disabled
- Sensitivity slider: Low | Medium | High
- Description: "Let me know if they're moving around less than usual"

**Heart Rate Alerts**
- Toggle: enabled/disabled
- Sensitivity slider: Low | Medium | High
- Description: "Let me know if their heart rate seems higher than usual"

**Sleep Alerts**
- Toggle: enabled/disabled
- Checkboxes:
  - Poor sleep quality
  - Unusual sleep patterns

**Pattern Break Alerts**
- Toggle: always on (cannot disable)
- Label: "Always on — this is how CardiTrack catches emergencies"

**Notification Channels (per alert type):**
- Multi-select chips: [Email] [SMS] [Push] [All]

**Quiet Hours (collapsible):**
- Toggle: "Enable Quiet Hours"
- Time pickers: From 10:00 PM → To 7:00 AM
- Exception toggle: "Still wake me for emergencies"

**Family Routing (MVP 3):**
- "Also let these family members know:"
- Checkboxes with severity chips:
  - Sarah Johnson — [High Severity] [Critical]
  - John Doe — [Critical Only]

**Test Section:**
- "Send Test Push Notification" button
- "Send Test Email" button
- "Send Test SMS" button

---

## MVP 3 — Family & Multi-Member (14 screens)

Adds family collaboration: invite siblings to share caregiving, shared notes, manage multiple CardiMembers from a dedicated dashboard, test results scanning with CardiTrack medical insights, and expanded data export formats (LOINC, CCD).

**Prerequisite:** MVP 2 must be complete. MVP 3 extends existing screens (noted as "MVP 3 addition/change" in MVP 1–2 specs).

---

### M3-01: Family Members List
**User Story:** 4.1 Family Management
**Entry:** Tab bar (Family) | ← M2-01 Settings ("Family & Sharing")
**Exit:** → M3-02 Invite Modal | → Role management | → M3-04 Shared Notes

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
- "+" icon → opens M3-02 Invite Modal

---

### M3-02: Invite Family Modal
**User Story:** 4.1 Inviting Members
**Entry:** ← M3-01 Family List ("+" or "Invite") | ← M1-08 Baseline Info ("Invite Family Members First")
**Exit:** ← M3-01 Family List (close/cancel) | → Success confirmation

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
- Placeholder: "Hi Sarah, I'm using CardiTrack to keep an eye on Dad — want to help?"

**CTA:**
- Primary button: "Send Invitation"
- Text link: "Cancel"

---

### M3-03: Multi-Member Dashboard
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
- **M3-03a — Default:** Member cards listed
- **M3-03b — Empty:** Illustration + "No one here yet" + "Add someone you'd like to look after" button

---

### M3-04: Shared Notes Feed
**User Story:** 4.2 Coordination
**Entry:** ← M3-01 Family List | Tab bar (Family) → Notes sub-tab
**Exit:** → M3-05 Add Note | ← Previous screen (back)

**Header:**
- Back button
- Title: "Family Notes"
- Filter dropdown: "All Notes"

**Add Note Input (top):**
- User photo + text input: "Add a note for the family..."
- Tap → opens M3-05 full composer

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

### M3-05: Add / Edit Note
**User Story:** 4.2 Shared Notes
**Entry:** ← M3-04 Notes Feed (tap input or "+" button)
**Exit:** ← M3-04 Notes Feed (cancel or post)

**Presentation:** Full-screen modal

**Header:**
- Cancel button
- Title: "New Note"
- Post button (enabled when content exists)

**Note Input:**
- Multi-line text editor (expands with content)
- Placeholder: "How's Dad doing? Let the family know..."
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

### M3-06: Test Results Scanner
**User Story:** 12.1 Lab Results Capture
**Entry:** ← M2-01 Settings ("Scan Test Results") | ← M1-13 CardiMember Detail ("Add Test Results") | Tab bar (dedicated entry point)
**Exit:** → M3-07 Test Results Detail (scan complete) | ← Previous screen (cancel)

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
- Blurry image: "That came out a bit blurry — try holding steady and retake"
- Unreadable: "We're having trouble reading this — try better lighting or upload a PDF instead"
- Partial read: "We got most of it, but a few values need your help — you can fix them on the next screen"

**States:**
- **M3-06a — Default:** Capture options
- **M3-06b — Camera active:** Viewfinder with guide overlay
- **M3-06c — Processing:** Analysis progress animation
- **M3-06d — Error:** Error message with retry/retake options

---

### M3-07: Test Results Detail
**User Story:** 12.2 Medical Insights from Lab Results
**Entry:** ← M3-06 Test Results Scanner (analysis complete) | ← M1-13 CardiMember Detail ("View Test Results")
**Exit:** ← Previous screen (back) | → M1-17 Health Data Export | → Share

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

**CardiTrack Insights Card:**
- Lightbulb icon + "CardiTrack Insights"
- Disclaimer banner: "These observations are here to help — but always talk to a doctor before making health decisions."
- CardiTrack observations:
  - "Dad's Hemoglobin A1c is a bit high — this sometimes points to pre-diabetes"
  - "Good news — cholesterol levels look normal"
  - "His Vitamin D is low — worth mentioning to his doctor"
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
- "Export Results" → M1-17 Health Data Export
- "Share with Doctor" → pre-formatted email/share
- "Add to Health Record" → saves to CardiMember profile

**Data Standards:**
- Results are encoded using:
  - **LOINC** — standardized lab test codes (e.g., Hemoglobin A1c = LOINC 4548-4) *(MVP 3)*
  - **CCD** — Continuity of Care Document for structured clinical summaries *(MVP 3)*
  - **SNOMED CT** — clinical terminology for conditions and findings *(MVP 4 addition)*
- These formats are available in M1-17 Health Data Export as additional export options

**States:**
- **M3-07a — Default:** Parsed results with insights
- **M3-07b — Editing:** Inline editing mode for value corrections
- **M3-07c — No previous results:** Trend section hidden
- **M3-07d — Loading insights:** Skeleton loading for CardiTrack insights section

---

## MVP 4 — Native & Offline (13 screens)

Adds platform-native polish: biometric security (setup and login), offline data access with sync queue, rich push notifications with inline actions, home screen widgets for at-a-glance monitoring, native share sheet for exporting data to doctors or family, and SNOMED CT health data export.

**Prerequisite:** MVP 3 must be complete.

---

### M4-01: Biometric Setup
**User Story:** 10.2 Biometric Login
**Entry:** ← M2-01 Settings (Security section)
**Exit:** ← M2-01 Settings | → Skip ("Set Up Later")

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

### M4-02: Biometric Login
**User Story:** 10.2 Biometric Auth
**Entry:** ← M1-01 Splash (when biometric enabled via M4-01)
**Exit:** → M1-09 Dashboard (success) | → Password fallback

Replaces password entry on app launch when biometric is enabled.

- CardiTrack logo + user name/photo
- Platform biometric prompt (Face ID on iOS, fingerprint on Android)
- "Scan to unlock" label
- Fallback: "Use Password" link → password entry field
- Configurable biometric requirements: app launch, viewing alerts, acknowledging alerts, changing settings

---

### M4-03: Offline Mode Indicator
**User Story:** 10.1 Offline Support
**Entry:** Automatic — appears when device loses connectivity
**Exit:** Automatic — disappears when connection restored

**States:**
- **M4-03a — Offline:** Persistent banner + read-only mode + alert queue
- **M4-03b — Connection Restored:** Toast + sync animation + success confirmation

**M4-03a — Offline**

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

**M4-03b — Connection Restored:**
- Toast: "Back online!"
- Syncing animation with progress
- Success message: "All data synced"

---

### M4-04: Offline Data Cache Settings
**User Story:** 10.1 Cache Management
**Entry:** ← M2-01 Settings
**Exit:** ← M2-01 Settings (back)

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

### M4-05: Push Notifications
**User Story:** 5.1

Designs for system-level notification UI.

**States:**
- **M4-05a — Lock Screen (compact):** App icon + "[Name] - Critical Alert" + body preview + timestamp
- **M4-05b — Lock Screen (expanded on long press):** Full alert text + action buttons: "Call" / "View" / "Acknowledge"
- **M4-05c — In-App Banner:** Slides from top, shows summary, tap to navigate, swipe up to dismiss
- **M4-05d — Notification Center:** Grouped by CardiMember with expandable lists + app badge count

---

### M4-06: Home Screen Widget
**User Story:** 5.2

**States:**
- **M4-06a — Small Widget (2x2):** Logo + CardiMember photo + status indicator + name + last synced
- **M4-06b — Medium Widget (4x2):** 2 CardiMembers side-by-side with photo, name, status, key metric
- **M4-06c — Large Widget (4x4, iOS):** Up to 4 CardiMembers with mini dashboards (photo, name, status, 3 metrics, alert badge)

**Configuration:** Long-press → select CardiMembers, choose metrics, set update frequency

---

### M4-07: Share Sheet Integration
**User Story:** 5.3 Native Sharing

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

## Asset Inventory

Icons use **SF Symbols** (iOS) and **Material Symbols** (Android) — no custom icon design needed. Third-party logos (Google, Apple, Fitbit, Garmin, Samsung, Withings, Visa) are sourced from vendor brand kits. Items below are the assets that need to be created or sourced.

### MVP 1 — Assets

#### Illustrations (Storyset / Blush or custom)

| # | Asset | Screen | Description |
|---|-------|--------|-------------|
| 1 | Onboarding Slide 1 | M1-02 | Happy elderly person wearing a smartwatch — warm, reassuring tone |
| 2 | Onboarding Slide 2 | M1-02 | Phone showing a health dashboard — conveys "at-a-glance monitoring" |
| 3 | Onboarding Slide 3 | M1-02 | Family members on their phones — conveys shared caregiving |
| 4 | Learning Phase | M1-08 | Brain with gears concept — "getting to know your loved one." Animated (Lottie). Source from LottieFiles marketplace or simplify to static illustration + platform progress animation |
| 5 | Empty Alerts | M1-10 | Muted bell or peaceful scene — "nothing to worry about" feeling |
| 6 | No Device Connected | M1-09 | Prompt to connect a device — friendly nudge, not an error |

**Style guidance:** All 6 illustrations must use the same art style and brand color palette. Choose one Storyset or Blush collection and customize colors for consistency. Tone: warm, caring, approachable — not clinical.

#### Brand Assets (custom — must be unique)

| # | Asset | Used On | Notes |
|---|-------|---------|-------|
| 1 | CardiTrack logo | M1-01 Splash, M1-02 Welcome, M1-06 OAuth, M4-02 Login | Export at multiple sizes: large (splash), small (header), favicon. SVG master. |
| 2 | App icon | Home screen, app stores | Must work at all OS-required sizes. Follow Apple and Google icon guidelines. |

#### Animations

| # | Animation | Screen | Source |
|---|-----------|--------|--------|
| 1 | Success checkmark | M1-07 | LottieFiles — search "success checkmark" (free options available) |
| 2 | Shimmer / skeleton loading | M1-09, M1-10, M2-03 | Open-source library (e.g., Shimmer.Maui). Reusable across all screens. |
| 3 | Critical alert pulse | M1-12 | XAML/CSS animation — opacity + scale loop on severity banner |
| 4 | Learning phase brain/gears | M1-08 | LottieFiles — search "machine learning" or "brain processing." Or use static illustration (#4 above) + platform `ActivityIndicator`. |

#### Third-Party Logos (vendor-provided, no design needed)

| # | Logo | Screen | Source |
|---|------|--------|--------|
| 1 | Google | M1-03 | Google Identity branding guidelines (SVG provided) |
| 2 | Apple | M1-03 | Apple Sign In SDK (renders automatically) |
| 3 | Fitbit | M1-05, M1-06, M1-15 | Fitbit/Google developer brand assets |
| 4 | Apple Watch | M1-05, M1-06, M1-15 | Apple marketing assets (MFi partners) |
| 5 | Garmin | M1-05, M1-06, M1-15 | Garmin Connect developer program |
| 6 | Samsung | M1-05, M1-06, M1-15 | Samsung developer brand kit |
| 7 | Withings | M1-05, M1-06, M1-15 | Withings Health API partner assets |
| 8 | Visa / card brands | M2-02 | Payment SDK (Stripe, etc.) includes card icons |

---

### MVP 2 — Assets

No new custom assets required. Management & Settings screens (M2-01–M2-04) use platform components, existing brand assets, and third-party logos already sourced in MVP 1.

---

### MVP 3 — Assets

#### Illustrations

| # | Asset | Screen | Description |
|---|-------|--------|-------------|
| 7 | Empty Members | M3-03 | "No one here yet" — friendly empty state, same art style as MVP 1 illustrations |

#### Animations

| # | Animation | Screen | Source |
|---|-----------|--------|--------|
| 5 | OCR processing steps | M3-06 | Custom step indicator animation (3 steps with progress). Can be built with XAML or sourced from LottieFiles — search "document scanning" |

#### Icons (platform — no custom design)

All icons in MVP 3 reuse platform icon sets. No new custom icons needed.

---

### MVP 4 — Assets

#### Icons (platform-provided)

| # | Icon | Screen | Source |
|---|------|--------|--------|
| 1 | Face ID | M4-01, M4-02 | iOS: SF Symbol `faceid` (system-provided) |
| 2 | Fingerprint | M4-01, M4-02 | Android: Material Symbol `fingerprint` (system-provided) |
| 3 | Crossed-out signal | M4-03 | SF Symbol `wifi.slash` / Material `signal_wifi_off` |

#### Widget Assets

| # | Asset | Screen | Notes |
|---|-------|--------|-------|
| 1 | Widget backgrounds | M4-06 | Defined by design system. Small (2x2), medium (4x2), large (4x4, iOS only). Must adapt to system light/dark mode. |

No new custom illustrations or animations needed for MVP 3.

---

### Asset Summary

| Category | MVP 1 | MVP 2 | MVP 3 | MVP 4 | Total |
|----------|-------|-------|-------|-------|-------|
| Custom illustrations | 6 | 0 | 1 | 0 | **7** |
| Brand assets (logo + icon) | 2 | 0 | 0 | 0 | **2** |
| Animations (Lottie / XAML) | 4 | 0 | 1 | 0 | **5** |
| Third-party logos | 8 | 0 | 0 | 0 | **8** |
| Widget assets | 0 | 0 | 0 | 1 | **1** |
| **Subtotal** | **20** | **0** | **2** | **1** | **23** |

**Truly custom** (must be designed): **2** — CardiTrack logo and app icon. Everything else can be sourced from Storyset/Blush (illustrations), LottieFiles (animations), vendor brand kits (logos), and platform icon sets (SF Symbols / Material Symbols).

---

## Figma Delivery Requirements

This project uses **Cursor + Figma MCP** to build the UI directly from Figma designs. The following requirements ensure the designs translate accurately into code.

### File Structure

- Organise screens into **named Pages** per MVP: `MVP 1 — Core Monitoring`, `MVP 2 — Management & Settings`, `MVP 3 — Family & Multi-Member`, `MVP 4 — Native & Offline`
- Each screen must live in its own **named Frame**, using the Screen IDs from this document (e.g. `M1-09 Main Dashboard`)
- Group all reusable UI into a dedicated **`Components`** page

### Layout

- Use **Auto Layout** on every frame, section, and component — this is critical for accurate code generation
- Set explicit **spacing tokens** via Auto Layout gap/padding values (do not use manual nudging)
- Define a consistent **8pt grid** for all spacing and sizing

### Design Tokens (Figma Variables)

Set up the following as **Figma Variables** (not hardcoded values):

| Token Type | Examples |
|------------|---------|
| Colors | `color/status/normal`, `color/status/caution`, `color/status/urgent`, `color/status/critical`, `color/brand/primary`, `color/text/primary`, `color/background/card` |
| Typography | `text/heading/large`, `text/body/default`, `text/label/small` |
| Spacing | `space/xs` (4), `space/sm` (8), `space/md` (16), `space/lg` (24), `space/xl` (32) |
| Radius | `radius/card`, `radius/button`, `radius/chip` |

### Components

- Publish all reusable UI as **Figma Components** with variants (e.g. Button: Primary / Secondary / Destructive / Disabled)
- Name components using the pattern: `ComponentName/Variant` (e.g. `AlertCard/Critical`, `Button/Primary`)
- Use **component properties** (text, boolean, instance swap) so variants are machine-readable
- All interactive states must be defined as variants: Default, Hover, Pressed, Disabled, Loading, Error

### Naming Conventions

- All layers must be **named meaningfully** — no `Frame 42`, `Rectangle 7`, or `Group 3`
- Use camelCase or kebab-case consistently (e.g. `statusHeroCard` or `status-hero-card`)
- Hidden layers that are not part of the design must be deleted, not just hidden

### Handoff Checklist (per screen)

Before marking a screen as ready:
- [ ] Screen is in a named frame with the correct Screen ID
- [ ] All layers are named
- [ ] Auto Layout is applied throughout
- [ ] All colors and text styles reference Figma Variables (no hardcoded hex values)
- [ ] Interactive states are defined as component variants
- [ ] Responsive behaviour is defined (iPhone 14 base, with notes for larger screens)
- [ ] All four severity levels (Normal / Caution / Urgent / Critical) are visually tested in context

### Figma Access

- Share the file with **Edit access** so the MCP server can read all component and variable data
- Provide a **personal access token** (Figma → Settings → Security → Personal Access Tokens) for MCP authentication
- Alternatively, use **OAuth login** if using the official Figma remote MCP server

### MCP Configuration (Cursor)

The developer will connect Cursor to Figma using the following MCP config (`~/.cursor/mcp.json`):

```json
{
  "mcpServers": {
    "figma": {
      "command": "npx",
      "args": ["-y", "figma-developer-mcp", "--figma-api-key", "YOUR_TOKEN_HERE", "--stdio"]
    }
  }
}
```

Once connected, screen designs can be referenced directly by Figma frame URL during development — no manual redlines or spec exports needed.

---

**Total Screens:** 68 (counting each state as a screen)
**MVP 1:** 37 screens — Core Monitoring (design first)
**MVP 2:** 4 screens — Management & Settings
**MVP 3:** 14 screens — Family & Multi-Member
**MVP 4:** 13 screens — Native & Offline
