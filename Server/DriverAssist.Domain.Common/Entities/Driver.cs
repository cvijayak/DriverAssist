using DriverAssist.Domain.Common.Entities;
using System;

namespace DriverAssist.Domain.Common.Entities
{
    public class Driver : IEntity<Guid>
    {
        public Guid Id { get; set; }
    }
}
