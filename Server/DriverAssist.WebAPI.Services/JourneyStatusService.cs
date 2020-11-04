﻿using DriverAssist.Domain.Common;
using DriverAssist.Domain.Common.Entities;
using DriverAssist.WebAPI.Common;
using DriverAssist.WebAPI.Common.Requests;
using DriverAssist.WebAPI.Common.Responses;
using DriverAssist.WebAPI.Common.Results;
using DriverAssist.WebAPI.Configs;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DriverAssist.WebAPI.Services
{
    public class JourneyStatusService : IJourneyStatusService
    {
        private readonly IDriverRepository _driverRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IJourneyStatusRepository _journeyStatusRepository;
        private readonly IServiceResultFactory _serviceResultFactory;

        public JourneyStatusService(IDriverRepository driverRepository,
            IVehicleRepository vehicleRepository,
            IJourneyStatusRepository journeyStatusRepository,
            IServiceResultFactory serviceResultFactory)
        {
            _driverRepository = driverRepository;
            _vehicleRepository = vehicleRepository;
            _journeyStatusRepository = journeyStatusRepository;
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

        public async Task<ServiceResult> NotifyAsync(PostNotificationRequest request, Func<string, NotificationResponse, Task> sendMessage, CancellationToken cancellationToken)
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

            var response = new NotificationResponse
            {
                DriverId = journeyStatus.Id,
                VehicleId = journeyStatus.Id,
                DriverName = journeyStatus.DriverName,
                DriverContactNumber = journeyStatus.DriverContactNumber,
                RegistrationNumber = journeyStatus.RegistrationNumber,
                TypeOfSpeedUnit = ConvertTo(journeyStatus.TypeOfSpeedUnit),
                CurrentLocation = journeyStatus.CurrentLocation,
                AvgSpeed = journeyStatus.AvgSpeed,
                MaxSpeed = journeyStatus.MaxSpeed,
                MinSpeed = journeyStatus.MinSpeed
            };

            await sendMessage(vehicle.RegistrationNumber, response);

            return _serviceResultFactory.Accepted("", response);
        }

        private async Task<JourneyStatus> AddJourneyStatus(PostNotificationRequest request, Driver driver, Vehicle vehicle, CancellationToken cancellationToken)
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
                    DriverName = $"{driver.FirstName} {driver.MiddleName} {driver.LastName}",
                    DriverContactNumber = driver.ContactNumber1,
                    RegistrationNumber = vehicle.RegistrationNumber,
                    TypeOfSpeedUnit = ConvertTo(request.TypeOfSpeedUnit),
                    CurrentLocation = request.CurrentLocation.ToString(),
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
                    DriverName = $"{driver.FirstName} {driver.MiddleName} {driver.LastName}",
                    DriverContactNumber = driver.ContactNumber1,
                    RegistrationNumber = vehicle.RegistrationNumber,
                    TypeOfSpeedUnit = ConvertTo(request.TypeOfSpeedUnit),
                    CurrentLocation = request.CurrentLocation.ToString(),
                    AvgSpeed = (request.CurrentSpeed + journeyStatus.AvgSpeed) / 2.0,
                    MaxSpeed = request.CurrentSpeed > journeyStatus.AvgSpeed ? request.CurrentSpeed : journeyStatus.AvgSpeed,
                    MinSpeed = request.CurrentSpeed < journeyStatus.AvgSpeed ? request.CurrentSpeed : journeyStatus.AvgSpeed,
                }, cancellationToken);
            }

            return await _journeyStatusRepository.GetAsync(journeyStatusId, cancellationToken);
        }

        //private async Task<ServiceResult> NotifyThroughSms(string message, string ownerNumber, CancellationToken cancellationToken)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        using (var request = new HttpRequestMessage(HttpMethod.Get, _notificationSettings.Sms.Url))
        //        {
        //            using (var response = await client.SendAsync(request, cancellationToken))
        //            {
        //                if (!response.IsSuccessStatusCode)
        //                {
        //                    // Throw exception
        //                    throw new Exception();
        //                }

        //                var smsResponse = await response.Content.ReadAsStringAsync();
        //                var jObj = JObject.Parse(smsResponse);
        //                var status = (int)jObj["Status"];
        //                if (status == 0)
        //                {
        //                    return _serviceResultFactory.Accepted("", new NotificationResponse
        //                    {
        //                    });
        //                } 
        //                else
        //                {
        //                    return _serviceResultFactory.BadRequest(new BadRequestErrorResponse
        //                    {
        //                        Message = ""
        //                    });
        //                }
        //            }
        //        }
        //    }
        //}

        //private async Task<ServiceResult> NotifyThroughWhatsApp(string message, string ownerNumber, CancellationToken cancellationToken)
        //{
        //    // TODO : Write Actual logic

        //    throw new NotImplementedException();
        //}
    }
}