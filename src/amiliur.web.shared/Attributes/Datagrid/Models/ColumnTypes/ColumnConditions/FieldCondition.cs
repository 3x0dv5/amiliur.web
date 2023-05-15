using System.Linq.Dynamic.Core;
using amiliur.web.shared.Filtering;
using Serilog;

namespace amiliur.web.shared.Attributes.Datagrid.Models.ColumnTypes.ColumnConditions;

public class DefaultParsinConfig : ParsingConfig
{
    public DefaultParsinConfig()
    {
        DateTimeIsParsedAsUTC = true;
        CustomTypeProvider = new LinqCustomProvider();
    }
}

public class FieldCondition : ICondition
{
    public string FieldName { get; init; }
    public WhereFilterType Operator { get; init; }
    public string Value { get; init; }
    public string ControlName { get; init; }

    public bool Hit(object data)
    {
        var expressionString = ExpressionFilterGenerator.ExpressionString(data, FieldName, Operator, Value);
        return new List<object> {data}.AsQueryable().Any(new DefaultParsinConfig(), expressionString);
    }
}

public class ExpressionCondition : ICondition
{
    public string Expression { get; init; }
    public string ControlName { get; init; }

    public bool Hit(object data)
    {
        try
        {
            var expression = DynamicExpressionParser.ParseLambda(new DefaultParsinConfig(), data.GetType(), typeof(bool), Expression);

            var hit = (bool) expression.Compile().DynamicInvoke(data);
            return hit;
        }
        catch (Exception e)
        {
            Log.Information("--> data: {data}, ex: {ex}", data, e.Message);
            throw;
        }
    }
}