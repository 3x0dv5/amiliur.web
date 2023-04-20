using System.Reflection;
using amiliur.shared.Reflection;
using Microsoft.AspNetCore.Components;

namespace amiliur.web.shared.Reflection;

public static class TypeUtils
{
    public static IEnumerable<PropertyInfo> GetEventCallbackProperties(this object obj)
    {
        var eventCallbackFullname = typeof(EventCallback).FullName;
        return obj
            .GetProperties()
            .Where(p =>
                eventCallbackFullname != null
                && p.PropertyType.FullName != null
                && p.CanWrite
                && p.GetCustomAttributes<ParameterAttribute>().Any()
                && p.PropertyType.IsGenericType && p.PropertyType.FullName.StartsWith(eventCallbackFullname)
            )
            .ToList();
    }
}