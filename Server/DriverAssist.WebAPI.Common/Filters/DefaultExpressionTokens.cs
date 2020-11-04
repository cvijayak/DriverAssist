using System.Collections.Generic;

namespace DriverAssist.WebAPI.Common.Filters
{
    public class DefaultExpressionTokens : ExpressionTokenBase
    {
        public DefaultExpressionTokens()
        {
            Tokens = new List<ExpressionTokenBase>();
        }

        public List<ExpressionTokenBase> Tokens { get; }
    }
}