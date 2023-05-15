using amiliur.web.shared.Attributes.Datagrid.Attributes.GridColAttributes;

namespace amiliur.web.shared.Models.Generic;

public class BaseSearchResultModel : ISearchResultModel
{
    [TextGridCol(Visible = false, HeaderText = "PK")]
    public string Id { get; set; }
}