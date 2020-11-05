using DriverAssist.Common;
using DriverAssist.WebAPI.Common.Responses;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DriverAssist.ApiClients
{
    public class DriverServiceClient : ClientBase
    {
        public DriverServiceClient(Uri baseUri) 
            : base(baseUri)
        {
        }

        public async Task<DriverResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var uri = new Uri( $"drivers/{id}", UriKind.Relative);
            using (var client = new HttpClient())
            {
                client.BaseAddress = _baseUri;
                using (var request = new HttpRequestMessage(HttpMethod.Get, uri))
                {
                    using (var response = await client.SendAsync(request, cancellationToken).ConfigureAwait(false))
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            throw new Exception();
                        }

                        using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                        {
                            return stream.ToObject<DriverResponse>();
                        }
                    }
                }
            }
        }

        public async Task<DriversResponse> ListAsync(Guid id, CancellationToken cancellationToken)
        {
            var uri = new Uri($"drivers", UriKind.Relative);
            using (var client = new HttpClient())
            {
                client.BaseAddress = _baseUri;
                using (var request = new HttpRequestMessage(HttpMethod.Get, uri))
                {
                    using (var response = await client.SendAsync(request, cancellationToken).ConfigureAwait(false))
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            throw new Exception();
                        }

                        using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                        {
                            return stream.ToObject<DriversResponse>();
                        }
                    }
                }
            }
        }
    }
}
