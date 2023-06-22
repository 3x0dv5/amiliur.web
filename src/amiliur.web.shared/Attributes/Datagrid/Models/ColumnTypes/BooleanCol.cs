using amiliur.web.shared.Attributes.Datagrid.Enums;

namespace amiliur.web.shared.Attributes.Datagrid.Models.ColumnTypes;

public class BooleanCol : GridColBase
{
    public override TypeOfColumn TypeOfColumn => TypeOfColumn.Boolean;

    public BooleanCol(string field, string name) : base(field, name)
    {
    }
}
