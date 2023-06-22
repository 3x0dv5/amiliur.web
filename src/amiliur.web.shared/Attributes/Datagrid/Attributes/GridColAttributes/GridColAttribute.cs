using System.Reflection;
using amiliur.shared.Extensions;
using amiliur.shared.Reflection;
using amiliur.web.shared.Attributes.Datagrid.Enums;
using amiliur.web.shared.Attributes.Datagrid.Models;
using amiliur.web.shared.Attributes.Datagrid.Models.ColumnTemplates;
using amiliur.web.shared.Attributes.Datagrid.Models.ColumnTypes;
using Serilog;

namespace amiliur.web.shared.Attributes.Datagrid.Attributes.GridColAttributes;

public abstract class GridColAttribute : Attribute
{
    public bool IsConditional { get; set; }
    public string Tooltip { get; set; }
    public string RenderingName { get; set; }
    public abstract TypeOfColumn TypeOfColumn { get; }

    protected string _headerText;
    public bool HeaderTextSet { get; private set; }

    public string HeaderText
    {
        get
        {
            if (string.IsNullOrEmpty(_headerText) && !HeaderTextSet)
            {
                return null;
            }

            return _headerText;
        }
        set
        {
            _headerText = value;
            HeaderTextSet = true;
        }
    }

    public bool Visible { get; set; } = true;
    public int DisplayOrder { get; set; }
    public virtual TextAlignment? TextAlign { get; set; } = Enums.TextAlignment.Left;

    public string Width { get; set; } = string.Empty;

    public string TemplateName { get; set; } = string.Empty;

    public GridColBase ToControl(PropertyInfo property)
    {
        if (string.IsNullOrEmpty(RenderingName))
            RenderingName = StringUtils.ToSnakeCase($"col_{property.Name}");

        var control = DoToControl(property);
        return control;
    }

    protected virtual GridColBase DoToControl(PropertyInfo property)
    {
        var className = $"{typeof(TextCol).Namespace}.{TypeOfColumn}Col";
        var colType = Type.GetType(className);
        if (colType == null)
        {
            return DefaultColumnControl(property, className);
        }

        var colObj = Activator.CreateInstance(colType, args: new object[] {property.Name, RenderingName});
        ApplyDefaultPropertyValues(colType, colObj);
        ApplyTemplate(colObj);
        return (GridColBase) colObj;
    }

    private void ApplyTemplate(object colObj)
    {
        if (string.IsNullOrEmpty(TemplateName)) return;

        var refColumn = (GridColBase) colObj;
        refColumn.Template = ColumnTemplateManager.Instance().GetTemplate(TemplateName);
    }

    protected void ApplyDefaultPropertyValues(Type colType, object colObj)
    {
        foreach (var aProperty in GetType().GetProperties())
        {
            if (aProperty.Name == nameof(HeaderText) && !HeaderTextSet)
                continue;

            PropertyUtils.CopyPropertyValue(this, colObj, aProperty.Name, colType.GetProperty(aProperty.Name));
        }
    }

    private GridColBase DefaultColumnControl(PropertyInfo property, string className)
    {
        Log.Information($"class {className} not found, defaulting to Text");
        return new TextCol(property.Name, RenderingName);
    }
}