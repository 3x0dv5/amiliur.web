using amiliur.web.shared.Attributes.Datagrid.Enums;

namespace amiliur.web.shared.Attributes.Datagrid.Attributes.GridColAttributes;

[AttributeUsage(AttributeTargets.Property)]
public class DeleteButtonGridColAttribute : ButtonGridColAttribute
{
    public DeleteButtonGridColAttribute()
    {
        TypeOfButton = TypeOfButton.Delete;
        Tooltip = "Remover";  // TODO: Translate
    }
}