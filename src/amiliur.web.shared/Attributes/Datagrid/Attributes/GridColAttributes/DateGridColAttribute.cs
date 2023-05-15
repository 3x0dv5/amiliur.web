using amiliur.web.shared.Attributes.Datagrid.Enums;

namespace amiliur.web.shared.Attributes.Datagrid.Attributes.GridColAttributes;

[AttributeUsage(AttributeTargets.Property)]
public class DateGridColAttribute : GridColAttribute
{
    public override TypeOfColumn TypeOfColumn => TypeOfColumn.Date;
}