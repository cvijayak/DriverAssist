using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;

namespace DriverAssist.WebAPI.App
{
    public class VehicleNotificationHub : Hub
    {
        public async Task Subscribe(string topic, CancellationToken cancellationToken)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, topic, cancellationToken);
        }
        public async Task Unsubscribe(string topic, CancellationToken cancellationToken)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, topic, cancellationToken);
        }
    }
}
