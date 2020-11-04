using System.Threading;
using System.Threading.Tasks;

namespace DriverAssist.Infrastructure.Common
{
    public interface ISmsClient
    {
        Task<bool> SendAsync(SmsMessage message, CancellationToken cancellationToken);
    }
}