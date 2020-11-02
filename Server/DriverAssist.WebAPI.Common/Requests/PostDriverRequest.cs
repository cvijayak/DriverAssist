using System;

namespace DriverAssist.WebAPI.Common.Requests
{
    public class PostDriverRequest : RequestBase
    {
        public Guid VechicleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Contact { get; set; }
    }
}
