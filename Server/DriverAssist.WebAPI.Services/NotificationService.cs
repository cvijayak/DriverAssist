using DriverAssist.WebAPI.Common;
using DriverAssist.WebAPI.Common.Requests;
using DriverAssist.WebAPI.Common.Responses;
using DriverAssist.WebAPI.Common.Results;
using DriverAssist.WebAPI.Configs;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DriverAssist.WebAPI.Services
{
    public class NotificationService : INotificationService
    {
        private readonly NotificationSettings _notificationSettings;

        public NotificationService(IOptions<NotificationSettings> settings)
        {
            _notificationSettings = settings.Value;
        }

        public async Task<ServiceResult> NotifyAsync(PostNotificationRequest request, CancellationToken cancellationToken)
        {
            return request.TypeOfNotification switch
            {
                NotificationTypeDto.SMS => await NotifyThroughSms(request.Message, "", cancellationToken),
                NotificationTypeDto.WhatsApp => await NotifyThroughWhatsApp(request.Message, "", cancellationToken),
                _ => new ServiceResult
                {
                    Response = new BadRequestErrorResponse
                    {
                        Message = "Unknown Notification Type"
                    },
                    StatusCode = HttpStatusCode.BadRequest
                },
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
                            return new ServiceResult
                            {
                                Response = new NotificationResponse
                                {
                                },
                                StatusCode = HttpStatusCode.Accepted
                            };
                        } 
                        else
                        {
                            return new ServiceResult
                            {
                                Response = new BadRequestErrorResponse
                                {
                                    Message = ""
                                },
                                StatusCode = HttpStatusCode.BadRequest
                            };
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
