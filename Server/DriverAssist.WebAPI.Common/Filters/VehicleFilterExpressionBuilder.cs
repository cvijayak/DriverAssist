using DriverAssist.Domain.Common.Entities;

namespace DriverAssist.WebAPI.Common.Filters
{
    public class VehicleFilterExpressionBuilder : FilterExpressionBuilderBase<VehicleFilter, Vehicle>
    {
        private class FieldValueTransformer : FieldValueTransformerBase<VehicleFilter.Fields>, IFieldValueTransformer
        {
        }

        private static readonly IFieldValueTransformer ValueTransformer = new FieldValueTransformer();

        public VehicleFilterExpressionBuilder()
            : base(null, ValueTransformer)
        {
        }
    }
}