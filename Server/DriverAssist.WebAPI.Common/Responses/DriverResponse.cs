﻿using System.Collections.Generic;

namespace DriverAssist.WebAPI.Common.Responses
{
    public class DriverResponse : ResponseBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string ContactNumber1 { get; set; }
        public string ContactNumber2 { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string Address { get; set; }
        public string IdentificationNumber { get; set; }
        public IdentificationNumberTypeDto TypeOfIdentification { get; set; }
        public EmploymentTypeDto TypeOfEmployment { get; set; }
    }
}
