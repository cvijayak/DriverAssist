namespace DriverAssist.WebAPI.Common.Filters
{
    internal class ErrorParserState : ParserStateBase
    {
        public string ErrorMessage { get; set; }

        protected override void ProcessWhiteSpace()
        {
            RaiseProcessEvent(ParserEvents.ProcessError);
        }
    }
}