using amiliur.web.shared.Attributes.Datagrid.Enums;

namespace amiliur.web.shared.Attributes.Datagrid.Attributes.GridColAttributes;

[AttributeUsage(AttributeTargets.Property)]
public class DecimalGridColAttribute : GridColAttribute
{
    public override TypeOfColumn TypeOfColumn => TypeOfColumn.Decimal;
    public int DecimalPlaces { get; set; } = 2;
}