using System.Globalization;
using System.Reflection;
using amiliur.shared.Reflection;
using amiliur.web.shared.Models;
using JetBrains.Annotations;
using Serilog;

namespace amiliur.web.shared.Filtering.FilteringExpressions;

public abstract class BaseExpressionService
{
    protected readonly Type _elementType;
    protected readonly ISearchInputModel Input;
    protected readonly PropertyInfo PropertyInfo;

    public BaseExpressionService(Type elementType, ISearchInputModel input, PropertyInfo p)
    {
        _elementType = elementType;
        Input = input;
        PropertyInfo = p;
    }

    protected T As<T>()
    {
        return (T) Input.GetPropertyValue(PropertyInfo.Name);
    }

    public string Exp()
    {
        return DoExp();
    }

    private Tuple<string, Type> _mapping = null;

    protected Tuple<string, Type> Mapping
    {
        get { return _mapping ??= GetMapping(); }
    }

    protected virtual string DoExp()
    {
        var methodName = $"{ResultType.GetNonNullableType().Name}Exp";
        var expMethodInfo = GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);

        return (string) expMethodInfo.Invoke(this, BindingFlags.Instance | BindingFlags.NonPublic, null, null, CultureInfo.DefaultThreadCurrentCulture);
    }

    protected string ResultFieldName => Mapping.Item1;
    protected Type ResultType => Mapping.Item2;
    protected Type InputType => PropertyInfo.PropertyType;
    protected Type ResultNoNullableType => ResultType.GetNonNullableType();
    protected Type InputNonNullableType => InputType.GetNonNullableType();


    private Tuple<string, Type> GetMapping()
    {
        return PropertyInfo.GetFilterMapping(_elementType.GetProperties());
    }

    protected abstract string Operator();

    [UsedImplicitly]
    protected virtual string StringExp()
    {
        return $"EF.Functions.Unaccent({ResultFieldName}) {Operator()} \"{As<string>()}\"";
    }

    [UsedImplicitly]
    protected virtual string DateTimeExp()
    {
        if (InputNonNullableType == typeof(DateTime))
            return $"{ResultFieldName} {Operator()} DateTime.Parse(\"{As<DateTime>()}\")";

        return $"{ResultFieldName} {Operator()} DateTime.Parse(\"{As<DateOnly>()}\")";
    }

    [UsedImplicitly]
    protected virtual string DateOnlyExp()
    {
        if (InputNonNullableType == typeof(DateTime))
            return $"{ResultFieldName} {Operator()} DateOnly.Parse(\"{As<DateTime>().ToString("yyyy-MM-dd")}\")";
        return $"{ResultFieldName} {Operator()} DateOnly.Parse(\"{As<DateOnly>()}\")";
    }

    [UsedImplicitly]
    protected virtual string BooleanExp()
    {
        if (InputType == null)
            return $"{ResultFieldName} is null";

        return $"{ResultFieldName} == {As<bool>()}";
    }
    
    public static string Expression(Type elementType, ISearchInputModel input, PropertyInfo p)
    {
        var _namespace = typeof(BaseExpressionService).Namespace;
        var type = Type.GetType($"{_namespace}.{p.GetFilterType()}Expression");
        Log.Information("TYPE: {type}", $"{_namespace}.{p.GetFilterType()}Expression");

        var _c = (BaseExpressionService) Activator.CreateInstance(type, elementType, input, p);
        return _c.Exp();
    }
    
}