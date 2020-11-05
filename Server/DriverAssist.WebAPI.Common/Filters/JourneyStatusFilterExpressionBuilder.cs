using DriverAssist.Domain.Common.Entities;

namespace DriverAssist.WebAPI.Common.Filters
{
    public class JourneyStatusFilterExpressionBuilder : FilterExpressionBuilderBase<JourneyStatusFilter, JourneyStatus>
    {
        private class FieldValueTransformer : FieldValueTransformerBase<JourneyStatusFilter.Fields>, IFieldValueTransformer
        {
        }

        private static readonly IFieldValueTransformer ValueTransformer = new FieldValueTransformer();

        public JourneyStatusFilterExpressionBuilder()
            : base(null, ValueTransformer)
        {
        }
    }
}