using amiliur.web.shared.Attributes.Datagrid.Enums;

namespace amiliur.web.shared.Attributes.Datagrid.Attributes.GridColAttributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class CompoundGridColAttribute : GridColAttribute
{
    public override TypeOfColumn TypeOfColumn => TypeOfColumn.Compound;
}