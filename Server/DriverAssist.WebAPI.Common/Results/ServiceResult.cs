using DriverAssist.WebAPI.Common.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DriverAssist.WebAPI.Common.Results
{
    public class ServiceResult
    {
        public IResponse Response { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public IActionResult GetActionResult()
        {
            return null;
        }
    }
}
