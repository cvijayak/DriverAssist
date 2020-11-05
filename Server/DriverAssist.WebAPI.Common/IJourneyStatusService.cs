using DriverAssist.WebAPI.Common.Filters;
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
        Task<ServiceResult> PostAsync(PostJourneyStatusRequest request, Func<string, JourneyStatusResponse, Task> publishMessage, CancellationToken cancellationToken);
        Task<ServiceResult> ListAsync(JourneyStatusFilter filter, CancellationToken cancellationToken);
    }
}