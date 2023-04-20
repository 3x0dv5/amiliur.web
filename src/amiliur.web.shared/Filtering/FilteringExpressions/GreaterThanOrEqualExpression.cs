using System.Reflection;
using amiliur.web.shared.Models;
using JetBrains.Annotations;

namespace amiliur.web.shared.Filtering.FilteringExpressions;

[UsedImplicitly]
public class GreaterThanOrEqualExpression : BaseExpressionService
{
    public GreaterThanOrEqualExpression(Type elementType, ISearchInputModel input, PropertyInfo p) : base(elementType, input, p)
    {
    }

    protected override string Operator()
    {
        return ">=";
    }
}