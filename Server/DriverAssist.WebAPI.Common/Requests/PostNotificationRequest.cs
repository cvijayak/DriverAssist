using Newtonsoft.Json.Linq;
using System;

namespace DriverAssist.WebAPI.Common.Requests
{
    public class PostNotificationRequest : RequestBase
    {
        public Guid DriverId { get; set; }
        public Guid VehicleId { get; set; }
        public double CurrentSpeed { get; set; }
        public SpeedUnitTypeDto TypeOfSpeedUnit { get; set; }
        public JObject CurrentLocation { get; set; }
    }
}
