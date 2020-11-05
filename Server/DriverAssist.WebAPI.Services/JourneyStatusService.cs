using DriverAssist.Domain.Common;
using DriverAssist.Domain.Common.Entities;
using DriverAssist.Infrastructure.Common;
using DriverAssist.WebAPI.Common;
using DriverAssist.WebAPI.Common.Filters;
using DriverAssist.WebAPI.Common.Requests;
using DriverAssist.WebAPI.Common.Responses;
using DriverAssist.WebAPI.Common.Results;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DriverAssist.WebAPI.Services
{
    public class JourneyStatusService : IJourneyStatusService
    {
        private readonly IDriverRepository _driverRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IJourneyStatusRepository _journeyStatusRepository;
        private readonly ISmsClient _smsClient;
        private readonly IWhatsappClient _whatsappClient;
        private readonly IServiceResultFactory _serviceResultFactory;

        public JourneyStatusService(IDriverRepository driverRepository,
            IVehicleRepository vehicleRepository,
            IJourneyStatusRepository journeyStatusRepository,
            ISmsClient smsClient,
            IWhatsappClient whatsappClient,
            IServiceResultFactory serviceResultFactory)
        {
            _driverRepository = driverRepository;
            _vehicleRepository = vehicleRepository;
            _journeyStatusRepository = journeyStatusRepository;
            _smsClient = smsClient;
            _whatsappClient = whatsappClient;
            _serviceResultFactory = serviceResultFactory;
        }

        private SpeedUnitTypeDto ConvertTo(SpeedUnitType speedUnitType) => speedUnitType switch
        {
            SpeedUnitType.KilometerPerHour => SpeedUnitTypeDto.KilometerPerHour,
            _ => throw new InvalidCastException(),
        };

        private SpeedUnitType ConvertTo(SpeedUnitTypeDto speedUnitType) => speedUnitType switch
        {
            SpeedUnitTypeDto.KilometerPerHour => SpeedUnitType.KilometerPerHour,
            _ => throw new InvalidCastException(),
        };

        private JourneyStatusResponse ConvertTo(JourneyStatus journeyStatus)
        {
            return new JourneyStatusResponse
            {
                DriverId = journeyStatus.DriverId,
                VehicleId = journeyStatus.VehicleId,
                DriverName = journeyStatus.DriverName,
                DriverContactNumber = journeyStatus.DriverContactNumber,
                RegistrationNumber = journeyStatus.RegistrationNumber,
                TypeOfSpeedUnit = ConvertTo(journeyStatus.TypeOfSpeedUnit),
                Coordinates = journeyStatus.Coordinates,
                Hazards = journeyStatus.Hazards,
                AvgSpeed = journeyStatus.AvgSpeed,
                MaxSpeed = journeyStatus.MaxSpeed,
                MinSpeed = journeyStatus.MinSpeed
            };
        }

        public async Task<ServiceResult> PostAsync(PostJourneyStatusRequest request, Func<string, JourneyStatusResponse, Task> sendMessage, CancellationToken cancellationToken)
        {
            var driver = await _driverRepository.GetAsync(request.DriverId, cancellationToken);
            if (driver == null)
            {
                return _serviceResultFactory.BadRequest(new BadRequestErrorResponse
                {
                    Message = "Invalid DriverId or DriverId is not found"
                });
            }

            var vehicle = await _vehicleRepository.GetAsync(request.VehicleId, cancellationToken);
            if (vehicle == null)
            {
                return _serviceResultFactory.BadRequest(new BadRequestErrorResponse
                {
                    Message = "Invalid VehicleId or VehicleId is not found"
                });
            }

            var journeyStatus = await AddJourneyStatus(request, driver, vehicle, cancellationToken);
            var response = ConvertTo(journeyStatus);
            await sendMessage(vehicle.RegistrationNumber, response);

            return _serviceResultFactory.Accepted("", response);
        }

        public async Task<ServiceResult> ListAsync(JourneyStatusFilter filter, CancellationToken cancellationToken)
        {
            var expr = new JourneyStatusFilterExpressionBuilder().Build(filter);

            var journeyStatuses = await _journeyStatusRepository.GetAsync(expr, cancellationToken);
            return _serviceResultFactory.Ok(new JourneyStatusesResponse
            {
                Items = (from journeyStatus in journeyStatuses.Items
                         select ConvertTo(journeyStatus)).ToArray(),
                Total = journeyStatuses.Total
            });
        }

        private async Task<JourneyStatus> AddJourneyStatus(PostJourneyStatusRequest request, Driver driver, Vehicle vehicle, CancellationToken cancellationToken)
        {
            var journeyStatus = await _journeyStatusRepository.GetByVehicleIdAsync(vehicle.Id, cancellationToken);
            var journeyStatusId = journeyStatus == null ? Guid.NewGuid() : journeyStatus.Id;
            if (journeyStatus == null)
            {
                await _journeyStatusRepository.AddAsync(new JourneyStatus
                {
                    Id = journeyStatusId,
                    DriverId = driver.Id,
                    VehicleId = vehicle.Id,
                    DriverName = $"{driver.FirstName} {driver.MiddleName ?? string.Empty} {driver.LastName}",
                    DriverContactNumber = driver.ContactNumber1,
                    RegistrationNumber = vehicle.RegistrationNumber,
                    TypeOfSpeedUnit = ConvertTo(request.TypeOfSpeedUnit),
                    Coordinates = request.Coordinates,
                    Hazards = request.Hazards,
                    AvgSpeed = request.CurrentSpeed,
                    MaxSpeed = request.CurrentSpeed,
                    MinSpeed = request.CurrentSpeed
                }, cancellationToken);
            }
            else
            {
                await _journeyStatusRepository.UpdateAsync(new JourneyStatus
                {
                    DriverId = driver.Id,
                    VehicleId = vehicle.Id,
                    DriverName = $"{driver.FirstName} {driver.MiddleName ?? string.Empty} {driver.LastName}",
                    DriverContactNumber = driver.ContactNumber1,
                    RegistrationNumber = vehicle.RegistrationNumber,
                    TypeOfSpeedUnit = ConvertTo(request.TypeOfSpeedUnit),
                    Coordinates = request.Coordinates,
                    Hazards = request.Hazards,
                    AvgSpeed = (request.CurrentSpeed + journeyStatus.AvgSpeed) / 2.0,
                    MaxSpeed = request.CurrentSpeed > journeyStatus.MaxSpeed ? request.CurrentSpeed : journeyStatus.MaxSpeed,
                    MinSpeed = request.CurrentSpeed < journeyStatus.MinSpeed ? request.CurrentSpeed : journeyStatus.MinSpeed,
                }, cancellationToken);
            }

            return await _journeyStatusRepository.GetAsync(journeyStatusId, cancellationToken);
        }
    }
}
