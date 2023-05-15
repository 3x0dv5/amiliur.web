using amiliur.web.shared.Forms;

namespace amiliur.web.shared.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class FormDefinitionAttribute : Attribute
{
    public string FormTitle { get; set; }
    public string FormContext { get; set; }
    public string FormModule { get; set; }
    public string FormCode { get; set; }
    public string FormName { get; set; }
    public FormMode FormMode { get; set; }
    public string Description { get; set; } = "";
    public string DataTypeName { get; set; } = null!;

    public string ApiUrl { get; set; } = null!;

    public FormDefinitionAttribute(string formContext, string formModule, string formCode, string formName, FormMode formMode, string formTitle)
    {
        FormContext = formContext;
        FormModule = formModule;
        FormCode = formCode;
        FormName = formName;
        FormMode = formMode;
        FormTitle = formTitle;
    }
}