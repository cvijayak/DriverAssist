using DriverAssist.WebAPI.Common.Filters;
using DriverAssist.WebAPI.Common.Requests;
using DriverAssist.WebAPI.Common.Results;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DriverAssist.WebAPI.Common
{
    public interface IDriverSevice
    {
        Task<ServiceResult> PostAsync(PostDriverRequest request, CancellationToken cancellationToken);
        Task<ServiceResult> PutAsync(Guid id, PutDriverRequest request, CancellationToken cancellationToken);
        Task<ServiceResult> ListAsync(DriverFilter filter, CancellationToken cancellationToken);
        Task<ServiceResult> GetAsync(Guid id, CancellationToken cancellationToken);
        Task<ServiceResult> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}