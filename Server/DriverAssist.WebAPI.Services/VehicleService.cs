using DriverAssist.Domain.Common;
using DriverAssist.WebAPI.Common;

namespace DriverAssist.WebAPI.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }
    }
}
