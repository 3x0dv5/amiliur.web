using System.Reflection;
using amiliur.shared.Reflection;
using Fig.App.Web.Shared.OldFields;
using Serilog;

namespace amiliur.web.shared.Filtering;

public static class ExpressionFilterGenerator
{
    public static string ExpressionString<TResult>(object input, List<PropertyInfo> properties)
    {
        var expressionString = "";

        foreach (var p in properties)
        {
            if (input.GetPropertyValue(p.Name) is null)
            {
                continue;
            }

            var exp = ExpressionForField(input, p, typeof(TResult));

            if (!string.IsNullOrEmpty(exp))
            {
                expressionString = string.IsNullOrEmpty(expressionString) ? exp : $"({expressionString}) && ({exp})";
            }
        }

        Log.Debug($"Expression String: {0}", expressionString);
        return expressionString;
    }

    private static string ExpressionForField(object input, PropertyInfo p, Type objectType)
    {
        switch (p.GetFilterType())
        {
            case WhereFilterType.Equal:
                return new EqualsExpression(input).Exp(p, objectType);
            case WhereFilterType.Contains:
                return new ContainsExpression(input).Exp(p, objectType);
            case WhereFilterType.None:
                break;
            case WhereFilterType.NotEqual:
                break;
            case WhereFilterType.LessThan:
                break;
            case WhereFilterType.GreaterThan:
                break;
            case WhereFilterType.LessThanOrEqual:
                return new LessThanOrEqualExpression(input).Exp(p, objectType);
            case WhereFilterType.GreaterThanOrEqual:
                return new GreaterThanOrEqualExpression(input).Exp(p, objectType);
            case WhereFilterType.NotContains:
                break;
            case WhereFilterType.StartsWith:
                return new StartsWithExpression(input).Exp(p, objectType);
            case WhereFilterType.NotStartsWith:
                break;
            case WhereFilterType.EndsWith:
                break;
            case WhereFilterType.NotEndsWith:
                break;
            case WhereFilterType.Any:
                break;
            case WhereFilterType.NotAny:
                break;
            case WhereFilterType.IsNull:
                break;
            case WhereFilterType.IsNotNull:
                break;
            case WhereFilterType.IsEmpty:
                break;
            case WhereFilterType.IsNotEmpty:
                break;
            case WhereFilterType.IsNullOrEmpty:
                break;
            case WhereFilterType.IsNotNullOrEmpty:
                break;
        }

        return null;
    }

    public static string ExpressionString(object input, string field, WhereFilterType _operator, string value)
    {
        var prop = input.GetProperty(field);
        return ExpressionForField(input, prop, _operator, value);
    }

    private static string ExpressionForField(object input, PropertyInfo p, WhereFilterType _operator, string value)
    {
        switch (_operator)
        {
            case WhereFilterType.Equal:
                return new EqualsExpression(input).Exp(p.Name, p.PropertyType, value);
            case WhereFilterType.Contains:
                return new ContainsExpression(input).Exp(p.Name, p.PropertyType, value);
            case WhereFilterType.None:
                break;
            case WhereFilterType.NotEqual:
                break;
            case WhereFilterType.LessThan:
                break;
            case WhereFilterType.GreaterThan:
                break;
            case WhereFilterType.LessThanOrEqual:
                return new LessThanOrEqualExpression(input).Exp(p.Name, p.PropertyType, value);
            case WhereFilterType.GreaterThanOrEqual:
                return new GreaterThanOrEqualExpression(input).Exp(p.Name, p.PropertyType, value);
            case WhereFilterType.NotContains:
                break;
            case WhereFilterType.StartsWith:
                return new StartsWithExpression(input).Exp(p.Name, p.PropertyType, value);
            case WhereFilterType.NotStartsWith:
                break;
            case WhereFilterType.EndsWith:
                break;
            case WhereFilterType.NotEndsWith:
                break;
            case WhereFilterType.Any:
                break;
            case WhereFilterType.NotAny:
                break;
            case WhereFilterType.IsNull:
                break;
            case WhereFilterType.IsNotNull:
                break;
            case WhereFilterType.IsEmpty:
                break;
            case WhereFilterType.IsNotEmpty:
                break;
            case WhereFilterType.IsNullOrEmpty:
                break;
            case WhereFilterType.IsNotNullOrEmpty:
                break;
        }

        return null;
    }
}
