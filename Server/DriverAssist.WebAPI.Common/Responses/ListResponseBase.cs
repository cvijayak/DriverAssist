using System.Collections.Generic;

namespace DriverAssist.WebAPI.Common.Responses
{
    public abstract class ListResponseBase<T> : IResponse where T : ResponseBase
    {
        public long Total { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}