﻿using System;

namespace DriverAssist.WebAPI.Common.Responses
{
    public class NotificationResponse : IResponse
    {
        public Guid DriverId { get; set; }
        public string DriverName { get; set; }
        public string DriverContactNumber { get; set; }
        public Guid VehicleId { get; set; }
        public string RegistrationNumber { get; set; }
        public string CurrentLocation { get; set; }
        public double AvgSpeed { get; set; }
        public double MaxSpeed { get; set; }
        public double MinSpeed { get; set; }
        public SpeedUnitTypeDto TypeOfSpeedUnit { get; set; }
    }
}
