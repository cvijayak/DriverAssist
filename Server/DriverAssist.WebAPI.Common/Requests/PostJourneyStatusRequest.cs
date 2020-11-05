using Newtonsoft.Json.Linq;
using System;

namespace DriverAssist.WebAPI.Common.Requests
{
    public class PostJourneyStatusRequest : RequestBase
    {
        public Guid DriverId { get; set; }
        public Guid VehicleId { get; set; }
        public double CurrentSpeed { get; set; }
        public SpeedUnitTypeDto TypeOfSpeedUnit { get; set; }
        public double[] Coordinates { get; set; }
        public string[] Hazards { get; set; }
    }
}
