using System;

namespace DriverAssist.WebAPI.Common.Responses
{
    public abstract class ResponseBase : IResponse
    {
        public Guid Id { get; set; }
    }
}