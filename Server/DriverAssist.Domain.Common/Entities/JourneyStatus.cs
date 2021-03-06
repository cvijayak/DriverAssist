﻿using System;

namespace DriverAssist.Domain.Common.Entities
{
    public class JourneyStatus : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid DriverId { get; set; }
        public string DriverName { get; set; }
        public string DriverContactNumber { get; set; }
        public Guid VehicleId { get; set; }
        public string RegistrationNumber { get; set; }
        public double[] Coordinates { get; set; }
        public string[] Hazards { get; set; }
        public double AvgSpeed { get; set; }
        public double MaxSpeed { get; set; }
        public double MinSpeed { get; set; }
        public SpeedUnitType TypeOfSpeedUnit { get; set; }
        public DateTime? LastSentMessageDateTime { get; set; }
        public double Percentage { get; set; }
    }
}
