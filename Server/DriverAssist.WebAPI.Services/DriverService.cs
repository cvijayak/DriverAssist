using DriverAssist.Domain.Common;
using DriverAssist.Domain.Common.Entities;
using DriverAssist.WebAPI.Common;
using DriverAssist.WebAPI.Common.Filters;
using DriverAssist.WebAPI.Common.Requests;
using DriverAssist.WebAPI.Common.Responses;
using DriverAssist.WebAPI.Common.Results;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DriverAssist.WebAPI.Services
{
    public class DriverService : IDriverSevice
    {
        private readonly IDriverRepository _driverRepository;
        private readonly IServiceResultFactory _serviceResultFactory;

        public DriverService(IDriverRepository driverRepository, IServiceResultFactory serviceResultFactory)
        {
            _driverRepository = driverRepository;
            _serviceResultFactory = serviceResultFactory;
        }

        private EmploymentTypeDto ConvertTo(EmploymentType employmentType) => employmentType switch
        {
            EmploymentType.FullTime => EmploymentTypeDto.FullTime,
            EmploymentType.PartTime => EmploymentTypeDto.PartTime,
            _ => throw new InvalidCastException(),
        };

        private IdentificationNumberType ConvertTo(IdentificationNumberTypeDto identificationNumberType) => identificationNumberType switch
        {
            IdentificationNumberTypeDto.AadharCard => IdentificationNumberType.AadharCard,
            IdentificationNumberTypeDto.DrivingLicence => IdentificationNumberType.DrivingLicence,
            IdentificationNumberTypeDto.VoterId => IdentificationNumberType.VoterId,
            _ => throw new InvalidCastException(),
        };

        private EmploymentType ConvertTo(EmploymentTypeDto employmentType) => employmentType switch
        {
            EmploymentTypeDto.FullTime => EmploymentType.FullTime,
            EmploymentTypeDto.PartTime => EmploymentType.PartTime,
            _ => throw new InvalidCastException(),
        };

        private IdentificationNumberTypeDto ConvertTo(IdentificationNumberType identificationNumberType) => identificationNumberType switch
        {
            IdentificationNumberType.AadharCard => IdentificationNumberTypeDto.AadharCard,
            IdentificationNumberType.DrivingLicence => IdentificationNumberTypeDto.DrivingLicence,
            IdentificationNumberType.VoterId => IdentificationNumberTypeDto.VoterId,
            _ => throw new InvalidCastException(),
        };

        private DriverResponse ConvertTo(Driver driver) => new DriverResponse
        {
            Id = driver.Id,
            Address = driver.Address,
            ContactNumber1 = driver.ContactNumber1,
            ContactNumber2 = driver.ContactNumber2,
            EmergencyContactNumber = driver.EmergencyContactNumber,
            FirstName = driver.FirstName,
            IdentificationNumber = driver.IdentificationNumber,
            LastName = driver.LastName,
            MiddleName = driver.MiddleName,
            TypeOfEmployment = ConvertTo(driver.TypeOfEmployment),
            TypeOfIdentification = ConvertTo(driver.TypeOfIdentification)
        };

        public async Task<ServiceResult> PostAsync(PostDriverRequest request, CancellationToken cancellationToken)
        {
            var driver = new Driver
            {
                Id = Guid.NewGuid(),
                Address = request.Address,
                ContactNumber1 = request.ContactNumber1,
                ContactNumber2 = request.ContactNumber2,
                EmergencyContactNumber = request.EmergencyContactNumber,
                FirstName = request.FirstName,
                IdentificationNumber = request.IdentificationNumber,
                LastName = request.LastName,
                MiddleName = request.MiddleName,
                TypeOfEmployment = ConvertTo(request.TypeOfEmployment),
                TypeOfIdentification = ConvertTo(request.TypeOfIdentification)
            };

            await _driverRepository.AddAsync(driver, cancellationToken);
            return _serviceResultFactory.Created("", ConvertTo(driver));
        }

        public async Task<ServiceResult> PutAsync(Guid id, PutDriverRequest request, CancellationToken cancellationToken)
        {
            var driver = await _driverRepository.GetAsync(id, cancellationToken);
            if (driver == null)
            {
                return _serviceResultFactory.NotFound(new NotFoundErrorReponse
                {
                    Id = id
                });
            }

            driver.Address = request.Address;
            driver.ContactNumber1 = request.ContactNumber1;
            driver.ContactNumber2 = request.ContactNumber2;
            driver.EmergencyContactNumber = request.EmergencyContactNumber;

            await _driverRepository.UpdateAsync(driver, cancellationToken);
            return _serviceResultFactory.Ok(ConvertTo(driver));
        }

        public async Task<ServiceResult> ListAsync(DriverFilter filter, CancellationToken cancellationToken)
        {
            var expr = new DriverFilterExpressionBuilder().Build(filter);

            var drivers = await _driverRepository.GetAsync(expr, cancellationToken);
            return _serviceResultFactory.Ok(new DriversResponse
            {
                Items = (from driver in drivers.Items
                         select ConvertTo(driver)).ToArray(),
                Total = drivers.Total
            });
        }

        public async Task<ServiceResult> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var driver = await _driverRepository.GetAsync(id, cancellationToken);
            return _serviceResultFactory.Ok(ConvertTo(driver));
        }

        public async Task<ServiceResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var driver = await _driverRepository.GetAsync(id, cancellationToken);
            if (driver == null)
            {
                return _serviceResultFactory.Ok(new DeleteResponse
                {
                    Id = id,
                    TypeOfDeletion = DeleteTypeDto.NotFound
                });
            }

            await _driverRepository.DeleteAsync(id, cancellationToken);
            return _serviceResultFactory.Ok(new DeleteResponse
            {
                Id = id,
                TypeOfDeletion = DeleteTypeDto.Deleted
            }); 
        }
    }
}
