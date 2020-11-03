using DriverAssist.Domain.Common;
using DriverAssist.WebAPI.Common;
using DriverAssist.WebAPI.Common.Requests;
using DriverAssist.WebAPI.Common.Responses;
using DriverAssist.WebAPI.Common.Results;
using DriverAssist.WebAPI.Configs;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DriverAssist.WebAPI.Services
{
    public class NotificationService : INotificationService
    {
        private readonly NotificationSettings _notificationSettings;
        private readonly IDriverRepository _driverRepository;
        private readonly IServiceResultFactory _serviceResultFactory;

        public NotificationService(IOptions<NotificationSettings> settings, 
            IDriverRepository driverRepository, 
            IServiceResultFactory serviceResultFactory)
        {
            _notificationSettings = settings.Value;
            _driverRepository = driverRepository;
            _serviceResultFactory = serviceResultFactory;
        }

        public async Task<ServiceResult> NotifyAsync(PostNotificationRequest request, CancellationToken cancellationToken)
        {
            var driver = await _driverRepository.GetAsync(request.DriverId,cancellationToken);
            if (driver == null)
            {
                return _serviceResultFactory.BadRequest(new BadRequestErrorResponse
                {
                    Message = "Invalid DriverId or DriverId is not found"
                });
            }

            return request.TypeOfNotification switch
            {
                NotificationTypeDto.SMS => await NotifyThroughSms(request.Message, driver.EmergencyContactNumber, cancellationToken),
                NotificationTypeDto.WhatsApp => await NotifyThroughWhatsApp(request.Message, driver.EmergencyContactNumber, cancellationToken),
                _ => _serviceResultFactory.BadRequest(new BadRequestErrorResponse
                {
                    Message = "Unknown Notification Type"
                }),
            };
        }

        private async Task<ServiceResult> NotifyThroughSms(string message, string ownerNumber, CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, _notificationSettings.Sms.Url))
                {
                    using (var response = await client.SendAsync(request, cancellationToken))
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            // Throw exception
                            throw new Exception();
                        }

                        var smsResponse = await response.Content.ReadAsStringAsync();
                        var jObj = JObject.Parse(smsResponse);
                        var status = (int)jObj["Status"];
                        if (status == 0)
                        {
                            return _serviceResultFactory.Accepted("", new NotificationResponse
                            {
                            });
                        } 
                        else
                        {
                            return _serviceResultFactory.BadRequest(new BadRequestErrorResponse
                            {
                                Message = ""
                            });
                        }
                    }
                }
            }
        }

        private async Task<ServiceResult> NotifyThroughWhatsApp(string message, string ownerNumber, CancellationToken cancellationToken)
        {
            // TODO : Write Actual logic

            throw new NotImplementedException();
        }
    }
}
