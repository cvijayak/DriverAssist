using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;

namespace DriverAssist.WebAPI.Common.Results
{
    public class ServiceResult
    {
        public IResponse Response { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public IDictionary<string, JToken> Parameters { get; set; }

        public IActionResult GetActionResult<T>(T controller) where T : ControllerBase
        {
            return StatusCode switch
            {
                HttpStatusCode.Accepted => controller.Accepted(Response),
                HttpStatusCode.Created => controller.Created(string.Empty, Response),
                HttpStatusCode.OK => controller.Ok(Response),
                HttpStatusCode.NotFound => controller.NotFound(Response),
                HttpStatusCode.BadRequest => controller.NotFound(Response),
                _ => controller.Conflict()
            };
        }
    }
}
