using DriverAssist.Domain.Common;
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
