using amiliur.web.shared.Environments;
using Fig.App.Web.Shared.OldFields;

namespace amiliur.web.shared.Filtering;

[AttributeUsage(AttributeTargets.Property)]
public class FilterAttribute : MappingAttribute
{
    public FilterEnvironment FilterEnvironment { get; set; } = FilterEnvironment.Both;
    public WhereFilterType WhereFilterType { get; set; } = WhereFilterType.Equal;
    
}