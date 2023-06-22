using amiliur.web.shared.Attributes.Datagrid.Enums;

namespace amiliur.web.shared.Attributes.Datagrid.Attributes.GridColAttributes;

[AttributeUsage(AttributeTargets.Property)]
public class ButtonGridColAttribute : GridColAttribute
{
    public override TypeOfColumn TypeOfColumn => TypeOfColumn.Button;
    public override TextAlignment? TextAlign => Enums.TextAlignment.Center;
    public TypeOfButton TypeOfButton { get; protected set; } = TypeOfButton.Edit;
    public string? UrlFormat { get; set; }
    public string? Text { get; set; }
    public string? DialogMessageTextFormat { get; set; }

    public ButtonGridColAttribute()
    {
        HeaderText = "";
        Tooltip = "Editar"; // TODO: Translate
        Width = "50";
    }
}