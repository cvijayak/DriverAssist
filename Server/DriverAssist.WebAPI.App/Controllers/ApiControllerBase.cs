using Microsoft.AspNetCore.Mvc;
using System;
using DriverAssist.Common;

namespace DriverAssist.WebAPI.App.Controllers
{
    public abstract class ApiControllerBase : ControllerBase
    {
        protected const string RANGE_ITEMS_UNIT = "items";

        protected virtual int? GetMaxPageSize() => 25;
        protected virtual int? GetRangeMaxPageSize() => 1000;

        protected Tuple<int?, int?> GetRange()
        {
            return null;
            //var rangeHeader = Request.Headers.ReadRangeHeader(RANGE_ITEMS_UNIT);
            //var pageSize = rangeHeader == null ? GetMaxPageSize() : Nullable.Compare(rangeHeader.GetPageSize(), GetRangeMaxPageSize()) <= 0 ? rangeHeader.GetPageSize() : GetRangeMaxPageSize();

            //return new Tuple<int?, int?>(rangeHeader?.GetStartIndex(), pageSize);
        }
    }
}
