using DriverAssist.Common;
using DriverAssist.WebAPI.Common.Responses;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DriverAssist.ApiClients
{
    public class VehicleServiceClient : ClientBase
    {
        public VehicleServiceClient(Uri baseUri)
            : base(baseUri)
        {
        }

        public async Task<VehicleResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var uri = new Uri($"vehicles/{id}", UriKind.Relative);
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
                            return stream.ToObject<VehicleResponse>();
                        }
                    }
                }
            }
        }

        public async Task<VehicleResponse> GetByRegistrationNumberAsync(string registrationNumber, CancellationToken cancellationToken)
        {
            var uri = new Uri($"vehicles?filter=($RegistrationNumber eq '{registrationNumber}')", UriKind.Relative);
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
                            var result = stream.ToObject<VehiclesResponse>();
                            return result.Items.FirstOrDefault();
                        }
                    }
                }
            }
        }

        public async Task<VehiclesResponse> ListAsync(Guid id, CancellationToken cancellationToken)
        {
            var uri = new Uri($"vehicles", UriKind.Relative);
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
                            return stream.ToObject<VehiclesResponse>();
                        }
                    }
                }
            }
        }
    }
}
