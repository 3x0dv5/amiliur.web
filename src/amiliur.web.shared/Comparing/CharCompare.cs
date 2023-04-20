using JetBrains.Annotations;

namespace amiliur.web.shared.Comparing;

public static class CharCompare
{
    [UsedImplicitly]
    public static bool Contains(this char source, char value)
    {
        return StringCompare.Equals(source.ToString(), value.ToString());
    }

    [UsedImplicitly]
    public static bool Equals(this char source, char value)
    {
        return StringCompare.Equals(source.ToString(), value.ToString());
    }

    [UsedImplicitly]
    public static bool Equals(this char? source, char value)
    {
        if(source == null)
            return false;
        return StringCompare.Equals(source.ToString(), value.ToString());
    }

    [UsedImplicitly]
    public static bool GreaterThan(this char source, char value)
    {
        return source.ToString().GreaterThan(value.ToString());
    }

    [UsedImplicitly]
    public static bool GreaterThanOrEqual(this char source, char value)
    {
        return source.ToString().GreaterThanOrEqual(value.ToString());
    }

    [UsedImplicitly]
    public static bool LessThan(this char source, char value)
    {
        return source.ToString().LessThan(value.ToString());
    }

    [UsedImplicitly]
    public static bool LessThanOrEqual(this char source, char value)
    {
        return source.ToString().LessThanOrEqual(value.ToString());
    }

    [UsedImplicitly]
    public static bool StartsWith(this char source, char value)
    {
        return StringCompare.StartsWith(source.ToString(), value.ToString());
    }

    [UsedImplicitly]
    public static bool StartsWith(this char? source, char value)
    {
        if (source == null)
            return false;
        return StringCompare.StartsWith(source.ToString(), value.ToString());
    }
}