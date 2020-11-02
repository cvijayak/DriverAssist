namespace DriverAssist.WebAPI.Common.Requests
{
    public class PutDriverRequest : RequestBase
    {
        public string ContactNumber1 { get; set; }
        public string ContactNumber2 { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string Address { get; set; }
    }
}
