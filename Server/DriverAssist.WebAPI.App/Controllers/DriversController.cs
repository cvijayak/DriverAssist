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
    public class DriversController : ApiControllerBase
    {
        private ILogger<DriversController> _logger;
        private IDriverSevice _driverService;

        public DriversController(ILogger<DriversController> logger, IDriverSevice driverService)
        {
            _logger = logger;
            _driverService = driverService;
        }

        private IActionResultFactory ActionResultFactory
        {
            get
            {
                return new ActionResultFactory(this);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(PostDriverRequest request, CancellationToken cancellationToken)
        {
            var result = await _driverService.PostAsync(request, cancellationToken);
            return result.GetActionResult(ActionResultFactory);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, PutDriverRequest request, CancellationToken cancellationToken)
        {
            var result = await _driverService.PutAsync(id, request, cancellationToken);
            return result.GetActionResult(ActionResultFactory);
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
            return result.GetActionResult(ActionResultFactory);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _driverService.DeleteAsync(id, cancellationToken);
            return result.GetActionResult(ActionResultFactory);
        }

        [HttpGet]
        public async Task<IActionResult> ListAsync([ModelBinder(typeof(FilterModelBinder))] DriverFilter filter, 
            CancellationToken cancellationToken)
        {
            var result = await _driverService.ListAsync(filter, cancellationToken);
            return result.GetActionResult(ActionResultFactory);
        }
    }
}
