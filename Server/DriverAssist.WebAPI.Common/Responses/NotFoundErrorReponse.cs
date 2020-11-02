using System;

namespace DriverAssist.WebAPI.Common.Responses
{
    public class NotFoundErrorReponse : ErrorResponseBase
    {
        public Guid Id { get; set; }
    }
}
