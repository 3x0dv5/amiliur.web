namespace amiliur.web.shared.Comparing;

public static class IntCompare
{
    public static bool Equals(this object left, int right)
    {
        return Equals((int)left, right);
    }

    public static bool Equals(this double left, int right)
    {
        return Equals((int)left, right);
    }

    public static bool Equals(this int left, int right)
    {
        return left == right;
    }

    public static bool GreaterThan(this object left, int right)
    {
        return GreaterThan((int)left, right);
    }

    public static bool GreaterThan(this double left, int right)
    {
        return GreaterThan((int)left, right);
    }

    public static bool GreaterThan(this int left, int right)
    {
        return left > right;
    }

    public static bool GreaterThanOrEqual(this double left, int right)
    {
        return GreaterThanOrEqual((int)left, right);
    }

    public static bool GreaterThanOrEqual(this object left, int right)
    {
        return GreaterThanOrEqual((int)left, right);
    }

    public static bool GreaterThanOrEqual(this int left, int right)
    {
        return left >= right;
    }

    public static bool LessThan(this double left, int right)
    {
        return LessThan((int) left, right);
    }

    public static bool LessThan(this object left, int right)
    {
        return LessThan((int) left, right);
    }

    public static bool LessThan(this int left, int right)
    {
        return left < right;
    }

    public static bool LessThanOrEqual(this double left, int right)
    {
        return LessThanOrEqual((int) left, right);
    }
    public static bool LessThanOrEqual(this object left, int right)
    {
        return LessThanOrEqual((int) left, right);
    }

    public static bool LessThanOrEqual(this int left, int right)
    {
        return left <= right;
    }

}