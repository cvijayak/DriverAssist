using Newtonsoft.Json.Linq;
using System;

namespace DriverAssist.WebAPI.Common.Requests
{
    public class PostNotificationRequest : RequestBase
    {
        public JObject Message { get; set; }
        public Guid DriverId { get; set; }
        public Guid VehicleId { get; set; }
    }
}
