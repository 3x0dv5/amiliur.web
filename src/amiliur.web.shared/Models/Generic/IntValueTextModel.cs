namespace amiliur.web.shared.Models.Generic;

public class IntValueTextModel : ValueTextModel<int>
{
    public static List<IntValueTextModel> List(int[] possibleValues)
    {
        return possibleValues.Select(m => new IntValueTextModel {Text = m.ToString(), Value = m}).ToList();
    }
}