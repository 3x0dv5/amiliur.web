namespace amiliur.web.shared.Models;

public class ValueTextModel<T>
{
    public T Value { get; set; } = default!;
    public string Text { get; set; } = "";
}

public class ValueTextModel
{
    public string Value { get; set; } = "";
    public string Text { get; set; } = "";

    public ValueTextModel()
    {
    }

    public ValueTextModel(string value, string text)
    {
        Value = value;
        Text = text;
    }

    /// <summary>
    /// Generate a list of months from a given date. The result comes as ValueTextModel [ MMMM yyyy , yyyy-MM ]
    /// </summary>
    /// <param name="lastDay"></param>
    /// <param name="numMonths"></param>
    /// <returns></returns>
    public static List<ValueTextModel> GenerateListOfMonths(DateTime lastDay, int numMonths)
    {
        var initialDay = lastDay.AddMonths(0 - numMonths).Date;
        var loopDay = initialDay;
        var months = new List<Tuple<int, int>>();
        while (loopDay <= lastDay)
        {
            var month = new Tuple<int, int>(loopDay.Year, loopDay.Month);
            if (!months.Contains(month))
                months.Add(month);
            loopDay = loopDay.AddDays(1);
        }

        var firstMonthDays = months
            .Select(mTuple => new DateTime(mTuple.Item1, mTuple.Item2, 1))
            .ToList();

        var listValues = firstMonthDays
            .Select(m => new ValueTextModel { Text = $"{m:MMMM yyyy}", Value = $"{m:yyyy-MM}" })
            .ToList();

        return listValues.OrderByDescending(m => m.Value).ToList();
    }
}