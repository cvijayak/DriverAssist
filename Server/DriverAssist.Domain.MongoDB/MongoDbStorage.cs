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

            Drivers = _mongoDatabase.GetCollection<Driver>(nameof(Driver));
            Vehicles = _mongoDatabase.GetCollection<Vehicle>(nameof(Vehicle));
        }

        public IMongoCollection<Driver> Drivers { get; }

        public IMongoCollection<Vehicle> Vehicles { get; }
    }
}
