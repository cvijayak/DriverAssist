namespace DriverAssist.WebAPI.Common.Filters
{
    internal class FieldParserState : ParserStateBase
    {
        protected override void ProcessOpenBrace()
        {
            RaiseProcessEvent(ParserEvents.ProcessOpenBrace);
        }

        protected override void ProcessDollar()
        {
            RaiseProcessEvent(ParserEvents.ProcessField);
        }
    }
}