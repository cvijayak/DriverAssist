using DriverAssist.Common;
using DriverAssist.WebAPI.Common.Requests;
using DriverAssist.WebAPI.Common.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DriverAssist.ApiClients
{
    public class JourneyStatusServiceClient : ClientBase
    {
        public JourneyStatusServiceClient(Uri baseUri)
            : base(baseUri)
        {
        }

        public async Task<JourneyStatusResponse> PostAsync(PostJourneyStatusRequest postJourneyStatusRequest, CancellationToken cancellationToken)
        {
            var uri = new Uri($"JourneyStatuses", UriKind.Relative);
            using (var client = new HttpClient())
            {
                client.BaseAddress = _baseUri;
                using (var request = new HttpRequestMessage(HttpMethod.Post, uri))
                {
                    var settings = new JsonSerializerSettings
                    {
                        Formatting = Formatting.Indented,
                        ContractResolver = new JsonContractResolver(),
                        NullValueHandling = NullValueHandling.Ignore
                    };
                    settings.Converters.Add(new StringEnumConverter());

                    var serializedObject = JsonConvert.SerializeObject(postJourneyStatusRequest, settings);
                    request.Content = new StringContent(serializedObject, Encoding.UTF8, "application/json");

                    using (var response = await client.SendAsync(request, cancellationToken))
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            throw new Exception();
                        }

                        using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                        {
                            return stream.ToObject<JourneyStatusResponse>();
                        }
                    }
                }
            }
        }

        public async Task<JourneyStatusesResponse> ListAsync(CancellationToken cancellationToken)
        {
            var uri = new Uri($"JourneyStatuses", UriKind.Relative);
            using (var client = new HttpClient())
            {
                client.BaseAddress = _baseUri;
                using (var request = new HttpRequestMessage(HttpMethod.Get, uri))
                {
                    using (var response = await client.SendAsync(request, cancellationToken))
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            throw new Exception();
                        }

                        using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                        {
                            return stream.ToObject<JourneyStatusesResponse>();
                        }
                    }
                }
            }
        }
    }
}
