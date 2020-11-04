using DriverAssist.Common;
using System.Collections.Generic;
using System.Linq;

namespace DriverAssist.WebAPI.Common.Filters
{
    public class VehicleFilter : FilterBase
    {
        public enum Fields
        {
            [Id] [Filter] [SortBy] Id,
            [Filter] [SortBy] Make,
            [Filter] [SortBy] Model,
            [Filter] [SortBy] RegistrationNumber,
            [Filter] [SortBy] EngineNumber,
            [Filter] [SortBy] TypeOfFuel
        }

        private static readonly IReadOnlyCollection<string> FieldNames = EnumX.FilterByAttribute<Fields, FilterAttribute>().Select(f => f.ToString()).ToArray();

        public VehicleFilter()
            : base(FieldNames)
        {
        }
    }
}