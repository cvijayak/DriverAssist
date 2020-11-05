using System.Collections.Generic;

namespace DriverAssist.WebAPI.Common.Responses
{
    public class JourneyStatusesResponse : IResponse
    {
        public long Total { get; set; }
        public IEnumerable<JourneyStatusResponse> Items { get; set; }
    }
}
