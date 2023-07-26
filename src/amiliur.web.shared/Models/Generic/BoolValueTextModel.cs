namespace amiliur.web.shared.Models.Generic;

public class BoolValueTextModel : ValueTextModel<bool>
{
    public static List<BoolValueTextModel> List()
    {
        return new()
        {
            new() {Text = "Não", Value = false},
            new() {Text = "Sim", Value = true}
        };
    }
}