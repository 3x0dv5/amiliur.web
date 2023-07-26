using amiliur.web.shared.Forms;

namespace amiliur.web.shared.Attributes.Interfaces;

public interface IFormFieldAttribute
{
    public FormMode[] FormModes { get; set; }
}