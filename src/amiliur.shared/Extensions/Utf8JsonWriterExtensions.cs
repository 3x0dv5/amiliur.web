using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using amiliur.shared.Json;
using amiliur.shared.Reflection;


namespace amiliur.shared.Extensions;

public static class Utf8JsonWriterExtensions
{
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static bool WriteProperty(this Utf8JsonWriter writer, PropertyInfo propertyInfo, object dataObject, JsonSerializerOptions options)
    {
        var propertyValue = dataObject.GetPropertyValue(propertyInfo.Name);

        if (propertyValue == null)
        {
            return WriteNull(writer, propertyInfo);
        }

        if (propertyInfo.PropertyType == typeof(Type))
        {
            writer.WriteString(propertyInfo.Name, $"{((Type) propertyValue).FullName}, {((Type) propertyValue).Assembly.GetName().Name}");
            return true;
        }

        if (NumberUtils.IsNumber(propertyValue))
        {
            return WriteNumber(writer, propertyInfo, propertyValue);
        }

        if (propertyInfo.PropertyType.GetNonNullableType() == typeof(string))
        {
            return WriteString(writer, propertyInfo, propertyValue);
        }

        if (propertyInfo.PropertyType.GetNonNullableType() == typeof(char))
        {
            return WriteChar(writer, propertyInfo, propertyValue);
        }

        if (propertyInfo.PropertyType.GetNonNullableType() == typeof(bool))
        {
            return WriteBoolean(writer, propertyInfo, propertyValue);
        }

        if (propertyInfo.PropertyType.GetNonNullableType() == typeof(DateOnly))
        {
            return WriteDateOnly(writer, propertyInfo, (DateOnly) propertyValue);
        }

        if (propertyInfo.PropertyType.GetNonNullableType() == typeof(DateTime))
        {
            return WriteDateTime(writer, propertyInfo, (DateTime) propertyValue);
        }

        if (propertyValue.GetType().IsEnum)
        {
            return WriteEnum(writer, propertyInfo, propertyValue);
        }


        if (propertyValue.GetType().GetInterfaces().Contains(typeof(System.Collections.IEnumerable)))
        {
            return WriteList(writer, propertyInfo, propertyValue, options);
        }

        if (propertyValue.GetType().GetInterfaces().Contains(typeof(ISerializableModel)))
        {
            writer.WritePropertyName(propertyInfo.Name);
            writer.WriteRawValue(propertyValue.ToJson(options));
            return true;
        }

        throw new Exception($"Type {propertyInfo.PropertyType} serialization not supported. Property {propertyInfo.Name}");
    }

    private static bool WriteDateTime(Utf8JsonWriter writer, PropertyInfo propertyInfo, DateTime propertyValue)
    {
        writer.WriteString(propertyInfo.Name, propertyValue.AsIsoFormat());
        return true;
    }

    private static bool WriteDateOnly(Utf8JsonWriter writer, PropertyInfo propertyInfo, DateOnly propertyValue)
    {
        writer.WriteString(propertyInfo.Name, propertyValue.ToString("yyyy-MM-dd"));
        return true;
    }

    private static bool WriteList(Utf8JsonWriter writer, PropertyInfo propertyInfo, object propertyValue, JsonSerializerOptions options)
    {
        writer.WritePropertyName(propertyInfo.Name);
        writer.WriteStartArray();

        foreach (var obj in (System.Collections.IEnumerable) propertyValue)
        {
            writer.WriteRawValue(obj.ToJson(options));
        }

        writer.WriteEndArray();
        return true;
    }

    private static bool WriteBoolean(Utf8JsonWriter writer, PropertyInfo propertyInfo, object propertyValue)
    {
        writer.WriteBoolean(propertyInfo.Name, (bool) propertyValue);
        return true;
    }

    private static bool WriteString(Utf8JsonWriter writer, PropertyInfo propertyInfo, object propertyValue)
    {
        writer.WriteString(propertyInfo.Name, propertyValue.ToString());
        return true;
    }

    private static bool WriteChar(Utf8JsonWriter writer, PropertyInfo propertyInfo, object propertyValue)
    {
        writer.WriteString(propertyInfo.Name, propertyValue.ToString());
        return true;
    }

    private static bool WriteNumber(Utf8JsonWriter writer, PropertyInfo propertyInfo, object propertyValue)
    {
        if (NumberUtils.IsDecimal(propertyValue))
        {
            writer.WriteNumber(propertyInfo.Name, (decimal) propertyValue);
            return true;
        }

        if (WriteFloatingPoint(writer, propertyInfo, propertyValue))
            return true;

        if (NumberUtils.IsInteger(propertyValue))
        {
            if (propertyInfo.PropertyType.GetNonNullableType() == typeof(long))
            {
                writer.WriteNumber(propertyInfo.Name, (long) propertyValue);
                return true;
            }

            if (propertyInfo.PropertyType.GetNonNullableType() == typeof(int))
            {
                writer.WriteNumber(propertyInfo.Name, (int) propertyValue);
                return true;
            }

            throw new Exception($"Invalid number type: {propertyValue} in property: {propertyInfo.Name}");
        }

        return false;
    }

    private static bool WriteFloatingPoint(Utf8JsonWriter writer, PropertyInfo propertyInfo, object propertyValue)
    {
        if (propertyValue is double)
        {
            writer.WriteNumber(propertyInfo.Name, (double) propertyValue);
            return true;
        }

        if (propertyValue is Single)
        {
            writer.WriteNumber(propertyInfo.Name, (float) propertyValue);
            return true;
        }

        return false;
    }

    private static bool WriteNull(Utf8JsonWriter writer, PropertyInfo propertyInfo)
    {
        writer.WriteNull(propertyInfo.Name);
        return true;
    }

    private static bool WriteEnum(Utf8JsonWriter writer, PropertyInfo propertyInfo, object propertyValue)
    {
        var enumType = propertyValue.GetType();
        var enumElem = enumType.GetMembers().SingleOrDefault(m => m.Name == propertyValue.ToString());
        if (enumElem != null && enumElem.GetCustomAttribute<EnumMemberAttribute>() != null)
        {
            writer.WriteString(propertyInfo.Name, enumElem.GetCustomAttribute<EnumMemberAttribute>()?.Value);
        }
        else
        {
            writer.WriteString(propertyInfo.Name, propertyValue.ToString());
        }

        return true;
    }
}