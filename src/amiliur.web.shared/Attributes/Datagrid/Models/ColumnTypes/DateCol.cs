using amiliur.web.shared.Attributes.Datagrid.Enums;

namespace amiliur.web.shared.Attributes.Datagrid.Models.ColumnTypes;

public class DateCol : GridColBase
{
    public override TypeOfColumn TypeOfColumn => TypeOfColumn.Date;

    public DateCol(string field, string name) : base(field, name)
    {
    }
}
