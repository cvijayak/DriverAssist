using System;

namespace DriverAssist.Domain.Common
{
    public class Driver : IEntity<Guid>
    {
        public Guid Id { get; set; }
    }
}
