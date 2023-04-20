using System.Text.RegularExpressions;

namespace amiliur.shared.Extensions;

public sealed class StringUtils
{
    public static string ToSplitCase(string input, char sep = '_')
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        var startUnderscores = Regex.Match(input, $@"^{sep}+");
        return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", $"$1{sep}$2").ToLower();
    }

    public static string ToSnakeCase(string input)
    {
        return ToSplitCase(input, '_');
    }

    public static string ToKebabCase(string input)
    {
        return ToSplitCase(input, '-');
    }
    
    
}