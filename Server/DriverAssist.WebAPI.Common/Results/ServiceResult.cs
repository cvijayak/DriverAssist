using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;

namespace DriverAssist.WebAPI.Common.Results
{
    public class ServiceResult
    {
        internal ServiceResult() { }

        public IDictionary<string, dynamic> Parameters { get; internal set; }
        public HttpStatusCode StatusCode { get; internal set; }
        public IResponse Response { get; internal set; }

        public IActionResult GetActionResult(IActionResultFactory actionResultFactory)
        {
            IActionResult CreateAcceptedActionResult()
            {
                return actionResultFactory.Accepted(Parameters["LocationUrl"], Response);
            }

            IActionResult CreateCreatedActionResult()
            {
                return actionResultFactory.Created(Parameters["LocationUrl"], Response);
            }

            return StatusCode switch
            {
                HttpStatusCode.Accepted => CreateAcceptedActionResult(),
                HttpStatusCode.Created => CreateCreatedActionResult(),
                HttpStatusCode.OK => actionResultFactory.Ok(Response),
                HttpStatusCode.NotFound => actionResultFactory.NotFound(Response),
                HttpStatusCode.BadRequest => actionResultFactory.BadRequest(Response),
                _ => throw new Exception()
            };
        }
    }
}
