using amiliur.web.shared.Attributes.Interfaces;
using amiliur.web.shared.Forms;

namespace amiliur.web.shared.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class GenerateSlugAttribute : Attribute, IFormFieldAttribute
{
    public FormMode[] FormModes { get; set; } = null!;
    
    public string SourceField { get; set; } = null!;
}