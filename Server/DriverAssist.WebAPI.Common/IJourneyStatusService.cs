using DriverAssist.WebAPI.Common.Requests;
using DriverAssist.WebAPI.Common.Responses;
using DriverAssist.WebAPI.Common.Results;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DriverAssist.WebAPI.Common
{
    public interface IJourneyStatusService
    {
        Task<ServiceResult> NotifyAsync(PostNotificationRequest request, Func<string, NotificationResponse, Task> publishMessage, CancellationToken cancellationToken);
    }
}