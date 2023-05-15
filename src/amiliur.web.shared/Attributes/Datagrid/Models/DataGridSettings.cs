using amiliur.shared.Json;

namespace amiliur.web.shared.Attributes.Datagrid.Models;

public class DataGridSettings : ISerializableModel
{
    private int[] _pageSizes = {20, 40, 100};

    public int[] PageSizes
    {
        get => _pageSizes;
        set
        {
            _pageSizes = value;
            PageSize = value.Min();
        }
    }

    /// <summary>
    /// if true creates the missing properties using default settings, if false only creates the columns that have the attribute defined.
    /// </summary>
    public bool UseDefaultColumnTypesFromProperties { get; set; }

    public int PageSize { get; set; } = 20;
    public bool EnablePersistence { get; set; }
    public bool AllowPaging { get; set; } = true;
    public bool AllowSorting { get; set; } = true;
    public bool AllowMultiSorting { get; set; } = true;
    public bool AllowGrouping { get; set; }
    public bool AllowSelection { get; set; }
    public bool AllowFiltering { get; set; }


    /// <summary>
    /// The user can export if the option is shown
    /// </summary>
    public bool AllowExcelExport { get; set; } = true;

    /// <summary>
    /// The user can export if the option is shown
    /// </summary>
    public bool AllowPdfExport { get; set; } = true;

    /// <summary>
    /// Show the option to export PDF
    /// </summary>
    public bool ShowExcelExport { get; set; } = true;

    /// <summary>
    /// Show the option to export PDF
    /// </summary>
    public bool ShowPdfExport { get; set; } = true;

 
    public string Width { get; set; } = "auto";
    public IList<GridColBase> Columns { get; set; } = new List<GridColBase>();

    public void SortColumns()
    {
        var idx = 1;
        foreach (var col in Columns)
        {
            if (col.DisplayOrder > 0)
                continue;

            while (true)
            {
                if (Columns.All(m => m.DisplayOrder != idx))
                {
                    col.DisplayOrder = idx;
                    break;
                }

                idx++;
            }
        }

        Columns = Columns.OrderBy(m => m.DisplayOrder).ToList();
    }
}