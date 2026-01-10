# CardiTrack MAUI Mobile UI Screen Descriptions

Detailed UI specifications for the .NET MAUI mobile application (iOS and Android).

**Platform:** .NET MAUI (iOS & Android)
**Target Devices:** iPhone 12+, Android 10+
**Orientation:** Portrait (primary), Landscape (supported)
**Document Version:** 1.0
**Last Updated:** January 10, 2026

---

## Table of Contents

1. [Mobile App Architecture](#mobile-app-architecture)
2. [Onboarding Flow](#onboarding-flow)
3. [Dashboard Screens](#dashboard-screens)
4. [Alert Screens](#alert-screens)
5. [Family & Collaboration](#family--collaboration)
6. [Settings & Account](#settings--account)
7. [CardiMember Management](#cardimember-management)
8. [Offline Features](#offline-features)
9. [Native Features](#native-features)
10. [Navigation Patterns](#navigation-patterns)

---

## Mobile App Architecture

### Navigation Structure
- **Shell Navigation:** MAUI Shell for consistent navigation
- **Flyout Menu:** Left slide-out menu for main navigation
- **Tab Bar:** Bottom navigation for primary sections
- **Modal Pages:** For focused tasks (alerts, adding CardiMembers)

### Bottom Tab Bar (Always Visible)
```
[Dashboard] [Alerts] [Family] [Settings]
    🏠         🔔        👥        ⚙️
```

### Flyout Menu Items
- My Dashboard
- CardiMembers (with count badge)
- Alerts (with unread count)
- Family & Sharing
- Subscription
- Settings
- Help & Support
- Sign Out

---

## Onboarding Flow

### Screen 1.1: Splash Screen
**Duration:** 2-3 seconds while app initializes

**Layout:**
- Full-screen gradient background (CardiTrack brand colors)
- Large CardiTrack logo (centered)
- App name beneath logo
- Loading spinner (bottom third)
- Version number (bottom, small)

**MAUI Implementation:**
```xml
ContentPage with:
- AbsoluteLayout for centering
- Image for logo
- ActivityIndicator
- Label for version
```

---

### Screen 1.2: Welcome/Landing Screen
**User Story:** 1.1 - First-Time User Registration

**Layout:**

**Header (20% of screen):**
- CardiTrack logo (top-left, small)
- Skip button (top-right) → "Sign In"

**Hero Section (50% of screen):**
- Carousel view with 3 slides:

  **Slide 1:**
  - Illustration: Happy elderly person with smartwatch
  - Headline: "Peace of Mind for Your Family"
  - Subtext: "Monitor loved ones' health from anywhere"

  **Slide 2:**
  - Illustration: Phone showing health dashboard
  - Headline: "Works with Devices They Own"
  - Subtext: "Fitbit, Apple Watch, Garmin & more"

  **Slide 3:**
  - Illustration: Family members on phones
  - Headline: "Stay Connected as a Family"
  - Subtext: "Share caregiving with siblings"

- Pagination dots (below carousel)

**CTA Section (30% of screen):**
- Primary button: "Start Free 30-Day Trial" (full width, bold)
- Pricing text: "Then $8/month • Cancel anytime"
- Secondary button: "Sign In" (text button, subtle)
- Terms link: "By continuing, you agree to Terms & Privacy"

**MAUI Controls:**
- CarouselView for slides
- Button (style: Primary, Secondary)
- Label with TapGestureRecognizer for links

---

### Screen 1.3: Sign Up Screen
**User Story:** 1.1 - Account Creation

**Layout:**

**Header:**
- Back button (< Back)
- Title: "Create Account"
- Progress: "Step 1 of 4" (top-right)

**Form Section (Scrollable):**

**Email & Password:**
- Label: "Email Address"
- Entry: email keyboard type, autocapitalization off
- Label: "Password"
- Entry: IsPassword=true
- Password strength indicator (bar: red→yellow→green)
  - Weak / Medium / Strong
- Label: "Confirm Password"
- Entry: IsPassword=true

**OR Divider:**
- Horizontal line with "OR" in center

**Social Login Buttons:**
- "Continue with Google" button (white, Google logo)
- "Continue with Apple" button (black, Apple logo)

**Terms Checkbox:**
- CheckBox + Label: "I agree to Terms of Service and Privacy Policy"
- Links are tappable

**CTA:**
- Primary button: "Create Account" (disabled until valid)
- Error label (hidden until error occurs)

**Bottom Link:**
- "Already have an account? Sign In"

**MAUI Implementation:**
```xml
ContentPage with ScrollView containing:
- StackLayout with Entry controls
- ProgressBar for password strength
- CheckBox for terms
- Button with IsEnabled binding
- TapGestureRecognizer for social login
```

**Validation:**
- Real-time email format validation
- Password: minimum 8 chars, 1 uppercase, 1 number
- Matching password confirmation
- Terms must be checked

---

### Screen 1.4: Add First CardiMember
**User Story:** 1.2 - Adding First CardiMember

**Layout:**

**Header:**
- Back button
- Title: "Add CardiMember"
- Progress: "Step 2 of 4"

**Introduction:**
- Icon: 👤
- Text: "Who would you like to monitor?"
- Subtext: "We'll help you set up monitoring in just a few steps"

**Form (Scrollable):**

**Photo Section:**
- Circular image placeholder (80x80 dp)
- "Add Photo" button (centered below)
- Camera icon + Gallery icon (choose source)

**Required Information:**
- Label: "Full Name *"
  - Entry (TextKeyboard)
- Label: "Date of Birth *"
  - DatePicker (shows picker on tap)
  - Display format: "MM/DD/YYYY"
- Label: "Relationship *"
  - Picker (dropdown):
    - Parent
    - Grandparent
    - Spouse
    - Sibling
    - Other

**Optional Information (Collapsible):**
- Expander: "Add More Details (Optional)"
  - Label: "Medical Notes"
    - Editor (multi-line, encrypted icon shown)
    - Character counter: "0/500"
  - Label: "Emergency Contact Name"
    - Entry
  - Label: "Emergency Contact Phone"
    - Entry (PhoneKeyboard)

**Privacy Notice:**
- Frame with info icon
- Text: "🔒 Your parent will be notified and can provide consent"

**CTA:**
- Primary button: "Continue"
- Secondary button (text): "Skip for Now"

**MAUI Controls:**
- Image with TapGestureRecognizer for photo
- MediaPicker API for camera/gallery
- DatePicker
- Picker for dropdown
- Expander for collapsible section
- Editor for multi-line

---

### Screen 1.5: Device Connection - Selection
**User Story:** 1.3 - Device Connection Wizard

**Layout:**

**Header:**
- Back button
- Title: "Connect Device"
- Progress: "Step 3 of 4"

**Introduction:**
- Text: "What device does [Name] use?"
- Subtext: "We support all major fitness trackers"

**Device Grid:**
- CollectionView (2 columns on phone, 3 on tablet)

**Device Card (Repeated):**
- Frame with rounded corners, shadow
- Device logo image (64x64 dp)
- Device name (Label, bold)
- "Supported" badge (green checkmark)
- Tap = Select device

**Supported Devices:**
1. **Fitbit** (Charge, Versa, Sense series)
2. **Apple Watch** (Series 4+)
3. **Garmin** (Venu, Forerunner, etc.)
4. **Samsung Galaxy Watch**
5. **Withings** (ScanWatch, Move)
6. **Other/Manual Entry** (limited features)

**Bottom:**
- Text link: "Don't see your device? Contact support"

**MAUI Implementation:**
```xml
CollectionView with:
- ItemsLayout: GridItemsLayout, Span=2
- ItemTemplate: Frame with Image, Label
- SelectionMode: Single
- SelectionChanged event
```

---

### Screen 1.6: Device Connection - OAuth Permission
**User Story:** 1.3 - OAuth Flow

**Layout:**

**Header:**
- Back button
- Title: "[Device Name] Connection"

**Content (Centered):**

**Device Logo:**
- Large device logo (128x128 dp)
- Connection arrow icon
- CardiTrack logo (128x128 dp)

**Permission List:**
- Label: "CardiTrack needs access to:"
- Frame for each permission:

  **Permission Item:**
  - Icon (left): ❤️
  - Label: "Heart Rate Data"
  - Info button (right): (i)
    - Tap shows tooltip: "Used to detect unusual patterns"

  **Permission Item:**
  - Icon: 👟
  - Label: "Activity & Steps"
  - Info: "To monitor daily movement"

  **Permission Item:**
  - Icon: 😴
  - Label: "Sleep Data"
  - Info: "To spot rest pattern changes"

**Privacy Notice:**
- Frame (light background)
- Text: "🔒 We never sell your data. Your information stays private and secure."

**CTA:**
- Primary button: "Authorize [Device]"
  - Tap → Opens WebView with OAuth flow
- Text button: "Cancel"

**MAUI Implementation:**
- WebAuthenticator API for OAuth
- WebView for embedded auth (fallback)
- Platform-specific OAuth handling

---

### Screen 1.7: Device Connection - Success
**User Story:** 1.3 - Connection Success

**Layout:**

**Animation Section:**
- Lottie animation: Checkmark success (plays once)
- Or: Animated checkmark using MAUI Animations

**Success Message:**
- Heading: "Connected Successfully!"
- Text: "We're syncing [Name]'s data from [Device]"
- Subtext: "This may take a few minutes"

**Data Preview:**
- Frame: "Latest Data"
  - Label: "Steps Today: 4,250"
  - Label: "Last Synced: Just now"
  - Label: "Heart Rate: 72 bpm"

**Options:**
- Button: "+ Add Another Device" (outlined)
- Text: "You can connect multiple devices for [Name]"

**CTA:**
- Primary button: "Continue to Dashboard"

**MAUI Implementation:**
- SkiaSharp or Lottie for animations
- Timer for "syncing" status updates
- Data binding for preview values

---

### Screen 1.8: Baseline Learning Info
**User Story:** Baseline Setup

**Layout:**

**Header:**
- Title: "Learning Phase"
- Progress: "Step 4 of 4"

**Illustration:**
- Animated graphic: Brain with gears (learning)
- Or: Progress circle filling up

**Explanation:**
- Heading: "CardiTrack is Learning [Name]'s Patterns"
- Text: "Over the next 30 days, our AI will learn what's normal for [Name]:"
- Bullet list:
  - "Typical wake/sleep times"
  - "Average daily activity levels"
  - "Resting heart rate baseline"

**Progress:**
- ProgressBar: "Day 1 of 30"
- Label: "3% Complete"

**Options:**
- Frame with toggle:
  - Switch: "Use basic alerts while learning"
  - Text: "Get simple threshold alerts (e.g., heart rate >100)"

**CTA:**
- Primary button: "Go to Dashboard"
- Text button: "Invite Family Members First"

---

### Screen 1.9: Biometric Setup
**User Story:** 10.2 - Biometric Login

**Layout:**

**Header:**
- Skip button (top-right)
- Title: "Secure Your Account"

**Icon:**
- Platform-specific biometric icon (centered, large)
  - iOS: Face ID icon
  - Android: Fingerprint icon

**Explanation:**
- Heading: "Enable [Face ID / Fingerprint]"
- Text: "Quickly and securely access health data"
- Bullet benefits:
  - "Login in seconds"
  - "Extra security layer"
  - "Required for sensitive actions"

**CTA:**
- Primary button: "Enable [Biometric]"
  - Triggers platform biometric enrollment
- Text button: "Set Up Later"

**MAUI Implementation:**
```csharp
// Use platform-specific APIs
#if IOS
BiometricAuthentication.FaceID
#elif ANDROID
BiometricAuthentication.Fingerprint
#endif
```

---

## Dashboard Screens

### Screen 2.1: Main Dashboard (Single CardiMember)
**User Story:** 2.1 - Daily Health Overview

**Layout:**

**Header (Fixed):**
- Greeting: "Good Morning, [User First Name]"
- Notification bell icon (with badge count)
- Refresh icon (pull-to-refresh also works)

**Status Hero Card:**
- Large card with gradient background (status-colored)
- CardiMember photo (circular, 80dp)
- Name and age: "[Name], 78"
- Large status indicator:
  - **Green:** "All Good!" + ✓ icon
  - **Yellow:** "Needs Attention" + ⚠️ icon
  - **Orange:** "Action Recommended" + ⚡ icon
  - **Red:** "Urgent" + 🚨 icon
- Last synced: "Updated 10 minutes ago"
- Sync icon (tap to manual refresh)

**Quick Actions Row:**
- 3 buttons in horizontal stack:
  - "Call [Name]" (phone icon) → Direct dial
  - "Send Message" (SMS icon)
  - "View Details" (chart icon)

**Key Metrics Cards (3 cards):**

**Card 1: Activity**
- Icon: 👟
- Large number: "4,250 steps"
- Comparison bar (visual progress):
  - Filled: Current vs Goal
  - Color: Green (on track) / Yellow (low)
- Text: "85% of normal" (with arrow ↓)
- Mini sparkline chart (last 7 days)

**Card 2: Heart Rate**
- Icon: ❤️
- Large number: "72 bpm"
- Status: "Normal range"
- Range indicator: "68-75 bpm typical"
- Mini sparkline

**Card 3: Sleep**
- Icon: 😴
- Large number: "7.2 hours"
- Quality: "Good" (stars ⭐⭐⭐⭐)
- Text: "Better than average"
- Mini sparkline

**Recent Alerts Section (if any):**
- Heading: "Recent Alerts"
- Alert cards (scrollable horizontal):
  - Each shows: Icon, title, time, status
  - Tap to expand details

**Bottom CTA:**
- Button: "View Trends & History"

**MAUI Implementation:**
```xml
RefreshView (pull to refresh)
ScrollView containing:
- Grid for header
- Frame for status card
- CollectionView (horizontal) for quick actions
- FlexLayout for metric cards
- CarouselView for alerts
```

**Gestures:**
- Pull-to-refresh
- Swipe left on metric card → See details
- Long-press on photo → Change photo

---

### Screen 2.2: Multi-Member Dashboard
**User Story:** 2.2 - Multi-Member View

**Layout:**

**Header:**
- Title: "My CardiMembers"
- Filter icon (top-right) → Opens filter sheet
- "+ Add" button (top-right)

**Filter Bar (Collapsible):**
- Horizontal scroll chips:
  - [All] [Alerts Only] [Good Status]
- Sort button: "Sort by Status ▼"

**CardiMember Cards (Vertical Scroll):**

**Card Layout:**
- Frame with shadow, rounded corners
- Horizontal layout:

  **Left Section (30%):**
  - Circular photo (64dp)
  - Status badge (overlaid on photo)

  **Middle Section (50%):**
  - Name: "[Name]" (bold, large)
  - Age & relationship: "78 • Dad"
  - Status text: "All good" or alert summary
  - Last synced: "10 min ago"

  **Right Section (20%):**
  - Chevron (>)
  - Alert count badge (if any)

**Swipe Actions:**
- Swipe left: Reveals "Call" button (green)
- Swipe right: Reveals "Details" button (blue)

**Empty State:**
- Illustration: Elderly person with heart
- Text: "No CardiMembers Yet"
- Button: "Add Your First CardiMember"

**Floating Action Button (FAB):**
- Bottom-right corner
- "+" icon
- Tap → Add CardiMember flow

**MAUI Implementation:**
```xml
CollectionView with:
- ItemTemplate: SwipeView with Frame
- EmptyView: Custom template
- RefreshView parent
- FAB using platform-specific positioning
```

---

### Screen 2.3: Trend Charts Screen
**User Story:** 2.3 - Historical Data

**Layout:**

**Header:**
- Back button
- Title: "[Name]'s Trends"
- Export icon (share sheet)

**Time Range Selector:**
- Segmented control:
  ```
  [7D] [30D] [90D] [Custom]
  ```
- Custom date range picker (modal)

**Metric Tabs:**
- Horizontal scroll tabs:
  ```
  [Activity] [Heart Rate] [Sleep] [All]
  ```

**Chart Area:**

**Chart Container:**
- Scrollable chart view (Syncfusion/LiveCharts)
- Line chart with:
  - X-axis: Dates
  - Y-axis: Metric values
  - Baseline range (shaded green area)
  - Data line (blue)
  - Alert markers (red dots on timeline)

**Zoom Controls:**
- Pinch to zoom (native gesture)
- Double-tap to reset zoom

**Interactive Tooltip:**
- Long-press on chart point shows:
  - Frame popup with:
    - Date/time
    - Exact value
    - "120% above baseline"
    - Note icon (if notes exist)

**Timeline Annotations (Below Chart):**
- Horizontal scroll of events:
  - Alert markers with icons
  - Notes markers with text preview
  - Tap to expand details

**Summary Stats Card:**
- Frame at bottom:
  - "Average: 4,500 steps"
  - "High: 8,200 (Jan 5)"
  - "Low: 1,200 (Jan 8)"
  - "Trend: ↓ Declining 15%"

**Export Options (Share Sheet):**
- Export to PDF
- Export to CSV
- Share screenshot
- Send to email

**MAUI Implementation:**
- Syncfusion Charts or LiveCharts2
- Platform-specific chart interactions
- Share API for export
- Pinch/zoom gestures

---

## Alert Screens

### Screen 3.1: Alerts List
**User Story:** 3.1 - Alert Management

**Layout:**

**Header:**
- Title: "Alerts"
- Filter icon (funnel)
- Settings icon (gear) → Notification preferences

**Filter Chips:**
- Horizontal scroll:
  ```
  [All] [Unread] [Critical] [Today] [This Week]
  ```

**Alert Groups (CollectionView):**

**Grouped by Date:**
- Section header: "Today" / "Yesterday" / "This Week"

**Alert Card:**
- Frame with left border (color = severity)

  **Top Row:**
  - Severity badge: "CRITICAL" (red) / "URGENT" (orange) / "INFO" (yellow)
  - Timestamp: "2 hours ago"
  - Unread dot (if unread)

  **Content:**
  - CardiMember name + photo (small, inline)
  - Alert title (bold): "Low Activity Detected"
  - Preview text: "Dad hasn't moved this morning. Typical wake time..."

  **Bottom Row:**
  - Status: "New" / "Acknowledged" / "Resolved"
  - Quick actions:
    - Call icon (tap to call)
    - Checkmark icon (acknowledge)
    - Chevron (expand details)

**Swipe Actions:**
- Swipe right: "Acknowledge" (green)
- Swipe left: "Call" (blue)

**Empty State:**
- Icon: 🔔 (large, gray)
- Text: "No Alerts"
- Subtext: "We'll notify you if anything needs attention"

**Bottom:**
- Button: "View Archived Alerts"

**MAUI Implementation:**
```xml
CollectionView with:
- IsGrouped=true
- GroupHeaderTemplate
- ItemTemplate with SwipeView
- RefreshView
```

---

### Screen 3.2: Alert Detail (Activity Alert)
**User Story:** 11.1 - Activity Decline

**Layout:**

**Header:**
- Back button
- Title: "Alert Details"
- Share button

**Alert Header Card:**
- Severity banner (yellow background)
- Icon: ⚠️
- Title: "Low Activity Alert"
- CardiMember: Photo + Name
- Timestamp: "January 10, 2026 at 11:30 AM"

**Description Section:**
- Frame with icon
- Text (large, readable):
  "Dad's activity has been lower than usual"

**Data Visualization:**
- Chart: 2-week trend (mini version)
- Shows declining steps line

**Comparison Card:**
- Grid layout (2 columns):

  **Current:**
  - Label: "Recent Average"
  - Value: "2,500 steps/day"
  - Icon: ↓

  **Normal:**
  - Label: "Normal Average"
  - Value: "5,000 steps/day"
  - Icon: —

  **Difference:**
  - Full width, highlighted
  - "-50% below normal"

**Context Frame:**
- Background: Light blue
- Icon: 💡
- Text: "This could indicate:"
  - Bullet: "Illness or fatigue"
  - Bullet: "Pain or discomfort"
  - Bullet: "Low mood or depression"

**Recommended Actions:**
- Heading: "What You Can Do"
- Button list (full width):
  1. "Call to Check In" (primary, phone icon)
  2. "Send a Message" (outlined, SMS icon)
  3. "Schedule Doctor Visit" (outlined, calendar icon)

**Additional Actions:**
- Expander: "More Options"
  - "Adjust Baseline" (if new normal)
  - "Add Note About This Alert"
  - "Share with Family"

**Acknowledgment Section:**
- If unread: Button "Mark as Acknowledged"
- If acknowledged: Shows who + when
  - "Acknowledged by Sarah, 30 min ago"
  - Notes (if any)

**Bottom:**
- Button: "View Detailed Activity Data"

---

### Screen 3.3: Alert Detail (Heart Rate)
**User Story:** 11.2 - Elevated HR

**Layout:**

**Alert Header:**
- Orange severity banner
- Icon: ⚡
- Title: "Elevated Heart Rate Alert"
- CardiMember + timestamp

**Description:**
- "Mom's resting heart rate has been elevated for 3 consecutive days"

**Chart:**
- 7-day heart rate chart
- Shaded area: Normal range (68-75 bpm)
- Line: Actual values (elevated portion in orange)

**Comparison Grid:**
- Current: "88 bpm"
- Normal: "68 bpm"
- Difference: "+29% above baseline"

**Context:**
- "Possible causes:"
  - "Infection or illness"
  - "Stress or anxiety"
  - "Dehydration"
  - "Medication side effects"

**Recommended Actions:**
- "Recommend Doctor Visit" (primary, red)
- "Monitor for 2 More Days" (secondary)
- "Call to Check Symptoms"

**Medical History (if available):**
- Expander: "Related Health Info"
  - Shows medications, conditions from profile

---

### Screen 3.4: Alert Detail (Pattern Break - Critical)
**User Story:** 11.3 - No Morning Activity

**Layout:**

**Alert Header:**
- Red severity banner (full width)
- Large icon: 🚨
- Title: "CRITICAL: No Movement Detected"
- CardiMember + timestamp
- Pulsing animation on banner

**Urgent Message:**
- Frame (red border, 4dp)
- Large text: "Dad hasn't moved today"
- Subtext:
  - "Typical wake time: 7:00 AM"
  - "Current time: 11:00 AM"
  - "No activity for 4 hours"

**Last Known Activity:**
- Frame
- "Last Movement Detected:"
- Text: "Yesterday, 10:30 PM"
- Location: "Bedroom (based on device)"

**Immediate Actions (Large Buttons):**
1. "CALL NOW" (red, huge, phone icon)
   - One-tap to initiate call
   - Shows phone number
2. "I'M CHECKING IN PERSON" (orange)
   - Updates status immediately
   - Notifies family

**Secondary Actions:**
- Button: "He Told Me He'd Sleep In"
  - Opens note field
  - Dismisses alert with context

**Family Notification:**
- Frame: "Who else has been notified:"
  - List: Sarah (via SMS), John (via Push)
  - Timestamps of notifications

**Timeline:**
- Shows progression:
  - "10:30 PM: Last movement"
  - "7:00 AM: Expected wake time"
  - "9:00 AM: Alert threshold reached"
  - "11:30 AM: You were notified"

---

### Screen 3.5: Notification Settings
**User Story:** 3.2 - Alert Preferences

**Layout:**

**Header:**
- Back button
- Title: "Notification Settings"
- Save button (if changes)

**CardiMember Selector:**
- If multiple members:
  - Picker: "Settings for: [Dad] ▼"

**Alert Types Section:**

**Grouped List:**

**Group: Activity Alerts**
- Switch: Enable Activity Alerts
- Expanded (if enabled):
  - Slider: Sensitivity
    - Low | Medium | High
  - Text: "Alert when activity is 30% below normal"

**Group: Heart Rate Alerts**
- Switch: Enable
- Slider: Sensitivity
- Text: "Alert when HR exceeds baseline by 20%"

**Group: Sleep Alerts**
- Switch: Enable
- Options:
  - Checkbox: "Poor sleep quality"
  - Checkbox: "Unusual sleep patterns"

**Group: Pattern Break Alerts**
- Switch: Enable (always on for safety, greyed)
- Text: "Required for emergency detection"

**Notification Channels:**

**For Each Alert Type:**
- Multi-select chips:
  ```
  [Email] [SMS] [Push] [All]
  ```

**Quiet Hours:**
- Frame (expandable)
- Switch: "Enable Quiet Hours"
- Time pickers:
  - From: 10:00 PM
  - To: 7:00 AM
- Exception switch:
  - "Still alert for Critical (Red) events"

**Multi-User Routing:**
- Frame
- "Also notify these family members:"
- List of family with checkboxes:
  - Checkbox: Sarah Johnson
    - Chips: [High Severity] [Critical]
  - Checkbox: John Doe
    - Chips: [Critical Only]

**Test Notifications:**
- Button: "Send Test Push Notification"
- Button: "Send Test Email"
- Button: "Send Test SMS"

**MAUI Implementation:**
- CollectionView with groups
- Switch controls with ValueChanged
- Slider with custom styling
- TimePicker for quiet hours

---

## Family & Collaboration

### Screen 4.1: Family Members List
**User Story:** 4.1 - Family Management

**Layout:**

**Header:**
- Back button
- Title: "Family & Sharing"
- "+ Invite" button

**Tabs:**
```
[Active Members] [Pending Invites]
```

**Active Members Section:**

**List Items (CollectionView):**
- Frame per member

  **Layout:**
  - Profile photo (circular, 48dp)
  - Name: "Sarah Johnson"
  - Email: "sarah@email.com"
  - Role badge: "ADMIN" / "STAFF" / "VIEWER"
  - Last active: "Active 2 hours ago"
  - Menu icon (⋮)

**Context Menu (on ⋮ tap):**
- "Change Role" → Role picker
- "View Activity Log"
- "Remove Access" (red text)

**Pending Invites Section:**

**List Items:**
- Email: "john@email.com"
- Role: "Viewer"
- Sent: "2 days ago"
- Button: "Resend"
- Button: "Revoke" (text, red)

**Empty State (Pending):**
- Text: "No pending invitations"

**FAB:**
- "+" button (bottom-right)
- Tap → Invite modal

---

### Screen 4.2: Invite Family Modal
**User Story:** 4.1 - Inviting Members

**Modal Sheet (Bottom sheet or full screen modal):**

**Header:**
- Close button (X)
- Title: "Invite Family Member"

**Form:**

**Email Input:**
- Label: "Email Address"
- Entry (keyboard: Email)
- Validation indicator

**Role Selection:**
- Label: "Access Level"
- Segmented control:
  ```
  [Admin] [Staff] [Viewer]
  ```
- Selected role shows description below:

  **Admin Description:**
  - "Can view, modify settings, invite others"

  **Staff:**
  - "Can view and acknowledge alerts"

  **Viewer:**
  - "Can only view health data"

**Permission Matrix (Collapsible):**
- Expander: "View Permission Details"
- Table showing what each role can do

**Personal Message (Optional):**
- Label: "Add a message (optional)"
- Editor: Multi-line input
- Placeholder: "Hi Sarah, I'd like to share Dad's health monitoring with you..."

**CTA:**
- Primary button: "Send Invitation"
- Text button: "Cancel"

**MAUI Implementation:**
- Modal using Navigation.PushModalAsync
- Or BottomSheet using community toolkit

---

### Screen 4.3: Shared Notes Feed
**User Story:** 4.2 - Coordination

**Layout:**

**Header:**
- Back button
- Title: "Family Notes"
- Filter: "All Notes ▼"

**Add Note Section (Top):**
- Frame with user photo
- Entry: "Add a note for the family..."
- Tap → Expands to full composer

**Notes Feed (CollectionView):**

**Note Card:**
- Frame with rounded corners
- Header row:
  - Author photo (small, 32dp)
  - Author name
  - Timestamp: "2 hours ago"
  - Menu (⋮) - if author is you
- Content:
  - Text with @mentions highlighted
  - Attachments (if any)
- CardiMember tag (if associated): "About: Dad"
- Footer:
  - Reply button (💬) + count
  - Like button (❤️) + count

**Threaded Replies (Expandable):**
- Tap reply count → Expands
- Indented replies shown
- "Load more replies" if >3

**Filters (from header dropdown):**
- All Notes
- About Dad
- About Mom
- My Notes Only
- Mentions Me

---

### Screen 4.4: Add/Edit Note Screen
**User Story:** 4.2 - Shared Notes

**Full Screen Modal:**

**Header:**
- Cancel button
- Title: "New Note"
- Post button (enabled when valid)

**Form:**

**Note Content:**
- Editor (multi-line, expands)
- Placeholder: "Share an update with family..."
- Character counter: "0 / 500"
- @ symbol → Shows mention picker

**Mention Picker (Overlay):**
- Appears when typing @
- List of family members
- Tap to insert @Sarah Johnson

**CardiMember Association:**
- Label: "About (optional)"
- Picker: "Select CardiMember ▼"
  - None (General)
  - Dad
  - Mom

**Attachments:**
- Button: "+ Attach Photo"
- Shows thumbnail grid if added
- Max 3 attachments

**Visibility:**
- Label: "Who can see this"
- Default: "All family members"
- (Future: Selective sharing)

**CTA:**
- Primary button: "Post Note"
- Confirmation toast on success

---

## Settings & Account

### Screen 5.1: Settings Main
**User Story:** 6.1, 6.2 - Settings

**Layout:**

**Header:**
- Back button
- Title: "Settings"

**User Profile Section:**
- Frame (top)
- Profile photo (large, 80dp, tappable)
- Name: "[User Name]"
- Email: "[user@email.com]"
- Edit button (pencil icon)

**Settings Groups (CollectionView):**

**Group: Account**
- "My Profile" (chevron)
- "Subscription & Billing" (chevron, badge: "Complete Care")
- "Family & Sharing" (chevron)

**Group: CardiMembers**
- "Manage CardiMembers" (chevron)
- "Connected Devices" (chevron)

**Group: Notifications**
- "Alert Settings" (chevron)
- "Notification Preferences" (chevron)
- "Quiet Hours" (chevron)

**Group: Security**
- "Change Password" (chevron)
- Switch: "Biometric Login" (toggle inline)
- "Privacy Settings" (chevron)

**Group: Support**
- "Help Center" (chevron)
- "Contact Support" (chevron)
- "Terms & Privacy" (chevron)

**Group: About**
- "App Version" (text: "1.0.0")
- "Check for Updates"

**Danger Zone:**
- "Sign Out" (red text)
- "Delete Account" (red text)

**MAUI Implementation:**
- CollectionView with IsGrouped=true
- Mix of navigation cells and switch cells
- Platform-specific settings UI

---

### Screen 5.2: Subscription Management
**User Story:** 6.1 - Subscription

**Layout:**

**Header:**
- Back button
- Title: "Subscription"

**Current Plan Card:**
- Frame with gradient background
- Badge: "COMPLETE CARE"
- Price: "$15/month"
- Renewal: "Renews Feb 10, 2026"
- Button: "Manage Subscription"

**Features Included:**
- List with checkmarks:
  - ✓ Unlimited CardiMembers
  - ✓ Advanced ML Alerts
  - ✓ Family Sharing (5 members)
  - ✓ 90-day data retention
  - ✓ Priority support

**Usage Section:**
- Frame
- "Your Usage:"
- Progress bars:
  - CardiMembers: 2 of ∞
  - Family Members: 3 of 5
  - Data: 45 days of 90

**Plan Comparison:**
- Horizontal scroll (CarouselView)
- 3 plan cards (swipe to compare):

**Plan Card Structure:**
- Frame with border (current plan highlighted)
- Header:
  - Plan name
  - Price/month
  - "Current Plan" badge if active
- Features list (condensed)
- Button:
  - "Current Plan" (disabled) or
  - "Upgrade" / "Downgrade"

**Annual Discount Banner:**
- Frame (gold background)
- "💰 Save 15% with Annual Billing"
- "Switch to Annual" button

**Billing Section:**
- Frame
- "Payment Method:"
- Visa •••• 1234 (with card icon)
- "Change" button
- "Billing History" button

**MAUI Implementation:**
- CarouselView for plan comparison
- Platform-specific in-app purchase (StoreKit/Google Billing)
- WebView for Stripe payment if needed

---

### Screen 5.3: Device Management
**User Story:** 6.2 - Devices

**Layout:**

**Header:**
- Back button
- Title: "Connected Devices"
- "+ Add Device" button

**Devices List (Grouped by CardiMember):**

**Group Header:**
- CardiMember name + photo

**Device Card:**
- Frame
- Device logo (left, 48dp)
- Device info:
  - Name: "Dad's Fitbit Charge 5"
  - Status badge:
    - 🟢 "Active" (synced 10m ago)
    - 🟡 "Token Expiring Soon"
    - 🔴 "Disconnected"
  - Data sources: "Activity, HR, Sleep"
  - Primary star (if primary device)
- Menu (⋮)

**Context Menu:**
- "Refresh Connection"
- "Set as Primary" (toggle)
- "View Sync History"
- "Remove Device" (red)

**Sync Status Detail (Expandable):**
- Tap device card → Expands
- Shows:
  - Last sync: "10 minutes ago"
  - Next sync: "In 20 minutes"
  - Data synced today: "4 updates"
  - Battery: "75%" (if available)

**Add Device Button:**
- Floating or bottom button
- Tap → Device selection wizard

**Troubleshooting (Bottom):**
- Expander: "Device Not Syncing?"
- Common solutions:
  - Check Bluetooth
  - Reconnect OAuth
  - Contact support

---

## CardiMember Management

### Screen 6.1: CardiMember Detail
**Purpose:** View/edit CardiMember profile

**Layout:**

**Header:**
- Back button
- Title: "[Name]'s Profile"
- Edit button (pencil)

**Profile Section:**
- Large photo (centered, 120dp)
- Name (large, centered)
- Age & relationship: "78 years old • Dad"

**Contact Info:**
- Frame
- Emergency contact:
  - Name
  - Phone (tappable → call)
  - Relationship

**Medical Info (Encrypted):**
- Frame with lock icon
- Expander: "Medical Notes"
- Shows encrypted notes
- "Edit" requires biometric auth

**Monitoring Info:**
- Frame
- Connected devices: "2 devices"
- Monitoring since: "Jan 1, 2026"
- Baseline status: "Learning (15 days)"

**Actions:**
- Button: "View Dashboard"
- Button: "View Alerts"
- Button: "Manage Devices"

**Danger Zone:**
- Button: "Pause Monitoring" (yellow)
- Button: "Remove CardiMember" (red)

---

### Screen 6.2: Edit CardiMember
**Purpose:** Update CardiMember info

**Layout:**

**Header:**
- Cancel button
- Title: "Edit [Name]"
- Save button

**Form (Scrollable):**

**Photo:**
- Large circular image
- "Change Photo" button

**Basic Info:**
- Entry: "Full Name"
- DatePicker: "Date of Birth"
- Picker: "Relationship"

**Optional Info:**
- Editor: "Medical Notes" (encrypted)
- Entry: "Emergency Contact Name"
- Entry: "Emergency Contact Phone"

**Monitoring Preferences:**
- Switch: "Enable Monitoring"
- Picker: "Alert Sensitivity"
  - Low / Medium / High

**CTA:**
- Primary button: "Save Changes"
- Changes tracked, "Unsaved changes" warning on exit

---

## Offline Features

### Screen 7.1: Offline Mode Indicator
**User Story:** 10.1 - Offline Support

**Offline Banner (Top of Screen):**
- Frame (yellow background)
- Icon: 📡 (crossed out)
- Text: "Offline Mode"
- Subtext: "Last updated 2 hours ago"
- Closable (X) but reappears on navigation

**Dashboard (Offline State):**
- All data shown is cached
- Greyed-out sync icons
- "Syncing disabled" tooltip on refresh

**Alert Queue:**
- Frame (orange border)
- "2 alerts pending sync"
- List of offline alerts:
  - Shows alert cards
  - "Not yet synced" badge
- "Will sync when connected"

**Functionality:**
- Read-only mode
- No POST operations
- Queue actions for sync

**Connection Restored:**
- Toast notification: "Back online!"
- Syncing animation
- Progress: "Syncing 2 alerts..."
- Success: "All data synced"

---

### Screen 7.2: Offline Data Cache Settings
**User Story:** 10.1 - Cache Management

**Layout:**

**Header:**
- Title: "Offline Data"

**Cache Info:**
- Frame
- "Cached Data Size: 45 MB"
- "Last synced: 10 minutes ago"

**Settings:**
- Slider: "Days to cache"
  - 1 day | 7 days | 30 days
- Switch: "Auto-download charts"
- Switch: "Cache photos"

**Actions:**
- Button: "Clear Cache"
  - Confirmation: "You'll need internet to view data"
- Button: "Sync Now"

---

## Native Features

### Screen 8.1: Biometric Login
**User Story:** 10.2 - Biometric Auth

**Launch Screen (Biometric Enabled):**

**Layout:**
- CardiTrack logo (top)
- User name + photo
- Biometric prompt (centered):
  - iOS: Face ID animation
  - Android: Fingerprint animation
- Text: "Scan to unlock"
- Fallback: "Use Password" link

**Biometric Prompt (Platform Native):**
- iOS: Face ID system dialog
- Android: Biometric prompt

**Fallback (Password):**
- Password entry field
- "Forgot password?" link
- Login button

**Settings (Security):**
- Switch: "Require biometric for:"
  - App launch ✓
  - Viewing alerts ✓
  - Acknowledging alerts ✓
  - Changing settings ✓

---

### Screen 8.2: Push Notifications
**User Story:** 5.1 - Push Notifications

**Lock Screen Notification:**

**Compact (iOS/Android System UI):**
- App icon
- Title: "[Name] - Critical Alert"
- Body: "Dad hasn't moved this morning. Tap to view..."
- Time: "2m ago"

**Expanded (Long press):**
- Full alert text
- Image (if applicable)
- Action buttons:
  - "Call" (initiates call)
  - "View" (opens app)
  - "Acknowledge"

**In-App Notification (When App Open):**
- Banner slides from top
- Shows alert summary
- Tap → Navigate to alert detail
- Swipe up → Dismiss

**Notification Center:**
- Grouped by CardiMember:
  - "3 alerts from Dad"
  - Expandable list
- Badge count on app icon

---

### Screen 8.3: Home Screen Widget
**User Story:** 5.2 - Widget

**Small Widget (iOS/Android):**
- Size: 2x2 grid
- Content:
  - CardiTrack logo (small)
  - CardiMember photo (circular)
  - Status indicator (large)
  - Name
  - Last synced time
- Tap → Opens app to that member

**Medium Widget:**
- Size: 4x2 grid
- Shows 2 CardiMembers side-by-side
- Each with: Photo, name, status, key metric

**Large Widget (iOS):**
- Size: 4x4 grid
- Shows up to 4 CardiMembers
- Each with mini dashboard:
  - Photo, name, status
  - 3 key metrics (icons + values)
  - Alert badge

**Widget Configuration:**
- Long-press widget → Edit
- Select CardiMembers to show
- Choose metrics to display
- Update frequency setting

**MAUI Implementation:**
- iOS: WidgetKit extension
- Android: AppWidget
- Shared data via app groups

---

### Screen 8.4: Share Sheet Integration
**Purpose:** Export data via native sharing

**Share Options:**
- Trigger: Tap share icon on charts/alerts

**Share Sheet (System):**
- Export as PDF
- Export as CSV
- Share screenshot
- Email to myself
- Share to family member
- Save to Files

**Custom Actions:**
- "Send to Doctor" (pre-configured email)
- "Add to Health App" (iOS HealthKit)

---

## Navigation Patterns

### Bottom Tab Navigation
**Primary Navigation (Always Visible):**

```
┌────────────────────────────────┐
│                                │
│         Content Area           │
│                                │
│                                │
├────────────────────────────────┤
│  🏠      🔔      👥      ⚙️   │
│ Home  Alerts  Family Settings │
└────────────────────────────────┘
```

**MAUI Implementation:**
```xml
<Shell>
  <TabBar>
    <ShellContent Title="Dashboard" Icon="home.png" />
    <ShellContent Title="Alerts" Icon="bell.png" />
    <ShellContent Title="Family" Icon="family.png" />
    <ShellContent Title="Settings" Icon="settings.png" />
  </TabBar>
</Shell>
```

---

### Flyout Menu
**Secondary Navigation (Swipe from left):**

**Menu Items:**
- User Profile (header)
- Dashboard
- CardiMembers (badge: 2)
- Alerts (badge: 3)
- Family & Sharing
- Subscription
- Settings
- Help & Support
- Sign Out

**MAUI Implementation:**
```xml
<Shell.FlyoutContent>
  <CollectionView ItemsSource="{Binding MenuItems}">
    <!-- Menu template -->
  </CollectionView>
</Shell.FlyoutContent>
```

---

### Gesture Navigation

**Swipe Gestures:**
- Swipe right (from left edge): Open flyout
- Swipe left on list items: Quick actions
- Swipe down: Pull to refresh
- Pinch: Zoom charts
- Long press: Context menu

**MAUI Gestures:**
```xml
<Frame>
  <Frame.GestureRecognizers>
    <SwipeGestureRecognizer Direction="Left" />
    <TapGestureRecognizer />
    <PanGestureRecognizer />
  </Frame.GestureRecognizers>
</Frame>
```

---

## Design System

### Colors (Brand Palette)
```
Primary: #2563EB (Blue)
Secondary: #10B981 (Green)
Success: #10B981 (Green)
Warning: #F59E0B (Orange)
Error: #EF4444 (Red)
Info: #3B82F6 (Light Blue)

Status Colors:
Green (Good): #10B981
Yellow (Caution): #F59E0B
Orange (Urgent): #F97316
Red (Critical): #DC2626
```

### Typography
```
Title Large: 32sp, Bold
Title: 24sp, Bold
Headline: 20sp, SemiBold
Body: 16sp, Regular
Caption: 14sp, Regular
Micro: 12sp, Regular
```

### Spacing
```
Micro: 4dp
Small: 8dp
Medium: 16dp
Large: 24dp
XLarge: 32dp
```

### Component Sizes
```
Button Height: 48dp (min touch target)
Icon Size: 24dp (standard), 32dp (large)
Avatar: 48dp (small), 64dp (medium), 80dp (large)
Card Radius: 12dp
Border Width: 1dp (standard), 2dp (emphasis)
```

---

## Platform-Specific Considerations

### iOS-Specific
- Use SF Symbols for icons
- Haptic feedback on interactions
- Safe area insets for notch
- Pull-to-refresh with iOS styling
- Modal sheets with drag handle

### Android-Specific
- Material Design 3 components
- Floating Action Buttons
- Bottom sheets
- Navigation drawer
- Back button handling

---

## Accessibility Features

### Screen Reader Support
- All buttons have ContentDescription
- Images have Alt text
- Form labels properly associated
- Dynamic font sizing support

### Color Contrast
- WCAG AA minimum (4.5:1 for text)
- Status not solely reliant on color
- High contrast mode option

### Touch Targets
- Minimum 48x48 dp
- Adequate spacing between tappable elements

---

## Performance Optimizations

### Image Loading
- Lazy loading for lists
- Image caching (FFImageLoading)
- Thumbnail generation for large images

### List Virtualization
- CollectionView with recycling
- Incremental loading
- Pull-to-refresh

### Data Caching
- SQLite local database
- 7-day offline cache
- Background sync

---

**Document Complete**
**Total Screens:** 40+
**Next Steps:** Create Blazor Web UI screens document

