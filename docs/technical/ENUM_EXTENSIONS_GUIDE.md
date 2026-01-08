# Enum Extensions Guide

All enums in CardiTrack have been enhanced with Display attributes and powerful extension methods for easy UI rendering.

## Features

✅ Display Name attributes on all enum values
✅ Extension methods to convert enums to lists/dictionaries
✅ Select list generation for dropdowns
✅ Safe parsing with defaults
✅ Value validation

## Display Attributes

All enum values now have `[Display(Name = "...")]` attributes:

```csharp
public enum DeviceType
{
    [Display(Name = "Fitbit")]
    Fitbit = 1,

    [Display(Name = "Apple Watch")]
    AppleWatch = 2,

    [Display(Name = "Samsung Galaxy Watch")]
    Samsung = 4,

    // ... etc
}
```

## Extension Methods

### 1. GetDisplayName()

Get the friendly display name for any enum value:

```csharp
using CardiTrack.Domain.Enums;
using CardiTrack.Domain.Extensions;

var device = DeviceType.AppleWatch;
var displayName = device.GetDisplayName(); // Returns: "Apple Watch"

var severity = AlertSeverity.Red;
var severityName = severity.GetDisplayName(); // Returns: "Urgent"
```

### 2. ToList<TEnum>()

Convert enum to list of tuples (Value, DisplayName):

```csharp
var deviceList = EnumExtensions.ToList<DeviceType>();

foreach (var (value, displayName) in deviceList)
{
    Console.WriteLine($"{value} ({(int)value}): {displayName}");
}

// Output:
// Fitbit (1): Fitbit
// AppleWatch (2): Apple Watch
// Garmin (3): Garmin
// Samsung (4): Samsung Galaxy Watch
// ...
```

### 3. ToDictionary<TEnum>()

Convert enum to dictionary for quick lookups:

```csharp
var relationshipDict = EnumExtensions.ToDictionary<RelationshipType>();

string displayName = relationshipDict[RelationshipType.Grandparent];
// Returns: "Grandparent"
```

### 4. ToKeyValueList<TEnum>()

Perfect for JSON APIs and client-side rendering:

```csharp
var subscriptionTiers = EnumExtensions.ToKeyValueList<SubscriptionTier>();

// Returns:
// [
//   { Key: 1, Value: "Basic Care" },
//   { Key: 2, Value: "Complete Care" },
//   { Key: 3, Value: "Guardian Plus" }
// ]

// Serialize for API response
var json = JsonSerializer.Serialize(subscriptionTiers);
```

### 5. ToSelectList<TEnum>()

Generate select list items for HTML dropdowns:

```csharp
var genderOptions = EnumExtensions.ToSelectList<Gender>();

// Returns:
// [
//   { Value: "1", Text: "Male", Selected: false },
//   { Value: "2", Text: "Female", Selected: false },
//   { Value: "3", Text: "Other", Selected: false },
//   { Value: "4", Text: "Prefer Not to Say", Selected: false }
// ]
```

**Blazor Example:**

```razor
<InputSelect @bind-Value="cardiMember.Gender">
    <option value="">Select gender...</option>
    @foreach (var item in EnumExtensions.ToSelectList<Gender>())
    {
        <option value="@item.Value">@item.Text</option>
    }
</InputSelect>
```

**MVC Example:**

```cshtml
@using CardiTrack.Domain.Enums
@using CardiTrack.Domain.Extensions

@Html.DropDownListFor(
    m => m.DeviceType,
    new SelectList(
        EnumExtensions.ToSelectList<DeviceType>(),
        "Value",
        "Text"
    ),
    "Select a device...",
    new { @class = "form-control" }
)
```

### 6. ParseOrDefault<TEnum>()

Safely parse strings to enums with fallback:

```csharp
// From user input or query string
string input = "AppleWatch";
var device = EnumExtensions.ParseOrDefault<DeviceType>(input, DeviceType.Other);
// Returns: DeviceType.AppleWatch

string invalid = "InvalidDevice";
var fallback = EnumExtensions.ParseOrDefault<DeviceType>(invalid, DeviceType.Other);
// Returns: DeviceType.Other

// Case-insensitive parsing
string lowercase = "fitbit";
var parsed = EnumExtensions.ParseOrDefault<DeviceType>(lowercase, DeviceType.Other);
// Returns: DeviceType.Fitbit
```

### 7. IsDefined<TEnum>()

Validate enum integer values:

```csharp
bool isValid = EnumExtensions.IsDefined<DeviceType>(1); // true (Fitbit)
bool isInvalid = EnumExtensions.IsDefined<DeviceType>(999); // false

// Use in validation
if (!EnumExtensions.IsDefined<AlertSeverity>(alertValue))
{
    throw new ArgumentException("Invalid alert severity");
}
```

## Real-World Usage Examples

### API Endpoint - Return Enum Options

```csharp
[HttpGet("device-types")]
public IActionResult GetDeviceTypes()
{
    var devices = EnumExtensions.ToKeyValueList<DeviceType>();
    return Ok(devices);
}

// Response:
// [
//   { "key": 1, "value": "Fitbit" },
//   { "key": 2, "value": "Apple Watch" },
//   { "key": 3, "value": "Garmin" },
//   ...
// ]
```

### Blazor Component - Device Selector

```razor
@using CardiTrack.Domain.Enums
@using CardiTrack.Domain.Extensions

<div class="device-selector">
    <h3>Select Your Device</h3>
    @foreach (var (device, displayName) in EnumExtensions.ToList<DeviceType>())
    {
        <button class="device-card" @onclick="() => SelectDevice(device)">
            <img src="/images/devices/@(device.ToString().ToLower()).svg" />
            <span>@displayName</span>
        </button>
    }
</div>

@code {
    private void SelectDevice(DeviceType device)
    {
        // Handle device selection
    }
}
```

### Validation - Check Alert Severity

```csharp
public class AlertValidator : AbstractValidator<Alert>
{
    public AlertValidator()
    {
        RuleFor(x => x.Severity)
            .Must(severity => EnumExtensions.IsDefined<AlertSeverity>((int)severity))
            .WithMessage("Invalid alert severity");
    }
}
```

### Dashboard - Display Alert with Friendly Name

```csharp
public class AlertViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string SeverityDisplay { get; set; }
    public string AlertTypeDisplay { get; set; }

    public static AlertViewModel FromEntity(Alert alert)
    {
        return new AlertViewModel
        {
            Id = alert.Id,
            Title = alert.Title,
            SeverityDisplay = alert.Severity.GetDisplayName(), // "Urgent", "Minor Concern", etc.
            AlertTypeDisplay = alert.AlertType.GetDisplayName() // "Heart Rate Alert", etc.
        };
    }
}
```

### Filter Form - Subscription Status Dropdown

```razor
@using CardiTrack.Domain.Enums
@using CardiTrack.Domain.Extensions

<EditForm Model="filterModel">
    <div class="form-group">
        <label for="status">Subscription Status:</label>
        <InputSelect id="status" @bind-Value="filterModel.Status" class="form-control">
            <option value="">All Statuses</option>
            @foreach (var (status, displayName) in EnumExtensions.ToList<SubscriptionStatus>())
            {
                <option value="@status">@displayName</option>
            }
        </InputSelect>
    </div>
</EditForm>
```

## Performance Considerations

- **GetDisplayName()** - Uses reflection, but result can be cached
- **ToList/ToDictionary** - Create new collections, consider caching for repeated use
- **ToSelectList** - Lightweight, safe to call multiple times

### Caching Example

```csharp
public static class EnumCache
{
    private static readonly Dictionary<Type, object> Cache = new();

    public static List<(TEnum, string)> GetCachedList<TEnum>() where TEnum : struct, Enum
    {
        var type = typeof(TEnum);
        if (!Cache.TryGetValue(type, out var cached))
        {
            cached = EnumExtensions.ToList<TEnum>();
            Cache[type] = cached;
        }
        return (List<(TEnum, string)>)cached;
    }
}
```

## All Available Enums

### Core Enums
- **OrganizationType** - Family Account, Business Account
- **UserRole** - Member, Administrator, Staff Member
- **Gender** - Male, Female, Other, Prefer Not to Say

### Relationship & Monitoring
- **RelationshipType** - Self, Parent, Spouse, Grandparent, Sibling, Child, Other

### Device & Connection
- **DeviceType** - Fitbit, Apple Watch, Garmin, Samsung Galaxy Watch, Withings, Oura Ring, Whoop, Other Device
- **ConnectionStatus** - Connected, Disconnected, Token Expired, Authentication Error, Sync Error

### Alerts
- **AlertType** - Inactivity Alert, Heart Rate Alert, Sleep Quality Alert, Pattern Break Alert, Trend Alert
- **AlertSeverity** - Informational (Green), Minor Concern (Yellow), Moderate Concern (Orange), Urgent (Red)

### Billing
- **SubscriptionTier** - Basic Care, Complete Care, Guardian Plus
- **SubscriptionStatus** - Trial Period, Active, Past Due, Cancelled, Suspended
- **BillingCycle** - Monthly, Annual

## Testing

Example unit test:

```csharp
using CardiTrack.Domain.Enums;
using CardiTrack.Domain.Extensions;
using Xunit;

public class EnumExtensionsTests
{
    [Fact]
    public void GetDisplayName_ReturnsCorrectDisplayName()
    {
        // Arrange
        var device = DeviceType.AppleWatch;

        // Act
        var displayName = device.GetDisplayName();

        // Assert
        Assert.Equal("Apple Watch", displayName);
    }

    [Fact]
    public void ToList_ReturnsAllEnumValues()
    {
        // Act
        var list = EnumExtensions.ToList<AlertSeverity>();

        // Assert
        Assert.Equal(4, list.Count); // Green, Yellow, Orange, Red
        Assert.Contains(list, item => item.DisplayName == "Urgent");
    }

    [Fact]
    public void ParseOrDefault_ParsesValidValue()
    {
        // Act
        var result = EnumExtensions.ParseOrDefault<Gender>("Female", Gender.PreferNotToSay);

        // Assert
        Assert.Equal(Gender.Female, result);
    }

    [Fact]
    public void ParseOrDefault_ReturnsDefaultForInvalidValue()
    {
        // Act
        var result = EnumExtensions.ParseOrDefault<Gender>("Invalid", Gender.PreferNotToSay);

        // Assert
        Assert.Equal(Gender.PreferNotToSay, result);
    }

    [Fact]
    public void IsDefined_ValidatesCorrectly()
    {
        // Assert
        Assert.True(EnumExtensions.IsDefined<DeviceType>(1)); // Fitbit
        Assert.False(EnumExtensions.IsDefined<DeviceType>(999));
    }
}
```

## Summary

With these enum extensions, you can:

✅ Display user-friendly names in the UI
✅ Generate dropdown lists automatically
✅ Safely parse and validate enum values
✅ Reduce boilerplate code
✅ Maintain consistency across the application

All enums are ready to use with zero additional configuration!
