using System;
using System.Collections.Generic;
using System.Linq;

namespace DriverAssist.WebAPI.Common.Filters
{
    public class FilterParser : IDisposable
    {
        private readonly Dictionary<ParserEvents, Action> _parseActionMap;
        private ParserContext _context;
        private ParserStateBase _parserState;
        private Stack<DefaultExpressionTokens> _parseRuleStack;
        private ConditionExpressionToken _currentExpressionToken;
        private int bracesCount;
        private IReadOnlyCollection<string> _supportedFields;
        private HashSet<string> _availableFields;

        public FilterParser()
        {
            _parseActionMap = new Dictionary<ParserEvents, Action>
            {
                {ParserEvents.ProcessField, ProcessField},
                {ParserEvents.ProcessLogicalOperator, ProcessLogicalOperator},
                {ParserEvents.ProcessOperand, ProcessOperand},
                {ParserEvents.ProcessOperandInsideQuotes, ProcessOperandInsideQuotes},
                {ParserEvents.ProcessOperandArray, ProcessOperandArray},
                {ParserEvents.ProcessWhiteSpace, ProcessWhiteSpace},
                {ParserEvents.ProcessBinaryOperator, ProcessBinaryOperator},
                {ParserEvents.ProcessOpenBrace, ProcessOpenBrace},
                {ParserEvents.ProcessCloseBrace, ProcessCloseBrace},
                {ParserEvents.ProcessError, ProcessError }
            };
        }

        ~FilterParser()
        {
            DestroyParserState();
        }

        private void DestroyParserState()
        {
            if (_parserState != null)
            {
                _parserState.ProcessEvent -= HandleProcessEvent;
                _parserState = null;
            }
        }

        private void PushToExpressionStack()
        {
            var token = _parseRuleStack.Peek();
            _currentExpressionToken.PreviousToken = token.Tokens.LastOrDefault();
            token.Tokens.Add(_currentExpressionToken);
            _currentExpressionToken = null;
            SetState(new BinaryOperatorParserState());
        }

        private void SetState(ParserStateBase parserState)
        {
            DestroyParserState();

            _parserState = parserState;
            _parserState.ProcessEvent += HandleProcessEvent;
        }

        private void HandleProcessEvent(ParserEvents s)
        {
            _parseActionMap[s]();
        }

        private void ProcessField()
        {
            if (!_parseRuleStack.Any())
            {
                _parseRuleStack.Push(new DefaultExpressionTokens());
            }

            var fieldName = _context.GetToken(new[] { Delimiters.DOLLAR }, new[] { Delimiters.WHITE_SPACE }).Item1;
            if (_supportedFields != null && _supportedFields.All(s => s != fieldName))
            {
                var errorMessage = $"Unsupported Fields, Value : {fieldName}, SupportedFields : {string.Join(",", _supportedFields)}, ParserState : {_context.GetProcessedString()}";
                SetState(new ErrorParserState { ErrorMessage = errorMessage });
                return;
            }

            _availableFields.Add(fieldName);

            _currentExpressionToken = new ConditionExpressionToken
            {
                FieldName = fieldName
            };

            SetState(new LogicalOperatorParserState());
        }

        private void ProcessLogicalOperator()
        {
            var @operator = _context.GetToken(null, new[] { Delimiters.WHITE_SPACE }).Item1;
            LogicalOperatorType geoLogicalOperatorType;
            if (!Enum.TryParse(@operator, true, out geoLogicalOperatorType))
            {
                var errorMessage = $"Invalid logical operator, Operator : {@operator}, ParserState : {_context.GetProcessedString()}";
                SetState(new ErrorParserState { ErrorMessage = errorMessage });
                return;
            }
            _currentExpressionToken.Operator = geoLogicalOperatorType;
            SetState(new OperandParserState());
        }

        private void ProcessOperand()
        {
            var token = _context.GetToken(null, new[] { Delimiters.WHITE_SPACE, Delimiters.CLOSE_BRACE, Delimiters.OPEN_BRACE });

            var expectedOperand = token.Item1;
            if (!expectedOperand.Equals("null", StringComparison.CurrentCultureIgnoreCase))
            {
                var errorMessage = $"Invalid Field Value, Value : {expectedOperand}, ParserState : {_context.GetProcessedString()}";
                SetState(new ErrorParserState { ErrorMessage = errorMessage });
                return;
            }

            if (new[] { LogicalOperatorType.EQ, LogicalOperatorType.NE }.All(o => o != _currentExpressionToken.Operator))
            {
                var errorMessage = $"Invalid Field Value for Logical Operator, Operator : {_currentExpressionToken.Operator}, " +
                                   $"Value : {expectedOperand}, " +
                                   $"ParserState : {_context.GetProcessedString()}";
                SetState(new ErrorParserState { ErrorMessage = errorMessage });
                return;
            }

            _currentExpressionToken.Operand = null;
            if (!string.IsNullOrEmpty(token.Item3) && token.Item3 != Delimiters.WHITE_SPACE.ToString())
            {
                _context.MoveBack();
            }

            PushToExpressionStack();
        }

        private void ProcessOperandInsideQuotes()
        {
            _currentExpressionToken.Operand = _context.GetToken(new[] { Delimiters.SINGLE_QUOTE }, new[] { Delimiters.SINGLE_QUOTE }).Item1;
            PushToExpressionStack();
        }

        private void ProcessOperandArray()
        {
            if (new[] { LogicalOperatorType.IN, LogicalOperatorType.NI }.All(s => s != _currentExpressionToken.Operator))
            {
                var errorMessage = $"Invalid Field Value for Logical Operator, Operator : {_currentExpressionToken.Operator}, " +
                                   $"ParserState : {_context.GetProcessedString()}";
                SetState(new ErrorParserState { ErrorMessage = errorMessage });
                return;
            }

            var token = _context.GetToken(new[] { Delimiters.OPEN_BRACE }, new[] { Delimiters.CLOSE_BRACE }).Item1;
            var subTokens = token.Split(new[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);
            if (!subTokens.Any())
            {
                var errorMessage = $"Set cannot be Empty. It should contain atleast one Field Value, ParserState : {_context.GetProcessedString()}";
                SetState(new ErrorParserState { ErrorMessage = errorMessage });
                return;
            }

            if (!_parseRuleStack.Any())
            {
                _parseRuleStack.Push(new DefaultExpressionTokens());
            }

            var tExpressionTokens = new DefaultExpressionTokens();
            for (int i = 0; i < subTokens.Length; i++)
            {
                var subToken = subTokens[i];
                var tToken = new ConditionExpressionToken
                {
                    FieldName = _currentExpressionToken.FieldName,
                    Operator = _currentExpressionToken.Operator == LogicalOperatorType.IN ? LogicalOperatorType.EQ : LogicalOperatorType.NE,
                    PreviousToken = tExpressionTokens.Tokens.LastOrDefault()
                };

                if (subToken.StartsWith("'"))
                {
                    tToken.Operand = subToken.Trim('\'');
                }
                else if (subToken.Equals("null", StringComparison.CurrentCultureIgnoreCase))
                {
                    tToken.Operand = null;
                }
                else
                {
                    var errorMessage = $"Invalid Field Value, Value : {subToken}, ParserState : {_context.GetProcessedString()}";
                    SetState(new ErrorParserState { ErrorMessage = errorMessage });
                    return;
                }

                tExpressionTokens.Tokens.Add(tToken);
                if (i < subTokens.Length - 1)
                {
                    tExpressionTokens.Tokens.Add(new BinaryExpressionToken
                    {
                        Operator = _currentExpressionToken.Operator == LogicalOperatorType.IN ? BinaryOperatorType.OR : BinaryOperatorType.AND,
                        PreviousToken = tToken
                    });
                }
            }

            _currentExpressionToken = null;

            var previousToken = _parseRuleStack.Peek();
            tExpressionTokens.PreviousToken = previousToken.Tokens.LastOrDefault();
            previousToken.Tokens.Add(tExpressionTokens);
            SetState(new BinaryOperatorParserState());
        }

        private void ProcessWhiteSpace()
        {
            _context.MoveFornt();
        }

        private void ProcessBinaryOperator()
        {
            var @operator = _context.GetToken(null, new[] { Delimiters.WHITE_SPACE }).Item1;
            BinaryOperatorType geoBinaryOperatorType;
            if (!Enum.TryParse(@operator, true, out geoBinaryOperatorType))
            {
                var errorMessage = $"Invalid binary operator, Operator : {@operator}, ParserState : {_context.GetProcessedString()}";
                SetState(new ErrorParserState { ErrorMessage = errorMessage });
                return;
            }
            var token = _parseRuleStack.Peek();
            var binaryOperatorToken = new BinaryExpressionToken
            {
                Operator = geoBinaryOperatorType,
                PreviousToken = token.Tokens.LastOrDefault()
            };
            token.Tokens.Add(binaryOperatorToken);

            SetState(new FieldParserState());
        }

        private void ProcessOpenBrace()
        {
            if (!_parseRuleStack.Any())
            {
                _parseRuleStack.Push(new DefaultExpressionTokens());
            }

            bracesCount++;

            _parseRuleStack.Push(new DefaultExpressionTokens());
            _context.MoveFornt();
        }

        private void ProcessCloseBrace()
        {
            bracesCount--;
            if (bracesCount < 0)
            {
                var errorMessage = $"Braces are not matching, ParserState : {_context.GetProcessedString()}";
                SetState(new ErrorParserState { ErrorMessage = errorMessage });
                return;
            }

            _context.MoveFornt();
            if (_parseRuleStack.Count <= 1)
            {
                var errorMessage = $"Invalid Expression, ParserState : {_context.GetProcessedString()}";
                SetState(new ErrorParserState { ErrorMessage = errorMessage });
                return;
            }

            var token = _parseRuleStack.Pop();
            var previousToken = _parseRuleStack.Peek();
            token.PreviousToken = previousToken.Tokens.LastOrDefault();
            previousToken.Tokens.Add(token);
            SetState(new BinaryOperatorParserState());
        }

        private void ProcessError()
        {
            _context.MoveToEnd();

            var errorMessage = $"Invalid Expression, ParserState : {_context.GetProcessedString()}";
            SetState(new ErrorParserState { ErrorMessage = errorMessage });
        }

        public FilterBase Process(string expression, Func<FilterBase> createGeoFilter, out string errorMessage)
        {
            var geoFilter = createGeoFilter();
            if (geoFilter == null)
            {
                errorMessage = "Unable to create FilterExpression";
                return null;
            }

            _availableFields = new HashSet<string>();
            _supportedFields = geoFilter.SupportedFields;
            _context = new ParserContext(expression);
            _parseRuleStack = new Stack<DefaultExpressionTokens>();
            SetState(new FieldParserState());
            errorMessage = string.Empty;

            char? ch;
            while ((ch = _context.GetCurrentChar()) != null)
            {
                _parserState.Process(ch.Value);
                var errorParserState = _parserState as ErrorParserState;
                if (errorParserState == null)
                {
                    continue;
                }
                errorMessage = errorParserState.ErrorMessage;
                return null;
            }

            if (!(_parserState is BinaryOperatorParserState))
            {
                errorMessage = $"Invalid Expression, ParserState : {_context.GetProcessedString()}";
                return null;
            }

            if (_parseRuleStack.Any())
            {
                geoFilter.Tokens = _parseRuleStack.Pop();
                geoFilter.AvailableFields = _availableFields.ToArray();
                return geoFilter;
            }

            errorMessage = $"Invalid Expression, ParserState : {_context.GetProcessedString()}";
            return null;
        }

        public void Dispose()
        {
            DestroyParserState();
            GC.SuppressFinalize(this);
        }
    }
}