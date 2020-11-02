using DriverAssist.Domain.Common;
using DriverAssist.WebAPI.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;

namespace DriverAssist.Domain.MongoDB.Extensions
{
    public static class ServiceCollectionX
    {
        public static IServiceCollection AddMongoDbProvider(this IServiceCollection services,
            Action<OptionsBuilder<MongoDBSettings>> options)
        {
            options?.Invoke(services.AddOptions<MongoDBSettings>());

            RegisterEnumAsStringConvention();

            return services
                .AddSingleton<IMongoDbStorage, MongoDbStorage>()
                .AddScoped<IDriverRepository, DriverRepository>()
                .AddScoped<IVehicleRepository, VehicleRepository>();
        }

        private static void RegisterEnumAsStringConvention()
        {
            var pack = new ConventionPack { new EnumRepresentationConvention(BsonType.String) };

            ConventionRegistry.Register("EnumStringConvention", pack, _ => true);
        }
    }
}
