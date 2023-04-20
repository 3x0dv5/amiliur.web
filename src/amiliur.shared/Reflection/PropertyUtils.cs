using System.Collections;
using System.Reflection;
using amiliur.shared.Extensions;
using Serilog;

namespace amiliur.shared.Reflection;

public static class PropertyUtils
{
    static readonly MethodInfo? CastMethod = typeof(Enumerable).GetMethod("Cast");
    static readonly MethodInfo? ToListMethod = typeof(Enumerable).GetMethod("ToList");

    public static PropertyInfo? GetProperty(this object aClassObject, string propertyName)
    {
        return aClassObject
            .GetType()
            .GetProperty(propertyName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);
    }

    private static bool IsEnumerable(Type type)
    {
        return type.GetInterface(nameof(IEnumerable)) != null;
    }

    public static object? GetPropertyValue(this object aClassObject, string propertyName)
    {
        object? returnValue;
        try
        {
            returnValue = aClassObject.GetProperty(propertyName)?.GetValue(aClassObject, null);
        }
        catch (Exception e)
        {
            Log.Error(e, "Error getting property value: {}", propertyName);
            returnValue = null;
        }

        return returnValue;
    }

    public static bool SetPropertyValue(this object aClassObject, string propertyName, object value)
    {
        try
        {
            var pi = aClassObject.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);
            if (pi == null || !pi.CanRead)
                return false;

            if (!pi.CanWrite)
                throw new ArgumentException($"Property {pi.Name} of object {aClassObject.GetType().Name} cannot be set ");

            // se passamos por exemplo um string vazia para um inteiro consideremos null.
            if (value != null
                && pi.GetType() != value.GetType()
                && pi.PropertyType.Name != "String"
                && value is string
                && string.IsNullOrEmpty(value.ToString()?.Trim()))
            {
                value = null;
            }

            if (value == null)
            {
                pi.SetValue(aClassObject, null, null);
                return true;
            }

            var type = pi.PropertyType.GetNonNullableType();
            if (type.IsGenericType)
            {
                return SetPropertyValueGeneric(aClassObject, value, type, pi);
            }
            else
            {
                return SetPropertyValueNonGeneric(aClassObject, value, type, pi);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "SetPropertyValue({}, {})", propertyName, value);
            var strDict = new Dictionary<string, object>
            {
                {"aClassObject.GetType().Name", aClassObject.GetType().Name},
                {"PropertyName", propertyName},
                {"value", value},
                {"ex.StackTrace", ex.StackTrace}
            };
            if (value != null)
            {
                strDict.Add("value.GetType().Name", value.GetType().Name);
            }

            foreach (var o in strDict)
            {
                Log.Debug($"{o.Key}: {o.Value}");
            }

            throw;
        }
    }

    public static IEnumerable<PropertyInfo> GetProperties(this object obj)
    {
        var t = obj.GetType().FullName == "System.RuntimeType" ? (Type) obj : obj.GetType();
        return t.GetProperties().ToList();
    }

    private static bool SetPropertyValueGeneric(object aClassObject, object value, Type type, PropertyInfo pi)
    {
        if (IsEnumerable(type))
        {
            var castItems = CastMethod?.MakeGenericMethod(type.GenericTypeArguments[0])
                .Invoke(null, new object[] {(IEnumerable) value});

            var list = ToListMethod?.MakeGenericMethod(type.GenericTypeArguments[0])
                .Invoke(null, new[] {castItems});

            pi.SetValue(aClassObject, list);
            return true;
        }

        try
        {
            pi.SetValue(aClassObject, value);
            return true;
        }
        catch (Exception e)
        {
            Log.Error("SetPropertyValueGeneric: object {}, property: {}, type: {}", aClassObject.GetType().Name, pi.Name, type.Name);
            throw;
        }
    }

    private static string EnumToString(object value)
    {
        return value.ToString();
    }

    private static string ObjectToString(object value)
    {
        if (value.GetType().IsEnum)
            return EnumToString(value);

        if (value is DateTime time)
            return time.AsIsoFormat();

        return value.ToString();
    }

    private static void SetStringProperty(object aClassObject, object value, PropertyInfo pi)
    {
        pi.SetValue(aClassObject, ObjectToString(value), null);
    }

    private static void SetCharProperty(object aClassObject, object value, PropertyInfo pi)
    {
        if (value is string strVal)
        {
            if (strVal.Length > 0)
            {
                pi.SetValue(aClassObject, strVal[0], null);
            }
        }
        else
        {
            pi.SetValue(aClassObject, value, null);
        }
    }

    private static void SetDateTimeValue(object aClassObject, PropertyInfo pi, object value)
    {
        if (string.IsNullOrEmpty(value.ToString()))
        {
            pi.SetValue(aClassObject, null);
        }

        else
        {
            if (value is DateTime)
            {
                pi.SetValue(aClassObject, value);
            }
            else
            {
                pi.SetValue(aClassObject, value.ToString().Parse());
            }
        }
    }

    private static bool SetPropertyValueNonGeneric(object aClassObject, object value, Type type, PropertyInfo pi)
    {
        switch (type.Name.ToLower())
        {
            case "string":
                SetStringProperty(aClassObject, value, pi);
                break;
            case "char":
                SetCharProperty(aClassObject, value, pi);
                break;
            case "int32":
                switch (value.GetType().Name.ToLower())
                {
                    case "string":
                        if (int.TryParse(value.ToString(), out var intVal))
                        {
                            pi.SetValue(aClassObject, intVal, null);
                        }
                        else
                        {
                            throw new ApplicationException($"{value} not an integer value");
                        }

                        break;
                    default:
                        pi.SetValue(aClassObject, value, null);
                        break;
                }

                break;

            case "boolean":
                switch (value.GetType().Name)
                {
                    case "string":
                        var strVal = value.ToString().ToLower();
                        if (bool.TryParse(strVal, out var boolVal))
                        {
                            pi.SetValue(aClassObject, boolVal, null);
                        }
                        else if (strVal.StartsWith("t") || strVal.StartsWith("y"))
                        {
                            pi.SetValue(aClassObject, true, null);
                        }
                        else if (strVal.StartsWith("f") || strVal.StartsWith("n"))
                        {
                            pi.SetValue(aClassObject, false, null);
                        }
                        else
                        {
                            throw new ApplicationException($"{value} not an bool value");
                        }

                        break;
                    default:
                        pi.SetValue(aClassObject, value, null);
                        break;
                }

                break;
            case "datetime":
                SetDateTimeValue(aClassObject, pi, value);
                break;
            default:
                if (pi.PropertyType.BaseType != null)
                {
                    switch (pi.PropertyType.BaseType.FullName?.ToLower())
                    {
                        case "system.enum":
                            pi.SetValue(aClassObject, Enum.Parse(pi.PropertyType, value.ToString() ?? throw new InvalidOperationException(), true), null);
                            break;
                        default:
                            pi.SetValue(aClassObject, value, null);
                            break;
                    }
                }
                else
                {
                    pi.SetValue(aClassObject, value, null);
                }

                break;
        }

        return true;
    }

    public static bool HasProperty(this object aClassObject, string propertyName)
    {
        return aClassObject.GetProperty(propertyName) != null;
    }

    /// <summary>
    /// Returns the first property that matches one of the possible property names
    /// </summary>
    /// <param name="aClassObject"></param>
    /// <param name="possiblePropertyNames"></param>
    /// <returns></returns>
    public static PropertyInfo? GetProperty(this object aClassObject, List<string> possiblePropertyNames)
    {
        return (from name
                    in possiblePropertyNames
                where aClassObject.HasProperty(name)
                select aClassObject.GetProperty(name)
            )
            .FirstOrDefault();
    }

    public static bool IsWritableProperty(this object aClassObject, string propertyName)
    {
        var prop = aClassObject.GetProperty(propertyName);
        return prop != null && prop.CanWrite;
    }

    public static T GetPropertyValue<T>(this object aClassObject, string propertyName)
    {
        return (T) aClassObject.GetPropertyValue(propertyName);
    }
}