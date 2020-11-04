namespace DriverAssist.WebAPI.Common.Filters
{
    internal class OperandParserState : ParserStateBase
    {
        protected override void ProcessSingleQuote()
        {
            RaiseProcessEvent(ParserEvents.ProcessOperandInsideQuotes);
        }

        protected override void ProcessCharacter()
        {
            RaiseProcessEvent(ParserEvents.ProcessOperand);
        }

        protected override void ProcessOpenBrace()
        {
            RaiseProcessEvent(ParserEvents.ProcessOperandArray);
        }
    }
}