using System;

namespace DriverAssist.Domain.Common
{
    public class Vehicle : IEntity<Guid>
    {
        public Guid Id { get; set; }
    }
}
