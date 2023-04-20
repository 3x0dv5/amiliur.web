using System.Reflection;
using amiliur.web.shared.Models;
using JetBrains.Annotations;

namespace amiliur.web.shared.Filtering.FilteringExpressions;

[UsedImplicitly]
public class EqualExpression : BaseExpressionService
{
    public EqualExpression(Type elementType, ISearchInputModel input, PropertyInfo p) : base(elementType, input, p)
    {
    }

    protected override string Operator()
    {
        return "=";
    }

    protected override string StringExp()
    {
        return $"EF.Functions.ILike(EF.Functions.Unaccent({ResultFieldName}), \"{As<string>()}\")";
    }

    protected override string DateTimeExp()
    {
        throw new NotSupportedException();
    }

   
}