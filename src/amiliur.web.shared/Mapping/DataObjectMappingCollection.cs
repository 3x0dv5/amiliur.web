using amiliur.shared.Reflection;

namespace amiliur.web.shared.Mapping;

public class DataObjectMappingCollection : HashSet<DataObjectMapping>
{
    public DataObjectMapping GetMapping(Type source)
    {
        var mapping = this.SingleOrDefault(m => m.Source == TypeUtils.GetFullTypeName(source));
        if (mapping == null)
            throw new Exception("Mapping not found");
        return mapping;
    }

    public DataObjectMapping GetMapping(object source, object target)
    {
        var mapping = this.SingleOrDefault(m => m.Source == TypeUtils.GetFullTypeName(source.GetType()) && m.Target == TypeUtils.GetFullTypeName(target.GetType()));
        if (mapping == null)
            throw new Exception("Mapping not found");
        return mapping;
    }

    public DataObjectMapping GetMapping(string alias)
    {
        var mapping = this.SingleOrDefault(m => m.MappingAlias == alias);
        if (mapping == null)
            throw new Exception("Mapping not found");
        return mapping;
    }
}