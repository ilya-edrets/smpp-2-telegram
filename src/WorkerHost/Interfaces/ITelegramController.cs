using System.Threading;
using System.Threading.Tasks;

namespace WorkerHost.Interfaces
{
    public interface ITelegramController
    {
        Task<string> GetAllChannels(CancellationToken cancellationToken);

        Task<string> AssignChannel(long chatId, int smppChannel, CancellationToken cancellationToken);

        Task<string?> SendMessage(long chatId, int? threadId, string message, CancellationToken cancellationToken);

        Task<string?> SetPhoneNumber(long chatId, int? threadId, string phoneNumber, CancellationToken cancellationToken);

        Task<string> GetCurrentThreadInfo(long chatId, int? threadId, CancellationToken cancellationToken);

        Task<string> GetAllPhoneNumbers(CancellationToken cancellationToken);
    }
}
