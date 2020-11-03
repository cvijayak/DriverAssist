using DriverAssist.Domain.Common;
using DriverAssist.Domain.Common.Entities;
using DriverAssist.WebAPI.Common;
using DriverAssist.WebAPI.Common.Requests;
using DriverAssist.WebAPI.Common.Responses;
using DriverAssist.WebAPI.Common.Results;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DriverAssist.WebAPI.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IServiceResultFactory _serviceResultFactory;

        public VehicleService(IVehicleRepository vehicleRepository, IServiceResultFactory serviceResultFactory)
        {
            _vehicleRepository = vehicleRepository;
            _serviceResultFactory = serviceResultFactory;
        }

        private FuelTypeDto ConvertTo(FuelType fuelType) => fuelType switch
        {
            FuelType.Disel => FuelTypeDto.Disel,
            FuelType.Petrol => FuelTypeDto.Petrol,
            FuelType.Gas => FuelTypeDto.Gas,
            _ => throw new InvalidCastException(),
        };

        private FuelType ConvertTo(FuelTypeDto fuelType) => fuelType switch
        {
            FuelTypeDto.Disel => FuelType.Disel,
            FuelTypeDto.Petrol => FuelType.Petrol,
            FuelTypeDto.Gas => FuelType.Gas,
            _ => throw new InvalidCastException(),
        };

        private VehicleResponse ConvertTo(Vehicle vehicle) => new VehicleResponse
        {
            Id = vehicle.Id,
            Make = vehicle.Make,
            Model = vehicle.Model,
            RegistrationNumber = vehicle.RegistrationNumber,
            EngineNumber = vehicle.EngineNumber,
            TypeOfFuel = ConvertTo(vehicle.TypeOfFuel)
        };

        public async Task<ServiceResult> PostAsync(PostVehicleRequest request, CancellationToken cancellationToken)
        {
            var vehicle = new Vehicle
            {
                Id = Guid.NewGuid(),
                Make = request.Make,
                Model = request.Model,
                RegistrationNumber = request.RegistrationNumber,
                EngineNumber = request.EngineNumber,
                TypeOfFuel = ConvertTo(request.TypeOfFuel)
            };

            await _vehicleRepository.AddAsync(vehicle, cancellationToken);
            return _serviceResultFactory.Created("", ConvertTo(vehicle));
        }

        public async Task<ServiceResult> PutAsync(Guid id, PutVehicleRequest request, CancellationToken cancellationToken)
        {
            var vehicle = await _vehicleRepository.GetAsync(id, cancellationToken);
            if (vehicle == null)
            {
                return _serviceResultFactory.NotFound(new NotFoundErrorReponse
                {
                    Id = id
                });
            }

            await _vehicleRepository.UpdateAsync(vehicle, cancellationToken);
            return _serviceResultFactory.Ok(ConvertTo(vehicle));
        }

        public async Task<ServiceResult> ListAsync(CancellationToken cancellationToken)
        {
            var vehicles = await _vehicleRepository.GetAsync(cancellationToken);
            return _serviceResultFactory.Ok(new VehiclesResponse
            {
                Items = (from vehicle in vehicles.Items
                         select ConvertTo(vehicle)).ToArray(),
                Total = vehicles.Total
            }); 
        }

        public async Task<ServiceResult> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var vehicle = await _vehicleRepository.GetAsync(id, cancellationToken);
            return _serviceResultFactory.Ok(ConvertTo(vehicle)); 
        }

        public async Task<ServiceResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var driver = await _vehicleRepository.GetAsync(id, cancellationToken);
            if (driver == null)
            {
                return _serviceResultFactory.Ok(new DeleteResponse
                {
                    Id = id,
                    TypeOfDeletion = DeleteTypeDto.NotFound
                }); 
            }

            await _vehicleRepository.DeleteAsync(id, cancellationToken);
            return _serviceResultFactory.Ok(new DeleteResponse
            {
                Id = id,
                TypeOfDeletion = DeleteTypeDto.Deleted
            }); 
        }
    }
}
