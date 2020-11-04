namespace DriverAssist.WebAPI.Common.Filters
{
    internal class BinaryOperatorParserState : ParserStateBase
    {
        protected override void ProcessCloseBrace()
        {
            RaiseProcessEvent(ParserEvents.ProcessCloseBrace);
        }

        protected override void ProcessCharacter()
        {
            RaiseProcessEvent(ParserEvents.ProcessBinaryOperator);
        }
    }
}