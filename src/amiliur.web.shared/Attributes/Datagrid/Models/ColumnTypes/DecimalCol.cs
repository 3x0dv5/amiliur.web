using amiliur.web.shared.Attributes.Datagrid.Enums;

namespace amiliur.web.shared.Attributes.Datagrid.Models.ColumnTypes;

public class DecimalCol : GridColBase
{
    public override TypeOfColumn TypeOfColumn => TypeOfColumn.Decimal;

    public virtual int DecimalPlaces { get; set; } = 2;

    public virtual string Format => $"{{0:N{DecimalPlaces}}}"; 

    public DecimalCol(string field, string name) : base(field, name)
    {
    }
}

public class CurrencyCol : DecimalCol
{
    public override TypeOfColumn TypeOfColumn => TypeOfColumn.Currency;

    public override int DecimalPlaces { get; set; } = 3;

    public override string Format => $"{{0:C{DecimalPlaces}}}";

    public CurrencyCol(string field, string name) : base(field, name)
    {
    }
}