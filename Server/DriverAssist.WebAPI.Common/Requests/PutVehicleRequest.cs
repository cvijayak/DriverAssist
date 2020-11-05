namespace DriverAssist.WebAPI.Common.Requests
{
    public class PutVehicleRequest : RequestBase
    {
        public FuelTypeDto TypeOfFuel { get; set; }
    }
}
