using System;
using System.Linq;
using System.Text;

namespace DriverAssist.WebAPI.Common.Filters
{
    internal class ParserContext
    {
        private readonly string _expression;
        private int _pos;

        public ParserContext(string expression)
        {
            _expression = expression;
        }

        public char? GetCurrentChar()
        {
            if (_pos >= _expression.Length)
            {
                return null;
            }

            return _expression[_pos];
        }

        public void MoveFornt()
        {
            _pos++;
        }

        public void MoveBack()
        {
            _pos--;
        }

        public void MoveToEnd()
        {
            _pos = _expression.Length;
        }

        public void MoveToStart()
        {
            _pos = 0;
        }

        public string GetProcessedString()
        {
            int aLength = 0;
            if (_pos >= _expression.Length)
            {
                aLength = _expression.Length;
            }

            return new string(_expression.ToCharArray(), 0, aLength);
        }

        public Tuple<string, string, string> GetToken(char[] startDelimiters, char[] endDelimiters)
        {
            char? ch;
            var currentToken = new StringBuilder();
            var startDelimiterFound = startDelimiters == null || !startDelimiters.Any();
            var startDelimiter = string.Empty;
            var endDelimiter = string.Empty;
            while ((ch = GetCurrentChar()) != null)
            {
                MoveFornt();

                if (!startDelimiterFound)
                {
                    if (startDelimiters.Contains(ch.Value))
                    {
                        startDelimiterFound = true;
                        startDelimiter = ch.ToString();
                    }

                    continue;
                }

                if (endDelimiters.Contains(ch.Value))
                {
                    endDelimiter = ch.ToString();
                    break;
                }

                currentToken.Append(ch);
            }

            return new Tuple<string, string, string>(currentToken.ToString().Trim(' '), startDelimiter, endDelimiter);
        }
    }
}