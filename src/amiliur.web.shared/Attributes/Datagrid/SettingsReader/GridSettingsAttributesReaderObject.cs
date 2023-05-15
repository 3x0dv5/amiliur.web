using System.Reflection;
using amiliur.shared.Extensions;
using amiliur.shared.Reflection;
using amiliur.web.shared.Attributes.Datagrid.Attributes;
using amiliur.web.shared.Attributes.Datagrid.Attributes.GridColAttributes;
using amiliur.web.shared.Attributes.Datagrid.Enums;
using amiliur.web.shared.Attributes.Datagrid.Models;
using amiliur.web.shared.Attributes.Datagrid.Models.ColumnTypes;

namespace amiliur.web.shared.Attributes.Datagrid.SettingsReader;

/// <summary>
/// Reads the grid settings and column settings from attributes
/// </summary>
public class GridSettingsAttributesReaderObject : IGridSettingsReader
{
    public DataGridSettings Read(Type tValueObj)
    {
        var settings = ReadGridSettings(tValueObj);

        var properties = tValueObj.GetRuntimeProperties();

        foreach (var property in properties)
        {
            var setting = ReadColFromProperty(property, settings);
            if (setting != null)
                settings.Columns.Add(setting);
        }

        settings.SortColumns();
        return settings;
    }

    private static DataGridSettings ReadGridSettings(MemberInfo tValueObj)
    {
        var att = tValueObj.GetCustomAttribute<DataGridSettingsAttribute>();

        if (att == null) return new();
        var settings = new DataGridSettings();

        foreach (var aProperty in att.GetType().GetProperties())
        {
            var cP = att.GetProperty(aProperty.Name);
            if (cP == null || !cP.CanWrite)
                continue;
            var v = PropertyUtils.GetPropertyValue(att, aProperty.Name);
            Console.WriteLine($"{aProperty.Name}: {v}");
            settings.SetPropertyValue(cP.Name, v);
        }

        return settings;
    }

    private static GridColBase ReadColFromProperty(PropertyInfo property, DataGridSettings gridSettings)
    {
        return ColFromPropertyAttribute(property, gridSettings);
    }

    private static GridColBase ReadColFromPropertyDefault(PropertyInfo property)
    {
        var renderName = StringUtils.ToSnakeCase($"col_{property.Name}");

        switch (property.PropertyType.GetNonNullableType().Name)
        {
            case "String":
                return new TextCol(property.Name, renderName);
            case "DateTime":
                return new DateCol(property.Name, renderName);
            case "DateOnly":
                return new DateCol(property.Name, renderName);
            case "Boolean":
                return new BooleanCol(property.Name, renderName);
            default:
                throw new Exception($"Column type not supported {property.Name} > {property.PropertyType.GetNonNullableType().Name}");
        }
    }

    private static GridColBase ColFromPropertyAttribute(PropertyInfo property, DataGridSettings gridSettings)
    {
        var atts = property.GetCustomAttributes<GridColAttribute>().ToList();

        // mesmo sem attribute mostrar com o tipo default
        if (!atts.Any() && gridSettings.UseDefaultColumnTypesFromProperties)
            return ReadColFromPropertyDefault(property);

        if (!atts.Any() || atts.Any(m => m.TypeOfColumn == TypeOfColumn.Ignored))
            return null;

        return ColFromPropertyFromType(property, atts);
    }

    private static bool PropertyDoesHasConditionalRenderers(PropertyInfo property)
    {
        return property.GetCustomAttributes<ConditionalRendererAttribute>().Any() || property.GetCustomAttributes<ConditionalExprAttribute>().Any();
    }

    private static GridColBase ColFromPropertyFromType(PropertyInfo property, IEnumerable<GridColAttribute> colAtt)
    {
        var gridColAttributes = colAtt as GridColAttribute[] ?? colAtt.ToArray();
        if (gridColAttributes.Length == 1 && !PropertyDoesHasConditionalRenderers(property))
            return gridColAttributes.Single().ToControl(property);

        if (!PropertyDoesHasConditionalRenderers(property))
            throw new($"Multiple column attributes requires conditional renderers to be applied to the property {property.Name}");

        return CompoundColFromPropertyFromType(property, gridColAttributes);
    }

    private static CompoundCol CompoundColFromPropertyFromType(PropertyInfo property, GridColAttribute[] colAtt)
    {
        var conditionalRenderers = property.GetCustomAttributes<ConditionalBaseRendererAttribute>();
        var conditions = conditionalRenderers.Select(r => r.ToCondition()).ToList();

        var controls = colAtt
            .Where(c => c.TypeOfColumn != TypeOfColumn.Compound)
            .Select(c => c.ToControl(property))
            .ToList();

        var compoundColumn = (CompoundCol) CompoundCol.Instance(property.Name, controls, conditions);
        var compoundColAttribute = property.GetCustomAttribute<CompoundGridColAttribute>();

        if (compoundColAttribute != null)
            CopyPropertiesFromAttributeToCompoundCol(compoundColAttribute, compoundColumn);

        return compoundColumn;
    }


    private static void CopyPropertiesFromAttributeToCompoundCol(GridColAttribute attribute, CompoundCol columnObj)
    {
        if (attribute == null || columnObj == null)
            throw new ArgumentException("Both attribute and columnObj must be not null");

        foreach (var aProperty in attribute.GetType().GetProperties())
        {
            var prop = columnObj.GetProperty(aProperty.Name);
            if (prop != null && prop.CanWrite)
                PropertyUtils.CopyPropertyValue(attribute, columnObj, aProperty.Name, prop);
        }
    }
}