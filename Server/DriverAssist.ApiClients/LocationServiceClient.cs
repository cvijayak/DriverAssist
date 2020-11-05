using DriverAssist.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DriverAssist.ApiClients
{
    public class LocationServiceClient 
    {
        public async Task<string> GetPublicIPAddressAsync(CancellationToken cancellationToken)
        {
            var uri = new Uri("http://checkip.dyndns.org/", UriKind.Absolute);
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, uri))
                {
                    using (var response = await client.SendAsync(request, cancellationToken))
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            throw new Exception();
                        }

                        var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                        var tokens = result.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                        return tokens[1].Trim();
                    }
                }
            }
        }

        public async Task<(double Latitude, double Longitude, JObject Response)> GetLocationAsync(string accessKey, string publicIpAddress, CancellationToken cancellationToken)
        {
            var uri = new Uri($"http://api.ipstack.com/{publicIpAddress}?access_key={accessKey}&format=1", UriKind.Relative);
            using (var client = new HttpClient())
            {
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
                            var r = stream.ToJObject();
                            return ((double)r["latitude"], (double)r["longitude"], r);
                        }
                    }
                }
            }
        }
    }
}
