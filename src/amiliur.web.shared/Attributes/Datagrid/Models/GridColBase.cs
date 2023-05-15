using amiliur.shared.Json;
using amiliur.web.shared.Attributes.Datagrid.Enums;
using amiliur.web.shared.Attributes.Datagrid.Models.ColumnTemplates;

namespace amiliur.web.shared.Attributes.Datagrid.Models;

public abstract class GridColBase: ISerializableModel
{
    public abstract TypeOfColumn TypeOfColumn { get; }
    public int DisplayOrder { get; set; }
    public bool Visible { get; set; } = true;
    public string Field { get; set; }
    public string ControlName { get; init; }
    public string Tooltip { get; set; }

    private string _headerText;
    private bool _headerTextSet;

    public string HeaderText
    {
        get
        {
            if (string.IsNullOrEmpty(_headerText) && !_headerTextSet)
            {
                return Field;
            }

            return _headerText;
        }
        init
        {
            _headerText = value;
            _headerTextSet = true;
        }
    }

    public virtual TextAlignment? TextAlign { get; set; } = null;

    public virtual string Width { get; set; } = string.Empty;

    public ColTemplateBase? Template { get; set; }
    public GridColBase()
    {
    }

    public GridColBase(string field, string name)
    {
        Field = field;
        ControlName = name;
    }
}
