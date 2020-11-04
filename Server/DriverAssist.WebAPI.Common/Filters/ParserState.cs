using System;

namespace DriverAssist.WebAPI.Common.Filters
{
    internal abstract class ParserStateBase
    {
        public event Action<ParserEvents> ProcessEvent = delegate { };

        public void Process(char ch)
        {
            switch (ch)
            {
                case Delimiters.OPEN_BRACE:
                    ProcessOpenBrace();
                    break;
                case Delimiters.CLOSE_BRACE:
                    ProcessCloseBrace();
                    break;
                case Delimiters.WHITE_SPACE:
                    ProcessWhiteSpace();
                    break;
                case Delimiters.SINGLE_QUOTE:
                    ProcessSingleQuote();
                    break;
                case Delimiters.DOLLAR:
                    ProcessDollar();
                    break;
                default:
                    ProcessCharacter();
                    break;
            }
        }

        protected void RaiseProcessEvent(ParserEvents e)
        {
            ProcessEvent(e);
        }

        protected virtual void ProcessOpenBrace()
        {
            RaiseProcessEvent(ParserEvents.ProcessError);
        }
        protected virtual void ProcessCloseBrace()
        {
            RaiseProcessEvent(ParserEvents.ProcessError);
        }
        protected virtual void ProcessDollar()
        {
            RaiseProcessEvent(ParserEvents.ProcessError);
        }
        protected virtual void ProcessSingleQuote()
        {
            RaiseProcessEvent(ParserEvents.ProcessError);
        }
        protected virtual void ProcessWhiteSpace()
        {
            RaiseProcessEvent(ParserEvents.ProcessWhiteSpace);
        }
        protected virtual void ProcessCharacter()
        {
            RaiseProcessEvent(ParserEvents.ProcessError);
        }
    }
}
