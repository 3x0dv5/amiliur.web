using System.Globalization;

namespace amiliur.shared.Utils;

public class DateTimeUtils
{
    public static DateTime? ParseFromIso(string? isotime)
    {
        if (isotime == null)
            return null;

        var provider = CultureInfo.InvariantCulture;

        // Check if isotime includes time or is date only
        if (isotime.Length > 10) // includes time
        {
            var format = "yyyy-MM-ddTHH:mm:ss.fffffffZ";
            return DateTime.ParseExact(isotime, format, provider, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        }
        else // date only
        {
            var format = "yyyy-MM-dd";
            return DateTime.SpecifyKind(DateTime.ParseExact(isotime, format, provider), DateTimeKind.Utc);
        }
    }
}