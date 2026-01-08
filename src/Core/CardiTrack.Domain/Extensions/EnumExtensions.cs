using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CardiTrack.Domain.Extensions;

/// <summary>
/// Extension methods for working with enums, especially for UI rendering
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Gets the Display Name from the [Display(Name = "")] attribute
    /// </summary>
    /// <param name="enumValue">The enum value</param>
    /// <returns>The display name if attribute exists, otherwise the enum's ToString()</returns>
    public static string GetDisplayName(this Enum enumValue)
    {
        var displayAttribute = enumValue.GetType()
            .GetField(enumValue.ToString())?
            .GetCustomAttribute<DisplayAttribute>();

        return displayAttribute?.Name ?? enumValue.ToString();
    }

    /// <summary>
    /// Gets all values of an enum as a list with their display names
    /// </summary>
    /// <typeparam name="TEnum">The enum type</typeparam>
    /// <returns>List of tuples containing (Value, DisplayName)</returns>
    public static List<(TEnum Value, string DisplayName)> ToList<TEnum>() where TEnum : struct, Enum
    {
        return Enum.GetValues<TEnum>()
            .Select(value => (value, value.GetDisplayName()))
            .ToList();
    }

    /// <summary>
    /// Gets all values of an enum as a dictionary with the enum value as key and display name as value
    /// </summary>
    /// <typeparam name="TEnum">The enum type</typeparam>
    /// <returns>Dictionary of enum values to display names</returns>
    public static Dictionary<TEnum, string> ToDictionary<TEnum>() where TEnum : struct, Enum
    {
        return Enum.GetValues<TEnum>()
            .ToDictionary(value => value, value => value.GetDisplayName());
    }

    /// <summary>
    /// Gets all values of an enum as a list of key-value pairs suitable for dropdowns
    /// </summary>
    /// <typeparam name="TEnum">The enum type</typeparam>
    /// <returns>List of key-value pairs (int value, string display name)</returns>
    public static List<KeyValuePair<int, string>> ToKeyValueList<TEnum>() where TEnum : struct, Enum
    {
        return Enum.GetValues<TEnum>()
            .Select(value => new KeyValuePair<int, string>(
                Convert.ToInt32(value),
                value.GetDisplayName()))
            .ToList();
    }

    /// <summary>
    /// Gets all values of an enum as a list of select list items
    /// Useful for Blazor, MVC, and other UI frameworks
    /// </summary>
    /// <typeparam name="TEnum">The enum type</typeparam>
    /// <returns>List of select items with Value (int) and Text (display name)</returns>
    public static List<SelectListItem> ToSelectList<TEnum>() where TEnum : struct, Enum
    {
        return Enum.GetValues<TEnum>()
            .Select(value => new SelectListItem
            {
                Value = Convert.ToInt32(value).ToString(),
                Text = value.GetDisplayName()
            })
            .ToList();
    }

    /// <summary>
    /// Parses a string value to an enum, case-insensitive
    /// </summary>
    /// <typeparam name="TEnum">The enum type</typeparam>
    /// <param name="value">The string value to parse</param>
    /// <param name="defaultValue">Default value if parsing fails</param>
    /// <returns>The parsed enum value or default if parsing fails</returns>
    public static TEnum ParseOrDefault<TEnum>(string value, TEnum defaultValue) where TEnum : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(value))
            return defaultValue;

        return Enum.TryParse<TEnum>(value, true, out var result) ? result : defaultValue;
    }

    /// <summary>
    /// Checks if an enum value is defined in the enum
    /// </summary>
    /// <typeparam name="TEnum">The enum type</typeparam>
    /// <param name="value">The value to check</param>
    /// <returns>True if the value is defined</returns>
    public static bool IsDefined<TEnum>(int value) where TEnum : struct, Enum
    {
        return Enum.IsDefined(typeof(TEnum), value);
    }
}

/// <summary>
/// Simple select list item for UI rendering
/// </summary>
public class SelectListItem
{
    public string Value { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public bool Selected { get; set; }
}
