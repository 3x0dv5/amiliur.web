namespace amiliur.web.shared.Models.Generic;

public enum FileMimeType
{
    Jpeg,
    Png,
    Pdf,
    Xlsx
}

public static class FileMimeTypeExtensions
{
    public static Dictionary<FileMimeType, String> MimeTypes = new Dictionary<FileMimeType, string>()
    {
        {FileMimeType.Jpeg, "image/jpeg"},
        {FileMimeType.Png, "image/png"},
        {FileMimeType.Pdf, "application/pdf"},
        {FileMimeType.Xlsx, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
    };

    public static FileMimeType FromString(string mimeType)
    {
        var d = MimeTypes.Select(m => new {Value = m.Key, Key = m.Value}).ToDictionary(m => m.Key, arg => arg.Value);
        return d.GetValueOrDefault(mimeType, FileMimeType.Jpeg);
    }

    public static Dictionary<string, string> MimeTypesExtensions = new Dictionary<string, string>
    {
        {"image/jpeg", "jpg"}
    };

    public static Dictionary<string, string> ExtensionsMimeTypes = new Dictionary<string, string>
    {
        {"jpg", "image/jpeg"},
        {"png", "image/png"},
        {"pdf", "application/pdf"},
        {"xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
    };

    public static string MimeTypeStr(this FileMimeType t)
    {
        return MimeTypes.GetValueOrDefault(t, null);
    }

    public static string Extension(this FileMimeType t)
    {
        return ExtensionFromMimeType(t.MimeTypeStr());
    }

    public static string ExtensionFromMimeType(string mimeType)
    {
        if (string.IsNullOrEmpty(mimeType))
            return "";
        var parts = mimeType.ToLower().Split("/");
        var ext = "";
        if (parts.Length > 1)
        {
            ext = parts[1];
        }
        return MimeTypesExtensions.GetValueOrDefault(mimeType, ext);
    }

    public static string MimeTypeFromExtension(string extension)
    {
        extension = extension.Replace(".","");
        return ExtensionsMimeTypes.GetValueOrDefault(extension.ToLower(), "image/jpeg");
    }
}