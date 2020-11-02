using DriverAssist.WebAPI.Common.Requests;
using DriverAssist.WebAPI.Common.Results;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DriverAssist.WebAPI.Common
{
    public interface IVehicleService
    {
        Task<ServiceResult> PostAsync(PostVehicleRequest request, CancellationToken cancellationToken);
        Task<ServiceResult> PutAsync(Guid id, PutVehicleRequest request, CancellationToken cancellationToken);
        Task<ServiceResult> ListAsync(CancellationToken cancellationToken);
        Task<ServiceResult> GetAsync(Guid id, CancellationToken cancellationToken);
        Task<ServiceResult> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}