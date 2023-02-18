using System.Threading;
using System.Threading.Tasks;

namespace WorkerHost.Interfaces
{
    public interface ITelegramController
    {
        Task<string> GetAllChannels(CancellationToken cancellationToken);

        Task<string> AssignChannel(long chatId, int channel, CancellationToken cancellationToken);

        Task<string?> SendMessage(long chatId, int? messageMessageThreadId, string message, CancellationToken cancellationToken);
    }
}
