using System.Collections.Generic;

namespace DriverAssist.WebAPI.Common.Filters
{
    public abstract class FilterBase
    {
        protected FilterBase(IReadOnlyCollection<string> supportedFields)
        {
            SupportedFields = supportedFields;
        }

        public DefaultExpressionTokens Tokens { get; set; }
        public string[] AvailableFields { get; set; }

        public IReadOnlyCollection<string> SupportedFields
        {
            get;
        }
    }
}