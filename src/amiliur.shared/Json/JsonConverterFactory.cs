using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using amiliur.shared.Extensions;
using amiliur.shared.Reflection;
using Serilog;

namespace amiliur.shared.Json;

public class DynamicFormJsonConverterFactory : JsonConverterFactory
{
    private static bool IsSerializableModel(Type typeToConvert)
    {
        return typeToConvert
            .GetInterfaces()
            .Contains(typeof(ISerializableModel));
    }

    private string TypeConverterFullname(Type typeToConvert)
    {
        return $"{GetType().Namespace}.{typeToConvert.Name}JsonConverter, {GetType().Assembly.FullName}";
    }

    private Type? ConverterTypeOfBaseClass(Type type)
    {
        if (type.BaseType != null || type.BaseType == typeof(object))
            return Type.GetType(TypeConverterFullname(type.BaseType));

        Log.Warning($"Type {type.FullName} does not have a base class");
        return null;
    }

    private Type? ConverterType(Type typeToConvert)
    {
        return Type.GetType(TypeConverterFullname(typeToConvert));
    }

    public override bool CanConvert(Type typeToConvert)
    {
        return IsSerializableModel(typeToConvert);
    }


    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return new SerializableModelJsonConverter(options);
    }
}

public class SerializableModelJsonConverter : JsonConverter<ISerializableModel>
{
    private readonly JsonSerializerOptions _options;

    public SerializableModelJsonConverter(JsonSerializerOptions options)
    {
        _options = options;
    }

    public override ISerializableModel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();
        return ReadObject(ref reader, typeToConvert);
    }

    private Type? ReadObjectType(Utf8JsonReader reader)
    {
        while (reader.Read())
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString();
                if (propertyName == "__objectType__")
                    if (reader.Read())
                    {
                        var typeName = reader.GetString();
                        return string.IsNullOrEmpty(typeName)
                            ? null
                            : Type.GetType(typeName);
                    }
            }

        return null;
    }

    private ISerializableModel? ReadObject(ref Utf8JsonReader reader, Type type)
    {
        ISerializableModel? obj = null;
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                return obj;

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString();
                if (propertyName == "__objectType__")
                {
                    reader.Read();
                    var objectType = reader.GetString();


                    if (objectType != null && (TypeIsNotImportant(type) || TypeIsTheRequestedType(type, objectType)))
                        return CreateObject(ref reader, objectType);

                    throw new JsonException($"Type: {type?.FullName} is not of the type in the json {objectType}");
                }

                throw new JsonException();
            }
        }

        return null;
    }

    private static bool TypeIsTheRequestedType(Type type, string objectType)
    {
        if (TypeName(type) == objectType)
            return true;
        if (type.IsInterface)
            if (Type.GetType(objectType)!.GetInterfaces().Contains(type))
                return true;

        return false;
    }

    private static bool TypeIsNotImportant(Type? type)
    {
        return type == null;
    }

    private static string TypeName(Type typeToConvert)
    {
        var typeName = $"{typeToConvert.FullName}, {typeToConvert.Assembly.GetName().Name}";

        if (typeName.Trim() == ",")
            throw new Exception("TYPENAME BOOM!");

        return typeName;
    }

    private ISerializableModel? CreateObject(ref Utf8JsonReader reader, string typeName)
    {
        var objType = Type.GetType(typeName);
        if (objType == null)
            throw new Exception($"Typename: {typeName} does not look right");

        var obj = (ISerializableModel) Activator.CreateInstance(objType);

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject) return obj;

            var propertyName = "";
            object value = null;
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                propertyName = reader.GetString();
                reader.Read();

                var tk = reader.TokenType;
                switch (tk)
                {
                    case JsonTokenType.None:
                        throw new NotSupportedException($"JsonTokenType.None: {propertyName}");
                        break;
                    case JsonTokenType.StartObject:
                        value = ReadObject(ref reader, null);
                        break;
                    case JsonTokenType.EndObject:
                        throw new NotSupportedException($"JsonTokenType.EndObject: {propertyName}");
                        break;
                    case JsonTokenType.StartArray:
                        value = ReadArray(ref reader);
                        break;
                    case JsonTokenType.EndArray:
                        throw new NotSupportedException($"JsonTokenType.EndArray: {propertyName}");
                    case JsonTokenType.PropertyName:
                        throw new NotSupportedException($"JsonTokenType.PropertyName: {propertyName}");
                    case JsonTokenType.String:
                        value = reader.GetString();
                        if (propertyName == "GenericType")
                            value = Type.GetType((string) value ?? string.Empty);
                        else
                            value = ReadProperty(obj, propertyName, value?.ToString());

                        break;
                    case JsonTokenType.Number:
                        value = ReadNumberProperty(obj, propertyName, ref reader);
                        break;
                    case JsonTokenType.True:
                        value = reader.GetBoolean();
                        break;
                    case JsonTokenType.False:
                        value = reader.GetBoolean();
                        break;
                    case JsonTokenType.Null:
                        value = null;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (!PropertyIsMarkedAsJsonIgnore(obj, propertyName) && obj.GetProperty(propertyName).CanWrite)
                    try
                    {
                        obj.SetPropertyValue(propertyName, value);
                    }
                    catch (Exception e)
                    {
                        Log.Error("SerializableModelJsonConverter:CreateObject: property name: {propertyName}, value: {value}. Exception: {e}", propertyName, value, e);
                        throw;
                    }
            }
        }

        return obj;
    }

    private object ReadNumberProperty(ISerializableModel model, string p, ref Utf8JsonReader reader)
    {
        var pi = model.GetProperty(p);
        switch (pi.PropertyType.GetNonNullableType().Name)
        {
            case "Int16":
                return reader.GetInt16();
            case "Int32":
                return reader.GetInt32();
            case "Int64":
                return reader.GetInt64();
            case "UInt16":
                return reader.GetUInt16();
            case "UInt32":
                return reader.GetUInt32();
            case "UInt64":
                return reader.GetUInt64();
            case "Byte":
                return reader.GetByte();
            case "SByte":
                return reader.GetSByte();
            case "Float":
                return reader.GetDouble();
            case "Double":
                return reader.GetDouble();
            case "Decimal":
                return reader.GetDecimal();
            default:
                throw new NotSupportedException($"ReadNumberProperty:Type {pi.PropertyType.GetNonNullableType().Name} not supported. Property: {p}");
        }
    }

    private object ReadProperty(ISerializableModel serializableModel, string propertyName, string value)
    {
        if (value == null)
            return null;

        var pi = serializableModel.GetProperty(propertyName);

        if (pi.PropertyType.GetNonNullableType() == typeof(string))
            return value;

        if (pi.PropertyType.GetNonNullableType() == typeof(DateOnly)) return DateOnly.ParseExact(value, "yyyy-MM-dd");

        return value;
    }

    private static bool PropertyIsMarkedAsJsonIgnore(ISerializableModel? obj, string propertyName)
    {
        return obj?.GetProperty(propertyName)?.GetCustomAttribute(typeof(JsonIgnoreAttribute)) != null;
    }

    private IEnumerable<object> ReadArray(ref Utf8JsonReader reader)
    {
        var array = new List<object>();
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray) return array;

            if (reader.TokenType == JsonTokenType.StartObject)
            {
                var obj = ReadObject(ref reader, null);
                array.Add(obj);
            }
        }

        return new List<object>();
    }

    private static void WriteObjectToJson(object dataObject, Utf8JsonWriter writer, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteString("__objectType__", TypeName(dataObject.GetType()));

        foreach (var property in dataObject.GetProperties())
            if (!PropertyIsMarkedAsJsonIgnore(dataObject as ISerializableModel, property.Name))
                writer.WriteProperty(property, dataObject, options);

        writer.WriteEndObject();
    }

    public override void Write(Utf8JsonWriter writer, ISerializableModel value, JsonSerializerOptions options)
    {
        WriteObjectToJson(value, writer, options);
    }
}