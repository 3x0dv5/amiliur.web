using System.Text;
using System.Text.Json;

namespace amiliur.shared.Json;

public class JsonHelpers
{
    public static string Beautify(string json)
    {
        using var doc = JsonDocument.Parse(json);
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions {Indented = true});
        doc.WriteTo(writer);
        writer.Flush();
        return Encoding.UTF8.GetString(stream.ToArray());
    }
}