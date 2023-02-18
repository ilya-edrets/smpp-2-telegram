using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WorkerHost.Interfaces;

namespace WorkerHost.Controllers
{
    public class TelegramController : ITelegramController
    {
        private readonly ILogger<TelegramController> logger;

        public TelegramController(ILogger<TelegramController> logger)
        {
            this.logger = logger;
        }

        public Task<string> GetAllChannels(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult("channels");
        }

        public Task<string> AssignChannel(long chatId, int channel, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult($"SMPP channel {channel} is assigned to chat {chatId}");
        }

        public Task<string?> SendMessage(long chatId, int? messageMessageThreadId, string message, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult($"Send sms to {chatId} {messageMessageThreadId} with text {message}")!;
        }
    }
}
