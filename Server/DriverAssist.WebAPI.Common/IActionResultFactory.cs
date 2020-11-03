using Microsoft.AspNetCore.Mvc;

namespace DriverAssist.WebAPI.Common
{
    public interface IActionResultFactory
    {
        IActionResult Accepted(string locationUrl, IResponse response);
        IActionResult Created(string locationUrl, IResponse response);
        IActionResult Ok(IResponse response);
        IActionResult NotFound(IResponse response);
        IActionResult BadRequest(IResponse response);
    }
}