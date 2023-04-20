using amiliur.shared.Reflection;

namespace amiliur.web.shared.Mapping;

public class DataObjectMapping
{
    public string? MappingAlias { get; set; }
    public string? Source { get; set; }
    public string? SourceAlias { get; set; }
    public string? Target { get; set; }
    public string? TargetAlias { get; set; }

    public DataObjectMapping()
    {
    }

    public DataObjectMapping(string? mappingAlias, Type sourceType, Type targetType)
    {
        MappingAlias = mappingAlias;
        Source = TypeUtils.GetFullTypeName(sourceType);
        Target = TypeUtils.GetFullTypeName(targetType);
        SourceAlias = "source";
        SourceAlias = "target";
    }

    public List<DataObjectFieldMapping> Fields { get; set; } = new();

    public object? InstantiateTarget()
    {
        return Activator.CreateInstance(Type.GetType(Target) ?? throw new Exception($"Cannot find type: {Target}"));
    }

    public DataObjectMapping AddDefaultFields()
    {
        var source = Type.GetType(Source);
        var target = Type.GetType(Target);

        if (source == null || target == null)
            throw new Exception("Invalid types");

        var sourceProperties = source.GetProperties().Select(m => m.Name);
        var targetProperties = target.GetProperties().Where(m => m.CanWrite).Select(m => m.Name);

        var commonProperties = sourceProperties.Intersect(targetProperties);
        foreach (var property in commonProperties)
        {
            Fields.Add(new DataObjectFieldMapping(property, property));
        }

        return this;
    }

    public DataObjectMapping AddField(string sourceField, string targetField)
    {
        Fields.Add(new DataObjectFieldMapping(sourceField, targetField));
        return this;
    }
}