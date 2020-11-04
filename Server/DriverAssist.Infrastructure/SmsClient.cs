using DriverAssist.Infrastructure.Common;
using DriverAssist.WebAPI.Configs;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DriverAssist.Infrastructure
{
    public class SmsClient : ISmsClient
    {
        private NotificationSettings _notificationSettings;

        public SmsClient(NotificationSettings settings)
        {
            _notificationSettings = settings;
        }

        public async Task<bool> SendAsync(SmsMessage message, CancellationToken cancellationToken)
        {
            var uri = $"{_notificationSettings.Sms.Url}?api_key={_notificationSettings.Sms.Key}&api_secret={_notificationSettings.Sms.Secret}&to={message.PhoneNumber}&text={message.Text}";
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, uri))
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
                        return status == 0;
                    }
                }
            }
        }
    }
}
