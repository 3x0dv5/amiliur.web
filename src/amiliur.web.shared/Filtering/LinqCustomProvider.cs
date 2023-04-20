using System.Linq.Dynamic.Core.CustomTypeProviders;
using amiliur.web.shared.Comparing;

namespace amiliur.web.shared.Filtering;

public class LinqCustomProvider : DefaultDynamicLinqCustomTypeProvider
{
    public override HashSet<Type> GetCustomTypes()
    {
        var result = base.GetCustomTypes();

        result.Add(typeof(DateOnly));
        result.Add(typeof(StringCompare));
        result.Add(typeof(CharCompare));
        result.Add(typeof(DateTimeCompare));
        result.Add(typeof(IntCompare));
        result.Add(typeof(BooleanCompare));
        return result;
    }
} 