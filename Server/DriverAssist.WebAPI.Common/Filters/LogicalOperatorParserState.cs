namespace DriverAssist.WebAPI.Common.Filters
{
    internal class LogicalOperatorParserState : ParserStateBase
    {
        protected override void ProcessCharacter()
        {
            RaiseProcessEvent(ParserEvents.ProcessLogicalOperator);
        }
    }
}