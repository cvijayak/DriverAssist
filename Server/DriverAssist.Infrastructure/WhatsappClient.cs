using DriverAssist.Infrastructure.Common;
using DriverAssist.WebAPI.Configs;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace DriverAssist.Infrastructure
{
    public class WhatsappClient : IWhatsappClient
    {
        private NotificationSettings _notificationSettings;

        public WhatsappClient(IOptions<NotificationSettings> settings)
        {
            _notificationSettings = settings.Value;
        }

        public async Task<bool> SendAsync(WhatsappMessage message, CancellationToken cancellationToken)
        {
            TwilioClient.Init(_notificationSettings.WhatsApp.Username, _notificationSettings.WhatsApp.Password);
            var messageResource = await MessageResource.CreateAsync(
               to: new PhoneNumber($"whatsapp:{_notificationSettings.WhatsApp.SenderContactNumber}"),
               from: new PhoneNumber($"whatsapp:{message.PhoneNumber}"),
               body: message.Text
           );

            return messageResource != null && string.IsNullOrEmpty(messageResource.Sid);
        }
    }
}
