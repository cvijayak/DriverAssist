using DriverAssist.WebAPI.Common;
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
    public class JourneyStatusController : ControllerBase
    {
        private ILogger<JourneyStatusController> _logger;
        private readonly IHubContext<VehicleNotificationHub> _vehicleNotificationHubContext;
        private IJourneyStatusService _notificationService;

        public JourneyStatusController(ILogger<JourneyStatusController> logger, 
            IHubContext<VehicleNotificationHub> vehicleNotificationHubContext, 
            IJourneyStatusService notificationService)
        {
            _logger = logger;
            _vehicleNotificationHubContext = vehicleNotificationHubContext;
            _notificationService = notificationService;
        }

        private IActionResultFactory ActionResultFactory
        {
            get
            {
                return new ActionResultFactory(this);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(PostNotificationRequest request, CancellationToken cancellationToken)
        {
            async Task SendMessage(string topic, NotificationResponse reponse)
            {
                await _vehicleNotificationHubContext.Clients.Group(topic).SendCoreAsync("journeyStatus", new[] { reponse }, cancellationToken);
            }

            var result = await _notificationService.NotifyAsync(request, SendMessage, cancellationToken);
            return result.GetActionResult(ActionResultFactory);
        }
    }
}
