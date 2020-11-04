using DriverAssist.WebAPI.Common;
using Microsoft.AspNetCore.Mvc;

namespace DriverAssist.WebAPI.App
{
    internal class ActionResultFactory : IActionResultFactory
    {
        private readonly ControllerBase _controller;

        public ActionResultFactory(ControllerBase controller)
        {
            _controller = controller;
        }

        public IActionResult Accepted(string locationUrl, IResponse response) => 
            _controller.Accepted(locationUrl, response);

        public IActionResult BadRequest(IResponse response) =>
            _controller.BadRequest(response);

        public IActionResult Created(string locationUrl, IResponse response) =>
            _controller.Created(locationUrl, response);

        public IActionResult NotFound(IResponse response) =>
            _controller.NotFound(response);

        public IActionResult Ok(IResponse response) =>
            _controller.Ok(response);
    }
}
