using System.Collections;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using amiliur.shared.Reflection;
using amiliur.web.shared.Attributes.Datagrid.Models;
using JetBrains.Annotations;

namespace amiliur.web.shared.Json;

public class GridColBaseJsonConverter : JsonConverter<GridColBase>
{
    public override GridColBase Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, GridColBase value, JsonSerializerOptions options)
    {
        ObjJsonWriter.Write(writer, options, value);
    }
}

public static class ObjJsonConstants
{
    public const string ObjectClass = "__ObjectClass";
}

public static class ObjJsonWriter
{
    public static void Write(Utf8JsonWriter writer, JsonSerializerOptions options, Object obj)
    {
        writer.WriteStartObject();
        writer.WriteString(ObjJsonConstants.ObjectClass, obj.GetType().FullName);

        var properties = obj.GetProperties();
        foreach (var property in properties)
        {
            ValuesWriter.WritePropertyValue(writer, options, property, obj);
        }

        writer.WriteEndObject();
    }
}

public static class ValuesWriter
{
    public static void WritePropertyValue(Utf8JsonWriter writer, JsonSerializerOptions options, PropertyInfo property, Object obj)
    {
        Write(writer, options, property, property.PropertyType, obj);
    }

    private static void Write(Utf8JsonWriter writer, JsonSerializerOptions options, PropertyInfo property, Type propertyType, Object obj)
    {
        var methodName = $"Write{GetPropertyTypeName(propertyType)}";

        var method = typeof(ValuesWriter).GetMethod(methodName);
        if (method == null)
        {
            throw new NotImplementedException($"Method: {methodName} NOT IMPLEMENTED in class {nameof(ValuesWriter)}");
        }

        method.Invoke(null, new[] {writer, options, property, obj});
    }

    private static string GetPropertyTypeName(Type type)
    {
        if (type.IsEnum)
            return "Enum";

        if (type.IsAssignableTo(typeof(IList<>)) || type.IsAssignableTo(typeof(IList)))
        {
            return "IList";
        }

        if (type.IsNullableType())
        {
            return "Nullable";
        }

        return type.Name;
    }

    [UsedImplicitly]
    public static void WriteEnum(Utf8JsonWriter writer, JsonSerializerOptions options, PropertyInfo property, Object obj)
    {
        var value = ((Enum) obj.GetPropertyValue(property.Name)).ToString();
        writer.WriteString(property.Name, value);
    }

    [UsedImplicitly]
    public static void WriteIList(Utf8JsonWriter writer, JsonSerializerOptions options, PropertyInfo property, Object obj)
    {
        writer.WriteStartArray(property.Name);
        var l = (IEnumerable) obj.GetPropertyValue(property.Name);
        foreach (var o in l)
        {
            ObjJsonWriter.Write(writer, options, o);
        }

        writer.WriteEndArray();
    }

    [UsedImplicitly]
    public static void WriteInt32(Utf8JsonWriter writer, JsonSerializerOptions options, PropertyInfo property, Object obj)
    {
        writer.WriteNumber(property.Name, (int) obj.GetPropertyValue(property.Name));
    }

    [UsedImplicitly]
    public static void WriteBoolean(Utf8JsonWriter writer, JsonSerializerOptions options, PropertyInfo property, Object obj)
    {
        writer.WriteBoolean(property.Name, (bool) obj.GetPropertyValue(property.Name));
    }

    [UsedImplicitly]
    public static void WriteString(Utf8JsonWriter writer, JsonSerializerOptions options, PropertyInfo property, Object obj)
    {
        writer.WriteString(property.Name, (string) obj.GetPropertyValue(property.Name));
    }

    [UsedImplicitly]
    public static void WriteNullable(Utf8JsonWriter writer, JsonSerializerOptions options, PropertyInfo property, Object obj)
    {
        if (obj.GetPropertyValue(property.Name) == null)
        {
            writer.WriteNull(property.Name);
        }
        else
        {
            var nullableType = property.PropertyType.GenericTypeArguments[0];
            Write(writer, options, property, nullableType, obj);
        }
    }

    [UsedImplicitly]
    public static void WriteColTemplateBase(Utf8JsonWriter writer, JsonSerializerOptions options, PropertyInfo property, Object obj)
    {
        if (obj.GetPropertyValue(property.Name) == null)
        {
            writer.WriteNull(property.Name);
        }
        else
        {
            writer.WritePropertyName(property.Name);
            ObjJsonWriter.Write(writer, options, obj.GetPropertyValue(property.Name));
        }
    }
}