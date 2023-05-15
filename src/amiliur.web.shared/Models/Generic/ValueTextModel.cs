namespace amiliur.web.shared.Models.Generic;

/*public class ValueTextModel<T>
{
    public T Value { get; set; }
    public string Text { get; set; }
}

public class ValueTextModel : ValueTextModel<string>
{
    public ValueTextModel()
    {
    }

    public ValueTextModel(string value, string text)
    {
        Value = value;
        Text = text;
    }

    /// <summary>
    /// Cria uma lista de meses backwards apartir do lastDay. O resultado vem como ValueTextModel [ MMMM yyyy , yyyy-MM ]
    /// </summary>
    /// <param name="lastDay"></param>
    /// <param name="numMonths"></param>
    /// <returns></returns>
    public static List<ValueTextModel> GenerateListOfMonths(DateTime lastDay, int numMonths)
    {
        // TODO: move into a more suitable place
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

        var firstMonthDays = new List<DateTime>();
        foreach (var mTuple in months)
        {
            firstMonthDays.Add(new DateTime(mTuple.Item1, mTuple.Item2, 1));
        }

        var listValues = new List<ValueTextModel>();
        foreach (var m in firstMonthDays)
        {
            listValues.Add(new ValueTextModel
            {
                Text = $"{m.ToString("MMMM yyyy")}",
                Value = $"{m.ToString("yyyy-MM")}"
            });
        }

        return listValues.OrderByDescending(m => m.Value).ToList();
    }
}*/