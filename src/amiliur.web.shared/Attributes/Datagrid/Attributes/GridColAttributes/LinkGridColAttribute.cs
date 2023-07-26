using amiliur.web.shared.Attributes.Datagrid.Enums;

namespace amiliur.web.shared.Attributes.Datagrid.Attributes.GridColAttributes;

[AttributeUsage(AttributeTargets.Property)]
public class LinkGridColAttribute : TextGridColAttribute
{
    public override TypeOfColumn TypeOfColumn => TypeOfColumn.Link;
    public string? LinkFormat { get; set; }
}