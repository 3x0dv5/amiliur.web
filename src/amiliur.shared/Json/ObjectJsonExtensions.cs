using System.Text.Json;
using Serilog;

namespace amiliur.shared.Json;

public static class ObjectJsonExtensions
{
    public static string ToJson(this object obj, JsonSerializerOptions? options = null, bool hideError = false)
    {
        return JsonUtils.ConvertToJson(obj, options, hideError);
    }
}

public static class JsonUtils
{
    public static string ConvertToJson(object obj, JsonSerializerOptions? options = null, bool hideError = false)
    {
        try
        {
            if (options is {WriteIndented: true})
            {
                return JsonHelpers.Beautify(JsonSerializer.Serialize(obj, options));
            }

            return JsonSerializer.Serialize(obj, options);
        }
        catch (Exception e)
        {
            Log.Error(e, "Error converting to json: {}", obj);
            if (!hideError)
                throw;
            return "ERROR";
        }
    }
}