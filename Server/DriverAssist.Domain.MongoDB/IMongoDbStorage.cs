using DriverAssist.Domain.Common.Entities;
using MongoDB.Driver;

namespace DriverAssist.Domain.MongoDB
{
    internal interface IMongoDbStorage
    {
        IMongoCollection<Driver> Drivers { get; }
        IMongoCollection<Vehicle> Vehicles { get; }
        IMongoCollection<JourneyStatus> JourneyStatuses { get; }
    }
}
