using System.Text.Json;

namespace amiliur.shared.Json;

public static class SerializableModel
{
    public static JsonSerializerOptions SerializerOptions(bool writeIdented = false)
    {
        var o = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            WriteIndented = writeIdented,
            PropertyNameCaseInsensitive = false,
            PropertyNamingPolicy = null
        };
        o.Converters.Add(new DynamicFormJsonConverterFactory());
        return o;
    }

    public static ISerializableModel? Deserialize(string json)
    {
        return (ISerializableModel?) JsonSerializer.Deserialize(json, GetTypeOfJsonObject(json), SerializerOptions());
    }

    private static string GetTypeNameFromJson(string json)
    {
        var elems = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
        if (elems == null)
            throw new InvalidOperationException("Json is invalid");

        return ((JsonElement) elems.GetValueOrDefault(Constants.ObjectTypePropertyName, "")).GetString() ?? "";
    }

    private static Type GetTypeOfJsonObject(string json)
    {
        var typeName = GetTypeNameFromJson(json);
        return Type.GetType(typeName) ?? throw new InvalidOperationException($"Tipo {typeName} is invalid");
    }
}