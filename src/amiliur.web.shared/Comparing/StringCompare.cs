using System.Globalization;
using amiliur.shared.Extensions;
using JetBrains.Annotations;

namespace amiliur.web.shared.Comparing;

public static class StringCompare
{
    [UsedImplicitly]
    public static bool Contains(this string source, string value)
    {
        var compareInfo = CultureInfo.InvariantCulture.CompareInfo;
        return compareInfo.IndexOf(source.RemoveDiacritics(), value.RemoveDiacritics(), CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) > -1;
    }

    [UsedImplicitly]
    public static bool Equals(this string source, string value)
    {
        return string.Compare(source.RemoveDiacritics(), value.RemoveDiacritics(), CultureInfo.InvariantCulture, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) == 0;
    }

    [UsedImplicitly]
    public static bool GreaterThan(this string source, string value)
    {
        return string.Compare(source.RemoveDiacritics(), value.RemoveDiacritics(), CultureInfo.InvariantCulture, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) > 0;
    }

    [UsedImplicitly]
    public static bool GreaterThanOrEqual(this string source, string value)
    {
        return source.Equals(value) || source.GreaterThan(value);
    }

    [UsedImplicitly]
    public static bool LessThan(this string source, string value)
    {
        return string.Compare(source, value, CultureInfo.InvariantCulture, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) < 0;
    }

    [UsedImplicitly]
    public static bool LessThanOrEqual(this string source, string value)
    {
        return source.Equals(value) || source.LessThan(value);
    }

    [UsedImplicitly]
    public static bool StartsWith(this string source, string value)
    {
        return source.RemoveDiacritics().StartsWith(value.RemoveDiacritics(), true, CultureInfo.InvariantCulture);
    }
}