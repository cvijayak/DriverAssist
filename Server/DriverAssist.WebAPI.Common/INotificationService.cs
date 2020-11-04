using DriverAssist.WebAPI.Common.Requests;
using DriverAssist.WebAPI.Common.Results;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DriverAssist.WebAPI.Common
{
    public interface INotificationService
    {
        Task<ServiceResult> NotifyAsync(PostNotificationRequest request, Func<string, Task> publishMessage, CancellationToken cancellationToken);
    }
}