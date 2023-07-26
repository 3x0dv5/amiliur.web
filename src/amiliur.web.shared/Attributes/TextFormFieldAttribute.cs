namespace amiliur.web.shared.Attributes;

public class TextFormFieldAttribute : BaseFormFieldAttribute
{
    public bool Multiline { get; set; } = false;
    public bool ReadOnly { get; set; } = false;
}