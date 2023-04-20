using System.Globalization;
using amiliur.shared.Reflection;

namespace amiliur.shared.Extensions;

public static class DateTimeExtensions
{
    public static bool HasTime(this DateTime date)
    {
        return date.Hour > 0
               || date.Minute > 0
               || date.Second > 0
               || date.Millisecond > 0
               || date.Microsecond > 0;
    }

    public static string AsIsoFormat(this DateTime date)
    {
        return date.ToString(date.HasTime() ? "O" : "yyyy-MM-dd");
    }

    public static DateTime Parse(this string date)
    {
        return DateTime.ParseExact(date, "yyyy-MM-dd", new DateTimeFormatInfo());
    }

    public static string AsFileName(this DateTime dt)
    {
        return dt.ToString("yyyyMMdd_HHmmss");
    }

    public static DateTime RemoveTimezone(this DateTime d)
    {
        return new DateTime(
            d.Year,
            d.Month,
            d.Day,
            d.Hour,
            d.Minute,
            d.Second,
            d.Millisecond,
            DateTimeKind.Unspecified);
    }

    public static DateOnly LastDayOfMonth(this DateTime dt)
    {
        var lastDay = DateTime.DaysInMonth(dt.Year, dt.Month);
        return new DateOnly(dt.Year, dt.Month, lastDay);
    }

    public static DateOnly LastDayOfMonth(this DateOnly dt)
    {
        var lastDay = DateTime.DaysInMonth(dt.Year, dt.Month);
        return new DateOnly(dt.Year, dt.Month, lastDay);
    }

    public static DateTime LastSecondOfLastDayOfMonth(this DateTime dt)
    {
        var lastDay = dt.LastDayOfMonth();
        return new DateTime(lastDay.Year, lastDay.Month, lastDay.Day, 23, 59, 59);
    }

    public static DateOnly FirstDayNextMonth(this DateTime dt)
    {
        return dt.LastDayOfMonth().AddDays(1);
    }

    public static DateTime FirstSecondOfFirstDayOfMonth(this DateTime dt)
    {
        return new DateTime(dt.Year, dt.Month, 1, 0, 0, 0);
    }

    public static DateOnly FirstDayOfMonth(this DateTime dt)
    {
        return new DateOnly(dt.Year, dt.Month, 1);
    }

    public static DateOnly FirstDayOfMonth(this DateOnly dt)
    {
        return new DateOnly(dt.Year, dt.Month, 1);
    }

    public static DateOnly AsDateOnly(this DateTime date)
    {
        return DateOnly.FromDateTime(date);
    }

    public static DateOnly? AsDateOnly(this DateTime? date)
    {
        if (date == null)
            return null;
        return AsDateOnly(date.Value);
    }


    public static DateTime? AsDateTime(this DateOnly? date)
    {
        return AsDateTime(date, DateTimeKind.Unspecified);
    }

    public static DateTime AsDateTime(this DateOnly date)
    {
        return AsDateTime(date, DateTimeKind.Unspecified);
    }

    public static DateTime? AsDateTime(this DateOnly? date, DateTimeKind kind)
    {
        return date?.AsDateTime(kind);
    }


    public static DateTime AsDateTime(this DateOnly date, DateTimeKind kind)
    {
        return date.ToDateTime(new TimeOnly(), kind);
    }

    public static bool IsValidDate(this DateOnly? date)
    {
        return date == null && date.Value < new DateOnly(1970, 1, 1) &&
               date.Value > DateOnly.FromDateTime(DateTime.Today);
    }

    public static double IdadeMeses(this DateTime dataOrigem, DateTime? dataTarget = null)
    {
        return Math.Abs(DateOnly.FromDateTime(dataTarget ?? DateTime.Today).DayNumber -
                        DateOnly.FromDateTime(dataOrigem).DayNumber) / (365.25 / 12);
    }

    public static double IdadeAnos(this DateTime dataOrigem, DateTime? dataTarget = null)
    {
        return Math.Round(dataOrigem.IdadeMeses(dataTarget) / 12.0f, 1);
    }

    public static double IdadeMeses(this DateOnly dataOrigem, DateOnly? dataTarget = null)
    {
        return dataOrigem.AsDateTime().IdadeMeses(dataTarget.AsDateTime());
    }

    public static double IdadeAnos(this DateOnly dataOrigem, DateOnly? dataTarget = null)
    {
        return dataOrigem.AsDateTime().IdadeAnos(dataTarget.AsDateTime());
    }

    public static double IdadeMeses(this DateOnly? dataOrigem, DateOnly? dataTarget = null)
    {
        if (dataOrigem == null)
            return 0;
        return dataOrigem.Value.IdadeMeses(dataTarget);
    }

    public static double IdadeAnos(this DateOnly? dataOrigem, DateOnly? dataTarget = null)
    {
        if (dataOrigem == null)
            return 0;
        return dataOrigem.Value.IdadeAnos(dataTarget);
    }

    public static int IdadeDias(this DateTime dataOrigem, DateTime? dataTarget = null)
    {
        return Math.Abs(((dataTarget ?? DateTime.Today) - dataOrigem).Days);
    }

    public static int IdadeDias(this DateOnly dataOrigem, DateOnly? dataTarget = null)
    {
        return dataOrigem.AsDateTime().IdadeDias(dataTarget.AsDateTime());
    }

    public static int IdadeDias(this DateOnly? dataOrigem, DateOnly? dataTarget = null)
    {
        if (dataOrigem == null)
            return 0;
        return dataOrigem.Value.IdadeDias(dataTarget);
    }


    public static int AsInt(this DateTime date)
    {
        return int.Parse(date.ToString("yyyyMMdd"));
    }

    public static int AsInt(this DateOnly date)
    {
        return date.AsDateTime().AsInt();
    }

    public static DateTime FromIntRepr(this string date)
    {
        return DateTime.ParseExact(date, "yyyyMMdd", new DateTimeFormatInfo());
    }

    public static DateTime FromIntRepr(this int date)
    {
        return date.ToString().FromIntRepr();
    }
}

public static class DateUtils
{
    public static bool TryConvertToDateOnly<T>(T value, out DateOnly? dateOnly)
    {
        dateOnly = default;

        if (value == null)
        {
            dateOnly = null;
            return true;
        }

        if (value is DateOnly dtO)
        {
            dateOnly = dtO;
            return true;
        }

        if (value is DateTime dt)
        {
            dateOnly = DateOnly.FromDateTime(dt);
            return true;
        }

        if (value is DateTimeOffset dto)
        {
            dateOnly = DateOnly.FromDateTime(dto.DateTime);
            return true;
        }

        if (value is string str)
            if (DateOnly.TryParse(str, out var dateOnlyAux))
            {
                dateOnly = dateOnlyAux;
                return true;
            }

        return false;
    }

    public static TValue ConvertDateOnlyToTValue<TValue>(DateOnly? dateOnly)
    {
        if (dateOnly == null)
            return default;

        var targetType = typeof(TValue).GetNonNullableType();

        if (targetType == typeof(DateOnly)) return (TValue) (object) dateOnly;

        if (targetType == typeof(DateTime))
        {
            var dateTime = dateOnly.GetValueOrDefault().AsDateTime();
            return (TValue) (object) dateTime;
        }

        if (targetType == typeof(DateTimeOffset))
        {
            var dateTime = dateOnly.GetValueOrDefault().AsDateTime();
            var dateTimeOffset = new DateTimeOffset(dateTime);
            return (TValue) (object) dateTimeOffset;
        }

        if (targetType == typeof(string))
        {
            var dateString = dateOnly.ToString();
            return (TValue) (object) dateString;
        }

        throw new InvalidOperationException($"Unsupported type '{targetType}' for TValue.");
    }
}

public static class DateTimeNoTz
{
    public static DateTime UtcNow()
    {
        return DateTime.UtcNow.RemoveTimezone();
    }

    public static DateTime WithUtcKind(this DateTime dt)
    {
        return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond, DateTimeKind.Utc);
    }

    public static DateTime WithUnspecifiedKind(this DateTime dt)
    {
        return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond, DateTimeKind.Unspecified);
    }
}