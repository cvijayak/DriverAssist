using DriverAssist.Domain.Common;
using MongoDB.Driver;

namespace DriverAssist.Domain.MongoDB
{
    internal interface IMongoDbStorage
    {
        IMongoCollection<Driver> Drivers { get; }
        IMongoCollection<Vehicle> Vehicles { get; }
    }
}
