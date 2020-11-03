using System.Collections.Generic;
using System.Net;

namespace DriverAssist.WebAPI.Common.Results
{
    public class ServiceResultFactory : IServiceResultFactory
    {
        private ServiceResult CreateResult(IResponse response, HttpStatusCode httpStatusCode)
        {
            return new ServiceResult
            {
                Response = response,
                StatusCode = httpStatusCode
            };
        }

        private ServiceResult CreateResult(IResponse response, HttpStatusCode httpStatusCode, IDictionary<string, object> parameters)
        {
            return new ServiceResult
            {
                Response = response,
                StatusCode = httpStatusCode,
                Parameters = parameters
            };
        }

        public ServiceResult Created(string locationUrl, IResponse response) =>
            CreateResult(response, HttpStatusCode.Created, new Dictionary<string, object>
            {
                {"LocationUrl", locationUrl }
            });

        public ServiceResult Accepted(string locationUrl, IResponse response) =>
            CreateResult(response, HttpStatusCode.Accepted, new Dictionary<string, object>
            {
                {"LocationUrl", locationUrl }
            });

        public ServiceResult Ok(IResponse response) => CreateResult(response, HttpStatusCode.OK);

        public ServiceResult NotFound(IResponse response) => CreateResult(response, HttpStatusCode.NotFound);

        public ServiceResult BadRequest(IResponse response) => CreateResult(response, HttpStatusCode.BadRequest);
    }
}
