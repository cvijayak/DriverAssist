using System.Threading;
using System.Threading.Tasks;

namespace DriverAssist.Infrastructure.Common
{
    public interface IWhatsappClient
    {
        Task<bool> SendAsync(WhatsappMessage message, CancellationToken cancellationToken);
    }
}
