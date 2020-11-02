using System;
using System.Threading;
using System.Threading.Tasks;
using DriverAssist.Domain.Common.Entities;
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
            return null;
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, PutDriverRequest request, CancellationToken cancellationToken)
        {
            return null;
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
            return null;
        }

        [HttpGet]
        public async Task<IActionResult> ListAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("test Driver");
            return null;
        }
    }
}
