namespace DriverAssist.WebAPI.Common.Filters
{
    internal enum ParserEvents
    {
        ProcessField,
        ProcessLogicalOperator,
        ProcessOperand,
        ProcessOperandArray,
        ProcessOperandInsideQuotes,
        ProcessWhiteSpace,
        ProcessBinaryOperator,
        ProcessOpenBrace,
        ProcessCloseBrace,
        ProcessError
    }
}