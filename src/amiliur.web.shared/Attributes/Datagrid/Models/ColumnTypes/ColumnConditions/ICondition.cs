namespace amiliur.web.shared.Attributes.Datagrid.Models.ColumnTypes.ColumnConditions;

public interface ICondition
{
    string ControlName { get; init; }
    bool Hit(object data);
}
