using DriverAssist.Domain.Common;
using DriverAssist.Domain.Common.Entities;
using MongoDB.Driver;

namespace DriverAssist.Domain.MongoDB
{
    internal class VehicleRepository : MongoRepositoryBase<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(IMongoDbStorage dbStorage)
            : base(dbStorage)
        {
        }

        protected override IMongoCollection<Vehicle> Collection => _dbStorage.Vehicles;
    }
}
