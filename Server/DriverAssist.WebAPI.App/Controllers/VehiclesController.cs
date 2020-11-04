using System;
using System.Threading;
using System.Threading.Tasks;
using DriverAssist.WebAPI.Common;
using DriverAssist.WebAPI.Common.Filters;
using DriverAssist.WebAPI.Common.Requests;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DriverAssist.WebAPI.App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VehiclesController : ApiControllerBase
    {
        private ILogger<VehiclesController> _logger;
        private IVehicleService _vehicleService;

        public VehiclesController(ILogger<VehiclesController> logger, IVehicleService vehicleService)
        {
            _logger = logger;
            _vehicleService = vehicleService;
        }

        private IActionResultFactory ActionResultFactory
        {
            get
            {
                return new ActionResultFactory(this);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]PostVehicleRequest request, CancellationToken cancellationToken)
        {
            var result = await _vehicleService.PostAsync(request, cancellationToken);
            return result.GetActionResult(ActionResultFactory);
        }
    
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, [FromBody] PutVehicleRequest request, CancellationToken cancellationToken)
        {
            var result = await _vehicleService.PutAsync(id, request, cancellationToken);
            return result.GetActionResult(ActionResultFactory);
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
            var result = await _vehicleService.GetAsync(id, cancellationToken);
            return result.GetActionResult(ActionResultFactory);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _vehicleService.DeleteAsync(id, cancellationToken);
            return result.GetActionResult(ActionResultFactory);
        }

        [HttpGet]
        public async Task<IActionResult> ListAsync([ModelBinder(typeof(FilterModelBinder))] VehicleFilter filter, 
            CancellationToken cancellationToken)
        {
            var result = await _vehicleService.ListAsync(filter, cancellationToken);
            return result.GetActionResult(ActionResultFactory);
        }
    }
}
