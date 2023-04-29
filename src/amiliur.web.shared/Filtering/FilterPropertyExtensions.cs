using System.Reflection;
using amiliur.shared.Extensions;

namespace amiliur.web.shared.Filtering;

public static class FilterPropertyExtensions
{
    public static WhereFilterType GetFilterType(this PropertyInfo property)
    {
        var attr = property.GetCustomAttribute<FilterAttribute>();
        if (attr != null)
        {
            return attr.WhereFilterType;
        }

        return WhereFilterType.Equal;
    }

    public static string[] GetMappingToNames(this PropertyInfo property)
    {
        var attr = property.GetCustomAttribute<MappingAttribute>();
        if (attr == null || attr.MappingTo.Length == 0)
            return new[] {property.Name};

        var a = new string[attr.MappingTo.Length + 1];
        attr.MappingTo.CopyTo(a, 0);
        // a[a.Length - 1] = property.Name;
        a[^1] = property.Name;
        return a.Distinct().ToArray();
    }

    public static PropertyInfo GetMappedProperty(this PropertyInfo property, IEnumerable<PropertyInfo> filterProperties)
    {
        var possibleMappings = property.GetMappingToNames();
        var propertyInfos = filterProperties.ToList();
        if (possibleMappings.Length == 1)
        {
            return propertyInfos.SingleOrDefault(m => m.Name == property.Name);
        }


        var existingProperties = filterProperties.Select(m => m.Name).ToList();
        foreach (var mapping in possibleMappings)
        {
            if (existingProperties.Contains(mapping))
                return propertyInfos.Single(m => m.Name == mapping);
        }

        return null;
    }

    /// <summary>
    ///     Gets the mapping for the property to the data property
    /// </summary>
    /// <param name="property"></param>
    /// <param name="dataProperties"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public static Tuple<string, Type> GetFilterMapping(this PropertyInfo property, IEnumerable<PropertyInfo> dataProperties)
    {
        var possibleMappings = property.GetMappingToNames();

        var existingProperties = dataProperties.ToList();
        foreach (var mapping in possibleMappings)
        {
            var propMapping = IsValidMapping(existingProperties, mapping);
            if (propMapping != null)
            {
                if (propMapping.PropertyType.IsPrimitive
                    || propMapping.PropertyType.BaseType == null
                    || (propMapping.PropertyType.BaseType.Name != "BaseEntity"))
                    return new Tuple<string, Type>(mapping, propMapping.PropertyType);
            }
        }

        throw new NotSupportedException($"Mapping not found for property: {property.Name} ");
    }

    private static PropertyInfo IsValidMapping(IEnumerable<PropertyInfo> dataProperties, string mapping)
    {
        if (dataProperties == null)
            return null;

        if (!mapping.Contains("."))
        {
            return dataProperties.SingleOrDefault(m => m.Name == mapping);
        }


        var firstProp = mapping.SubString(mapping.IndexOf('.'));
        var propertyInfos = dataProperties.ToList();

        if (IsValidMapping(propertyInfos, firstProp) == null)
            return null;

        var type = propertyInfos.First(m => m.Name == firstProp).PropertyType;
        return IsValidMapping(type.GetProperties(), mapping[(mapping.IndexOf('.') + 1)..]);
    }
}