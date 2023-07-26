using amiliur.web.shared.Attributes.Datagrid.Enums;

namespace amiliur.web.shared.Attributes.Datagrid.Attributes.GridColAttributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class TextGridColAttribute : GridColAttribute
{
    public override TypeOfColumn TypeOfColumn => TypeOfColumn.Text;
    public OoriTextClipMode ClipMode { get; set; }
    
    /// <summary>
    /// For when we simply want to always show a text and not a text coming from a field.
    /// </summary>
    public string? StaticText { get; set; } 
}
