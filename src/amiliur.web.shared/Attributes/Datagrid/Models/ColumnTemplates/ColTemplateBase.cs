using amiliur.shared.Json;

namespace amiliur.web.shared.Attributes.Datagrid.Models.ColumnTemplates;

public abstract class ColTemplateBase : ISerializableModel
{
    public abstract string ToHtml(string value, string tooltip);
    public abstract string ToHtml(object value, string tooltip);
}

public class HtmlTemplate : ColTemplateBase
{
    public string HtmlFormatter { get; set; }

    public override string ToHtml(string value, string tooltip)
    {
        return
            HtmlFormatter
                .Replace("{{value}}", value)
                .Replace("{{tooltip}}", tooltip)
        ;
    }

    public override string ToHtml(object value, string tooltip)
    {
        return ToHtml(value.ToString(), tooltip);
    }
}