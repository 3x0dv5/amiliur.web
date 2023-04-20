using amiliur.shared.Extensions;

namespace amiliur.web.shared.Comparing;

public static class DateTimeCompare
{
    public static bool GreaterThanOrEqual(this DateTime source, DateTime value)
    {
        return source >= value;
    }

    public static bool GreaterThanOrEqual(this DateTime source, DateOnly? value)
    {
        if (value == null)
            return false;

        return source >= value.AsDateTime();
    }

    public static bool GreaterThanOrEqual(this DateTime? source, DateTime value)
    {
        if (source == null)
            return false;
        return source >= value;
    }

    public static bool GreaterThanOrEqual(this DateTime? source, DateTime? value)
    {
        if (source == null || value == null)
            return false;
        return source >= value;
    }

    public static bool GreaterThanOrEqual(this DateOnly? source, DateTime value)
    {
        if (source == null)
            return false;
        return source.AsDateTime() >= value;
    }

    public static bool GreaterThanOrEqual(this DateOnly? source, DateOnly? value)
    {
        if (source == null || value == null)
            return false;
        return source >= value;
    }

    public static bool GreaterThanOrEqual(this DateOnly? source, DateOnly value)
    {
        return source >= value;
    }

    public static bool GreaterThanOrEqual(this DateOnly source, DateOnly? value)
    {
        return source >= value;
    }

    public static bool GreaterThanOrEqual(this DateOnly source, DateOnly value)
    {
        return source >= value;
    }

    public static bool LessThanOrEqual(this DateTime source, DateTime value)
    {
        return source <= value;
    }

    public static bool LessThanOrEqual(this DateTime source, DateOnly? value)
    {
        if (value == null)
            return false;

        return source <= value.AsDateTime();
    }

    public static bool LessThanOrEqual(this DateTime? source, DateTime value)
    {
        return source <= value;
    }

    public static bool LessThanOrEqual(this DateTime? source, DateTime? value)
    {
        if (source == null || value == null)
            return false;
        return source <= value;
    }

    public static bool LessThanOrEqual(this DateOnly? source, DateTime value)
    {
        if (source == null)
            return false;
        return source.AsDateTime() <= value;
    }

    public static bool LessThanOrEqual(this DateOnly? source, DateOnly? value)
    {
        if (source == null || value == null)
            return false;
        return source <= value;
    }


    public static bool LessThanOrEqual(this DateOnly? source, DateOnly value)
    {
        return source <= value;
    }

    public static bool LessThanOrEqual(this DateOnly source, DateOnly? value)
    {
        return source <= value;
    }

    public static bool LessThanOrEqual(this DateOnly source, DateOnly value)
    {
        return source <= value;
    }
}