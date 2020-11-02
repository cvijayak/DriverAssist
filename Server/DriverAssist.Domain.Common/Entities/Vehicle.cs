using System;

namespace DriverAssist.Domain.Common.Entities
{

    public class Vehicle : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string RegistrationNumber { get; set; }
        public string EngineNumber { get; set; }
        public FuelType TypeOfFuel { get; set; }
    }
}
