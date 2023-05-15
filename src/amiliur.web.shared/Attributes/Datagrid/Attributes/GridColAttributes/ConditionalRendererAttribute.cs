using amiliur.web.shared.Attributes.Datagrid.Models.ColumnTypes.ColumnConditions;
using amiliur.web.shared.Filtering;

namespace amiliur.web.shared.Attributes.Datagrid.Attributes.GridColAttributes;

public abstract class ConditionalBaseRendererAttribute: Attribute
{
    public abstract ICondition ToCondition();
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class ConditionalRendererAttribute: ConditionalBaseRendererAttribute
{
    public string CondRelatedField { get; set; }
    public string CondRelatedFieldValue { get; set; }
    public WhereFilterType CondOperator { get; set; } = WhereFilterType.Equal;
    
    public string RendererName { get; set; }

    public override ICondition ToCondition()
    {
        return new FieldCondition
        {
            FieldName = CondRelatedField,
            Operator = CondOperator,
            Value = CondRelatedFieldValue,
            ControlName = RendererName,
        };
    }
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class ConditionalExprAttribute : ConditionalBaseRendererAttribute 
{
    public string RendererName { get; set; }
    public string Exp { get; set; }

    public override ICondition ToCondition()
    {
        return new ExpressionCondition
        {
            Expression = Exp,
            ControlName = RendererName
        };
    }
}

