namespace amiliur.web.shared.Attributes.Datagrid.Attributes;


[AttributeUsage(AttributeTargets.Class)]
public class DataGridSettingsAttribute : Attribute
{
    private int[] _pageSizes = {20, 40, 100};

    /// <summary>
    /// if true creates the missing properties using default settings, if false only creates the columns that have the attribute defined.
    /// </summary>
    public bool UseDefaultColumnTypesFromProperties { get; set; }

    public int[] PageSizes
    {
        get => _pageSizes;
        set
        {
            _pageSizes = value;
            PageSize = value.Min();
        }
    }

    public int PageSize { get; set; } = 20;

    public bool AllowPaging { get; set; } = true;
    public bool AllowSorting { get; set; } = true;
    public bool AllowMultiSorting { get; set; } = true;
    public bool AllowGrouping { get; set; }
    public bool AllowSelection { get; set; }

    public string Width { get; set; } = "auto";
}