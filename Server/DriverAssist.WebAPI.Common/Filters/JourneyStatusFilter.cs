using DriverAssist.Common;
using System.Collections.Generic;
using System.Linq;

namespace DriverAssist.WebAPI.Common.Filters
{
    public class JourneyStatusFilter : FilterBase
    {
        public enum Fields
        {
            [Filter] [SortBy] Id,
            [Filter] [SortBy] DriverId,
            [Filter] [SortBy] DriverName,
            [Filter] [SortBy] DriverContactNumber,
            [Filter] [SortBy] VehicleId,
            [Filter] [SortBy] RegistrationNumber,
            [Filter] [SortBy] AvgSpeed,
            [Filter] [SortBy] MaxSpeed,
            [Filter] [SortBy] MinSpeed,
            [Filter] [SortBy] TypeOfSpeedUnit
    }

        private static readonly IReadOnlyCollection<string> FieldNames = EnumX.FilterByAttribute<Fields, FilterAttribute>().Select(f => f.ToString()).ToArray();

        public JourneyStatusFilter()
            : base(FieldNames)
        {
        }
    }
}