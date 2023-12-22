using System.Reflection;

namespace Ghost.Data.Enums;

public static class StringValueExtension
{
  public static string? GetStringValue(this Enum value)
  {
    // Get the type
    Type type = value.GetType();

    // Get fieldinfo for this type
    FieldInfo? fieldInfo = type.GetField(value.ToString());

    // Get the stringvalue attributes
    StringValueAttribute[]? attribs = fieldInfo?.GetCustomAttributes(
        typeof(StringValueAttribute), false) as StringValueAttribute[];

    // Return the first if there was a match.
    if (attribs == null) return null;
    return attribs.Length > 0 ? attribs[0].StringValue : null;
  }
}