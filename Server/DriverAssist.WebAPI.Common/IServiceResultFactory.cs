using DriverAssist.WebAPI.Common.Results;

namespace DriverAssist.WebAPI.Common
{
    public interface IServiceResultFactory
    {
        ServiceResult Accepted(string locationUrl, IResponse response);
        ServiceResult Created(string locationUrl, IResponse response);
        ServiceResult Ok(IResponse response);
        ServiceResult NotFound(IResponse response);
        ServiceResult BadRequest(IResponse response);
    }
}