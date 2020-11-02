using DriverAssist.Domain.Common;
using DriverAssist.Domain.Common.Entities;
using MongoDB.Driver;

namespace DriverAssist.Domain.MongoDB
{
    internal class DriverRepository : MongoRepositoryBase<Driver>, IDriverRepository
    {
        public DriverRepository(IMongoDbStorage dbStorage)
            : base (dbStorage)
        {
        }

        protected override IMongoCollection<Driver> Collection => _dbStorage.Drivers;
    }
}
