using System.Reflection;
using amiliur.shared.Extensions;
using amiliur.web.shared.Models;
using JetBrains.Annotations;

namespace amiliur.web.shared.Filtering.FilteringExpressions;

[UsedImplicitly]
public class ContainsExpression: BaseExpressionService
{

    public ContainsExpression(Type elementType, ISearchInputModel input, PropertyInfo p) : base(elementType, input, p)
    {
    }

    protected override string Operator()
    {
        throw new NotSupportedException();
    }

    protected override string StringExp()
    {
        return $"EF.Functions.ILike(EF.Functions.Unaccent({ResultFieldName}), \"%{As<string>().RemoveDiacritics()}%\")";
    }

    protected override string DateTimeExp()
    {
        throw new NotSupportedException();
    }

    
}