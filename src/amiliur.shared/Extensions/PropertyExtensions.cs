using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace amiliur.shared.Extensions;

public static class PropertyExtensions
{
    public static string? DisplayName(this PropertyInfo? property)
    {
        if (property == null)
            return "";
        var att = property.GetCustomAttribute<DisplayAttribute>();
        if (att == null)
            return property.Name;

        return att.Name;
    }
}