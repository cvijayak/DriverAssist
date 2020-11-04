namespace DriverAssist.WebAPI.Common.Filters
{
    public class ConditionExpressionToken : ExpressionTokenBase
    {
        public string FieldName { get; set; }
        public LogicalOperatorType Operator { get; set; }
        public string Operand { get; set; }
    }
}