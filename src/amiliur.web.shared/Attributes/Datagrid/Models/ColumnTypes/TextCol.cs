using amiliur.web.shared.Attributes.Datagrid.Enums;

namespace amiliur.web.shared.Attributes.Datagrid.Models.ColumnTypes;

public class TextCol : GridColBase
{
    public override TypeOfColumn TypeOfColumn => TypeOfColumn.Text;
    public OoriTextClipMode ClipMode { get; set; }
    public string? StaticText { get; set; }
    public TextCol(string field, string name) : base(field, name)
    {
    }
}
