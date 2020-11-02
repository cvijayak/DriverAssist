using System;
using System.Threading;
using System.Threading.Tasks;
using DriverAssist.WebAPI.Common;
using DriverAssist.WebAPI.Common.Requests;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DriverAssist.WebAPI.App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DriversController : ControllerBase
    {
        private ILogger<DriversController> _logger;
        private IDriverSevice _driverService;

        public DriversController(ILogger<DriversController> logger, IDriverSevice driverService)
        {
            _logger = logger;
            _driverService = driverService;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(PostDriverRequest request, CancellationToken cancellationToken)
        {
            var result = await _driverService.PostAsync(request, cancellationToken);
            return result.GetActionResult();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, PutDriverRequest request, CancellationToken cancellationToken)
        {
            var result = await _driverService.PutAsync(id, request, cancellationToken);
            return result.GetActionResult();
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> PatchAsync(Guid id, JsonPatchDocument request, CancellationToken cancellationToken)
        {
            return null;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _driverService.GetAsync(id, cancellationToken);
            return result.GetActionResult();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _driverService.DeleteAsync(id, cancellationToken);
            return result.GetActionResult();
        }

        [HttpGet]
        public async Task<IActionResult> ListAsync(CancellationToken cancellationToken)
        {
            var result = await _driverService.ListAsync(cancellationToken);
            return result.GetActionResult();
        }
    }
}
