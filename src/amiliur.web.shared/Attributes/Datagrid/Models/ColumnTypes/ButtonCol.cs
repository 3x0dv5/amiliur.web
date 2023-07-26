using amiliur.web.shared.Attributes.Datagrid.Enums;

namespace amiliur.web.shared.Attributes.Datagrid.Models.ColumnTypes;

public class ButtonCol : GridColBase
{
    public override TypeOfColumn TypeOfColumn => TypeOfColumn.Button;
    public TypeOfButton TypeOfButton { get; set; } = TypeOfButton.Edit;
    public override TextAlignment? TextAlign { get; set; } = Enums.TextAlignment.Center;
    public override string Width { get; set; } = "50";
    public string? UrlFormat { get; set; }
    public string? Text { get; set; }
    public string? DialogMessageTextFormat { get; set; }

    public ButtonCol(string field, string name) : base(field, name)
    {
    }
}