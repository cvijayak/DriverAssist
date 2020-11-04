using DriverAssist.Domain.Common.Entities;
using DriverAssist.WebAPI.Configs;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DriverAssist.Domain.MongoDB
{
    internal class MongoDbStorage : IMongoDbStorage
    {
        private readonly IMongoDatabase _mongoDatabase;

        public MongoDbStorage(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _mongoDatabase = client.GetDatabase(settings.Value.DatabaseName);

            Drivers = _mongoDatabase.GetCollection<Driver>(nameof(Drivers));
            Vehicles = _mongoDatabase.GetCollection<Vehicle>(nameof(Vehicles));
            JourneyStatuses = _mongoDatabase.GetCollection<JourneyStatus>(nameof(JourneyStatuses));
        }

        public IMongoCollection<Driver> Drivers { get; }
        public IMongoCollection<Vehicle> Vehicles { get; }
        public IMongoCollection<JourneyStatus> JourneyStatuses { get; }
    }
}
