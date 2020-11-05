using DriverAssist.Common;
using System.Collections.Generic;
using System.Linq;

namespace DriverAssist.WebAPI.Common.Filters
{
    public class DriverFilter : FilterBase
    {
        public enum Fields
        {
            [Filter] [SortBy] Id,
            [Filter] [SortBy] FirstName,
            [Filter] [SortBy] LastName,
            [Filter] [SortBy] MiddleName,
            [Filter] [SortBy] ContactNumber1,
            [Filter] [SortBy] ContactNumber2,
            [Filter] [SortBy] EmergencyContactNumber,
            [Filter] [SortBy] Address,
            [Filter] [SortBy] IdentificationNumber,
            [Filter] [SortBy] TypeOfIdentification,
            [Filter] [SortBy] TypeOfEmployment
        }

        private static readonly IReadOnlyCollection<string> FieldNames = EnumX.FilterByAttribute<Fields, FilterAttribute>().Select(f => f.ToString()).ToArray();

        public DriverFilter()
            : base(FieldNames)
        {
        }
    }
}