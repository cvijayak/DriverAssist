using DriverAssist.Domain.Common.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DriverAssist.Domain.Common
{
    public interface IJourneyStatusRepository : IRepository<JourneyStatus>
    {
        Task<JourneyStatus> GetByVehicleIdAsync(Guid id, CancellationToken cancellationToken);
    }
}