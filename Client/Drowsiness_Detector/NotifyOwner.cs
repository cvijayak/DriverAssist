using DriverAssist.ApiClients;
using System.Threading;

namespace DrowsyDoc
{
    class NotifyOwner
    { 
        public static void  SendAlert(){
            var locationClient = new LocationServiceClient();
            var ipAddress = locationClient.GetPublicIPAddressAsync(default(CancellationToken)).Result;

            var location = locationClient.GetLocationAsync("1816bd91b39fd14706f70e6b1bf6f644", ipAddress, default(CancellationToken)).Result;
        }   
    }
}
