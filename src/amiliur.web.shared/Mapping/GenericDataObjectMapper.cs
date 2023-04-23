using amiliur.shared.Json;
using amiliur.shared.Reflection;

namespace amiliur.web.shared.Mapping;

/// <summary>
/// Para quando os objetos tem o mesmos nomes
/// </summary>
public class GenericDataObjectMapper
{
    public GenericDataObjectMapper()
    {
    }

    public object Map(DataObjectMappingCollection objectMappings, ISerializableModel source)
    {
        var mapping = objectMappings.GetMapping(source.GetType());
        return Map(source, mapping, null);
    }

    public static object Map(object source, DataObjectMapping mapping, object target)
    {
        target ??= mapping.InstantiateTarget();

        var targetProperties = target.GetProperties().Select(p => p.Name).ToList();
        
        var dataObjectFieldMappings = mapping
            .Fields
            .Where(fieldMap => targetProperties.Contains(fieldMap.Target) 
                                && target.IsWritableProperty(fieldMap.Target));

        foreach (var fieldMap in dataObjectFieldMappings)
            target.SetPropertyValue(fieldMap.Target, source.GetPropertyValue(fieldMap.Source));

        return target;
    }

    public static object Map(object source, DataObjectMappingCollection mappings, object target)
    {
        if (source == null || target == null)
            throw new Exception("Cannot pass null");
        var map = mappings.GetMapping(source, target);
        return Map(source, map, target);
    }
}