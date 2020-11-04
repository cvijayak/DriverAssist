using DriverAssist.WebAPI.Common;
using DriverAssist.WebAPI.Common.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace DriverAssist.WebAPI.App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        private ILogger<NotificationController> _logger;
        private readonly IHubContext<VehicleNotificationHub> _vehicleNotificationHubContext;
        private INotificationService _notificationService;

        public NotificationController(ILogger<NotificationController> logger, 
            IHubContext<VehicleNotificationHub> vehicleNotificationHubContext, 
            INotificationService notificationService)
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
            async Task SendMessage(string topic)
            {
                await _vehicleNotificationHubContext.Clients.Group(topic).SendCoreAsync("vehicleNotification", new[] { request.Message });
            }

            var result = await _notificationService.NotifyAsync(request, SendMessage, cancellationToken);
            return result.GetActionResult(ActionResultFactory);
        }
    }
}
