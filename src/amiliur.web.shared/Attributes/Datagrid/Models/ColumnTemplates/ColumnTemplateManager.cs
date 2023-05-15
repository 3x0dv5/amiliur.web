namespace amiliur.web.shared.Attributes.Datagrid.Models.ColumnTemplates;

public class ColumnTemplateManager
{
    public class DefaultTemplateNames
    {
        public const string TextWarningSignYellow = "_default.TextWarningSignYellow";
        public const string TextWarningSignOrange = "_default.TextWarningSignOrange";
        public const string TextWarningSignRed = "_default.TextWarningSignRed";
        public const string TextOkSign = "_default.TextOKSign";
    }

    #region singleton

    private static ColumnTemplateManager _instance;

    private ColumnTemplateManager()
    {
    }

    #endregion

    private readonly Dictionary<string, ColTemplateBase> _templates = new()
    {
        {DefaultTemplateNames.TextWarningSignYellow, new HtmlTemplate {HtmlFormatter = "<span class='e-icons warning-sign-yellow' title='{{tooltip}}'>{{value}}<span>"}},
        {DefaultTemplateNames.TextWarningSignOrange, new HtmlTemplate {HtmlFormatter = "<span class='e-icons warning-sign-orange' title='{{tooltip}}'>{{value}}<span>"}},
        {DefaultTemplateNames.TextWarningSignRed, new HtmlTemplate {HtmlFormatter = "<span class='e-icons warning-sign-red' title='{{tooltip}}'>{{value}}<span>"}},
        {DefaultTemplateNames.TextOkSign, new HtmlTemplate {HtmlFormatter = "<span class='e-icons ok-sign-green' title='{{tooltip}}'>{{value}}<span>"}}
    };


    public static ColumnTemplateManager Instance()
    {
        return _instance ??= new ColumnTemplateManager();
    }

    public void AddTemplate(string name, ColTemplateBase template)
    {
        _templates.Add(name, template);
    }

    public ColTemplateBase GetTemplate(string name)
    {
        return _templates[name];
    }
}