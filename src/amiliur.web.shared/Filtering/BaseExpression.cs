using System.Reflection;
using amiliur.shared.Reflection;

namespace amiliur.web.shared.Filtering;

public abstract class BaseExpression
{
    protected readonly object Input;

    internal BaseExpression(object input)
    {
        Input = input;
    }

    protected Type InputType(PropertyInfo property) => property.PropertyType;

    private Tuple<string, Type> GetMapping(PropertyInfo property, Type objectType)
    {
        return property.GetFilterMapping(objectType.GetProperties());
    }

    private string DefaultCompareMethodName => GetType().Name[..^"Expression".Length]; // range a partir do fim ..^
    private string StringValue(PropertyInfo property) => Input.GetPropertyValue(property.Name) as string;
    
    

    private T As<T>(PropertyInfo property)
    {
        return (T)Input.GetPropertyValue(property.Name);
    }

    public string Exp(PropertyInfo property, Type objectType)
    {
        var propertyType = property.PropertyType;

        var mapping = GetMapping(property, objectType);
        Console.WriteLine($"Compare method: {DefaultCompareMethodName}");

        switch (propertyType.GetNonNullableType().Name.ToLower())
        {
            case "string":
                return $"StringCompare.{DefaultCompareMethodName}({mapping.Item1}, \"{StringValue(property)}\")";
            case "datetime":
                return $"DateTimeCompare.{DefaultCompareMethodName}({mapping.Item1}, DateTime.Parse(\"{As<DateTime>(property)}\"))";
            case "dateonly":
                return $"DateTimeCompare.{DefaultCompareMethodName}({mapping.Item1}, DateTime.Parse(\"{As<DateOnly>(property)}\"))";
            case "boolean":
                return $"BooleanCompare.{DefaultCompareMethodName}({mapping.Item1}, Boolean.Parse(\"{As<bool>(property)}\"))";
            case "char":
                return $"CharCompare.{DefaultCompareMethodName}({mapping.Item1}, '{Input.GetPropertyValue<char>(property.Name)}')";
            default:
                throw new Exception($"Tipo {propertyType.GetNonNullableType().Name.ToLower()} nao suportado para a propriedade: {property.Name}");
        }
    }

    public string Exp(string fieldName, Type fieldType, string fieldValue)
    {
        var compareMethodName = GetType().Name[..^"Expression".Length];
        Console.WriteLine($"Compare w2 {GetType().Name} method: {compareMethodName}");
        
        switch (fieldType.GetNonNullableType().Name.ToLower())
        {
            case "string":
                return $"StringCompare.{compareMethodName}({fieldName}, \"{fieldValue}\")";
            case "datetime":
                return $"DateTimeCompare.{compareMethodName}({fieldName}, DateTime.Parse(\"{fieldValue}\"))";
            case "dateonly":
                return $"DateTimeCompare.{compareMethodName}({fieldName}, DateTime.Parse(\"{fieldValue}\"))";
            case "boolean":
                return $"BooleanCompare.{compareMethodName}({fieldName}, Boolean.Parse(\"{fieldValue}\"))";
            case "char":
                return $"CharCompare.{compareMethodName}({fieldName}, '{fieldValue}'))";
            default:
                throw new Exception($"Tipo {fieldType.GetNonNullableType().Name.ToLower()} nao suportado para a propriedade: {fieldName}");
        }
    }

    protected virtual string DoExp()
    {
        return "";
    }
}


public class ContainsExpression : BaseExpression
{
    public ContainsExpression(object input) : base(input)
    {
    }
}

public class EqualsExpression : BaseExpression
{
    public EqualsExpression(object input) : base(input)
    {
    }
}

public class GreaterThanOrEqualExpression : BaseExpression
{
    public GreaterThanOrEqualExpression(object input) : base(input)
    {
    }
}

public class LessThanOrEqualExpression : BaseExpression
{
    public LessThanOrEqualExpression(object input) : base(input)
    {
    }
}
public class StartsWithExpression : BaseExpression
{
    public StartsWithExpression(object input) : base(input)
    {
    }
}