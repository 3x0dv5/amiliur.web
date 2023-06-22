using System.Linq.Expressions;
using System.Reflection;
using amiliur.shared.Extensions;
using amiliur.shared.Reflection;
using amiliur.web.shared.Comparing;
using amiliur.web.shared.Filtering;

namespace amiliur.web.shared.Models.Generic;

public abstract class BaseSearchInputModel : ISearchInputModel
{
    private DateTime _ts = DateTime.UtcNow.WithUtcKind();

    public DateTime Ts
    {
        get => _ts;
        set => _ts = value.WithUtcKind();
    }

    public DateOnly AsOf { get; set; } = DateTime.Now.AsDateOnly();

    public virtual bool HasFilters(FilterEnvironment env)
    {
        var environments = new List<FilterEnvironment> {env};
        if (env != FilterEnvironment.Both)
        {
            environments.Add(FilterEnvironment.Both);
        }

        foreach (var environment in environments)
        {
            var properties = GetFilterProperties(environment);

            foreach (var p in properties)
            {
                var propertyType = p.PropertyType;

                switch (propertyType.GetNonNullableType().Name.ToLower())
                {
                    case "string":
                        if (!string.IsNullOrEmpty(this.GetPropertyValue(p.Name)?.ToString()))
                        {
                            return true;
                        }

                        break;
                    case "boolean":
                        return true;

                    default:
                        if (this.GetPropertyValue(p.Name) != null)
                        {
                            return true;
                        }

                        break;
                }
            }
        }

        return false;
    }

    public List<PropertyInfo> GetFilterProperties(FilterEnvironment env)
    {
        return GetType()
            .GetProperties()
            .Where(m => CustomAttributeExtensions.GetCustomAttribute<FilterAttribute>((MemberInfo) m) != null
                        && new List<FilterEnvironment> {FilterEnvironment.Both, env}.Contains(
                            CustomAttributeExtensions.GetCustomAttribute<FilterAttribute>((MemberInfo) m).FilterEnvironment))
            .ToList();
    }


    private Type TypeCompareClass(PropertyInfo property)
    {
        var nonNullableType = property.PropertyType.GetNonNullableType();

        switch (nonNullableType.Name.ToLower())
        {
            case "string":
                return typeof(StringCompare); 
            case "char":
                return typeof(CharCompare);
            case "datetime":
            case "dateonly":
                return typeof(DateTimeCompare);
            default:
                throw new Exception($"TypeCompareClass not found for {nonNullableType.Name} | {property.PropertyType.FullName}");
        }
    }


    private Expression<Func<TResult, bool>> Filter<TResult>(PropertyInfo filterValueProperty, PropertyInfo dataValueProperty, String methodName)
    {
        ParameterExpression parameter = Expression.Parameter(typeof(TResult), "m");
        var memberExpressionDataValueProperty = Expression.Property(parameter, dataValueProperty.Name);
        ConstantExpression filterValueExpression = Expression.Constant(this.GetPropertyValue(filterValueProperty.Name), filterValueProperty.PropertyType);

        Type typeCompareClass = TypeCompareClass(filterValueProperty);
        if (typeCompareClass != null)
        {
            var compareMethodInfo = typeCompareClass.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public, new[] {dataValueProperty.PropertyType, filterValueProperty.PropertyType});

            if (compareMethodInfo != null)
            {
                var callExpression = Expression.Call(null, compareMethodInfo, memberExpressionDataValueProperty, filterValueExpression);
                return Expression.Lambda<Func<TResult, bool>>(callExpression, parameter);
            }

            throw new Exception($"Could not find method {typeCompareClass.Name}.{methodName} for types {dataValueProperty.PropertyType.FullName}, {filterValueProperty.PropertyType.FullName}");
        }

        return null;
    }

    private Expression<Func<TResult, bool>> GreaterOrEqualFilter<TResult>(
        PropertyInfo filterValueProperty,
        PropertyInfo dataValueProperty)
    {
        return Filter<TResult>(filterValueProperty, dataValueProperty, "GreaterThanOrEqual");
    }

    private Expression<Func<TResult, bool>> LessOrEqualFilter<TResult>(PropertyInfo filterValueProperty, PropertyInfo dataValueProperty)
    {
        return Filter<TResult>(filterValueProperty, dataValueProperty, "LessThanOrEqual");
    }

    private Expression<Func<TResult, bool>> ContainsFilter<TResult>(PropertyInfo filterValueProperty, PropertyInfo dataValueProperty)
    {
        return Filter<TResult>(filterValueProperty, dataValueProperty, "Contains");
    }

    private Expression<Func<TResult, bool>> EqualsFilter<TResult>(PropertyInfo filterValueProperty, PropertyInfo dataValueProperty)
    {
        return Filter<TResult>(filterValueProperty, dataValueProperty, "Equals");
    }


    //TODO: parece que ja nao se usa
    public Expression<Func<TResult, bool>> GetClientFilterExpression<TResult>(PropertyInfo filterProperty, IEnumerable<PropertyInfo> filteredObjectProperties)
    {
        if (this.GetPropertyValue(filterProperty.Name) == null)
            return null;

        var filterType = filterProperty.GetFilterType();
        var valueProperty = filterProperty.GetMappedProperty(filteredObjectProperties);
        if (valueProperty == null)
        {
            throw new Exception($"Property {filterProperty.Name} does not have a valid map to {typeof(TResult).FullName}");
        }

        switch (filterType)
        {
            case WhereFilterType.Equal:
                return EqualsFilter<TResult>(filterProperty, valueProperty);
            case WhereFilterType.Contains:
                return ContainsFilter<TResult>(filterProperty, valueProperty);
            case WhereFilterType.GreaterThanOrEqual:
                return GreaterOrEqualFilter<TResult>(filterProperty, valueProperty);
            case WhereFilterType.LessThanOrEqual:
                return LessOrEqualFilter<TResult>(filterProperty, valueProperty);
        }

        return null;
    }
}