using amiliur.shared.Reflection;

namespace amiliur.shared.Extensions;

public static class NumberUtils
{
    public static bool IsDecimal(object number)
    {
        var nonNullableNumber = number.GetType().GetNonNullableType();
        return nonNullableNumber == typeof(decimal);
    }

    public static bool IsInteger(object number)
    {
        var nonNullableNumber = number.GetType().GetNonNullableType();

        return (nonNullableNumber == typeof(Int32)
                || nonNullableNumber == typeof(Int16)
                || nonNullableNumber == typeof(Int64)
                || nonNullableNumber == typeof(UInt32)
                || nonNullableNumber == typeof(UInt16)
                || nonNullableNumber == typeof(UInt64)
                || nonNullableNumber == typeof(Byte)
                || nonNullableNumber == typeof(SByte));
    }
    private static bool IsFloatingPoint(object number)
    {
        var nonNullableNumber = number.GetType().GetNonNullableType();
        return (nonNullableNumber == typeof(float) || nonNullableNumber == typeof(double));
    }

    public static bool IsNumber(object number)
    {
        return IsInteger(number) || IsDecimal(number) || IsFloatingPoint(number);
    }
}