using DriverAssist.Domain.Common.Entities;

namespace DriverAssist.WebAPI.Common.Filters
{
    public class DriverFilterExpressionBuilder : FilterExpressionBuilderBase<DriverFilter, Driver>
    {
        private class FieldValueTransformer : FieldValueTransformerBase<DriverFilter.Fields>, IFieldValueTransformer
        {
        }

        private static readonly IFieldValueTransformer ValueTransformer = new FieldValueTransformer();

        public DriverFilterExpressionBuilder()
            : base(null, ValueTransformer)
        {
        }
    }
}