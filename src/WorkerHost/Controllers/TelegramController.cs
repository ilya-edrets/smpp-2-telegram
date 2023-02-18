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

        public Task<string?> SendMessage(long chatId, int? threadId, string message, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult($"Send sms to {chatId} {threadId} with text {message}")!;
        }

        public Task<string?> SetPhoneNumber(long chatId, int? threadId, string phoneNumber, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetCurrentThreadInfo(long chatId, int? threadId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult($"Chat id: {chatId}, thread id: {threadId}")!;
        }

        public Task<string> GetAllPhoneNumbers(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            throw new System.NotImplementedException();
        }
    }
}
