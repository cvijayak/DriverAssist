namespace DriverAssist.WebAPI.Common.Filters
{
    public class BinaryExpressionToken : ExpressionTokenBase
    {
        public BinaryOperatorType Operator { get; set; }
    }
}