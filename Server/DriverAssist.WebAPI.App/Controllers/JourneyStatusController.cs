using DriverAssist.WebAPI.Common;
using DriverAssist.WebAPI.Common.Filters;
using DriverAssist.WebAPI.Common.Requests;
using DriverAssist.WebAPI.Common.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace DriverAssist.WebAPI.App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JourneyStatusController : ApiControllerBase
    {
        private ILogger<JourneyStatusController> _logger;
        private readonly IHubContext<VehicleNotificationHub> _vehicleNotificationHubContext;
        private IJourneyStatusService _journeyStatusService;

        public JourneyStatusController(ILogger<JourneyStatusController> logger, 
            IHubContext<VehicleNotificationHub> vehicleNotificationHubContext, 
            IJourneyStatusService journeyStatusService)
        {
            _logger = logger;
            _vehicleNotificationHubContext = vehicleNotificationHubContext;
            _journeyStatusService = journeyStatusService;
        }

        private IActionResultFactory ActionResultFactory
        {
            get
            {
                return new ActionResultFactory(this);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]PostJourneyStatusRequest request, CancellationToken cancellationToken)
        {
            async Task SendMessage(string topic, JourneyStatusResponse reponse)
            {
                await _vehicleNotificationHubContext.Clients.Group(topic).SendCoreAsync("journeyStatus", new[] { reponse }, cancellationToken);
            }

            var result = await _journeyStatusService.PostAsync(request, SendMessage, cancellationToken);
            return result.GetActionResult(ActionResultFactory);
        }

        [HttpGet]
        public async Task<IActionResult> ListAsync([ModelBinder(typeof(FilterModelBinder))]JourneyStatusFilter filter, 
            CancellationToken cancellationToken)
        {
            var result = await _journeyStatusService.ListAsync(filter, cancellationToken);
            return result.GetActionResult(ActionResultFactory);

        }
    }
}
