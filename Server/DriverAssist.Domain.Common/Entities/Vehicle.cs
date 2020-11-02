using System;

namespace DriverAssist.Domain.Common.Entities
{
    public class Vehicle : IEntity<Guid>
    {
        public Guid Id { get; set; }
    }
}
