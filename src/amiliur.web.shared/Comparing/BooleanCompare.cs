namespace amiliur.web.shared.Comparing;

public static class BooleanCompare
{
    public static bool Equals(this object source, bool value)
    {
        return ((bool)source).Equals(value);
    }
    public static bool Equals(this bool source, bool value)
    {
        return source == value;
    }
}