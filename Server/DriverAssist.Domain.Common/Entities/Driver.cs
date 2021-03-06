﻿using System;

namespace DriverAssist.Domain.Common.Entities
{

    public class Driver : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string ContactNumber1 { get; set; }
        public string ContactNumber2 { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string Address { get; set; }
        public string IdentificationNumber { get; set; }
        public IdentificationNumberType TypeOfIdentification { get; set; }
        public EmploymentType TypeOfEmployment { get; set; }
    }
}
