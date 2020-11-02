using DriverAssist.WebAPI.Common.Responses;
using System.Net;

namespace DriverAssist.WebAPI.Common.Results
{
    public class ServiceResult
    {
        public IResponse Response { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
