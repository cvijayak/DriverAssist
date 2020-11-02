using System;
using System.Threading;
using System.Threading.Tasks;
using DriverAssist.Domain.Common;
using DriverAssist.WebAPI.Common;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DriverAssist.WebAPI.App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VehiclesController : ControllerBase
    {
        private ILogger<Driver> _logger;
        private IVehicleRepository _vehicleRepository;

        public VehiclesController(ILogger<Driver> logger, IVehicleRepository vehicleRepository)
        {
            _logger = logger;
            _vehicleRepository = vehicleRepository;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]PostVehicleRequest request, CancellationToken cancellationToken)
        {
            return null;
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, [FromBody]PutVehicleRequest request, CancellationToken cancellationToken)
        {
            return null;
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> PatchAsync(Guid id, [FromBody]JsonPatchDocument request, CancellationToken cancellationToken)
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
            _logger.LogDebug("test Vehicle");
            return null;
        }
    }
}
