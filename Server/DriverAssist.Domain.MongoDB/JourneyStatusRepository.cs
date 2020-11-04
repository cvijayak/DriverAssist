using DriverAssist.Domain.Common;
using DriverAssist.Domain.Common.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DriverAssist.Domain.MongoDB
{
    internal class JourneyStatusRepository : MongoRepositoryBase<JourneyStatus>, IJourneyStatusRepository
    {
        public JourneyStatusRepository(IMongoDbStorage dbStorage)
            : base(dbStorage)
        {
        }

        protected override IMongoCollection<JourneyStatus> Collection => _dbStorage.JourneyStatuses;

        public async Task<JourneyStatus> GetByVehicleIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await Collection.AsQueryable().Where(c => c.DriverId == id).FirstOrDefaultAsync();
        }
    }
}
