using System;

namespace DriverAssist.WebAPI.Common.Responses
{
    public class VehicleResponse : ResponseBase
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public string RegistrationNumber { get; set; }
        public string EngineNumber { get; set; }
        public FuelTypeDto TypeOfFuel { get; set; }
    }
}
