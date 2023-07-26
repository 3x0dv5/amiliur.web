using System.ComponentModel.DataAnnotations;
using amiliur.web.shared.Attributes.Interfaces;
using amiliur.web.shared.Forms;

namespace amiliur.web.shared.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class RequiredFieldAttribute: RequiredAttribute, IFormFieldAttribute
{
    public FormMode[] FormModes { get; set; } = null!;
}