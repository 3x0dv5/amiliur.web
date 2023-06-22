using amiliur.web.shared.Attributes.Datagrid.Models;

namespace amiliur.web.shared.Attributes.Datagrid.SettingsReader;

public interface IGridSettingsReader
{
    public DataGridSettings Read(Type tValueObj);
}