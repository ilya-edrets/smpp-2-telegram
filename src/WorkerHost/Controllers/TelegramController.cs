using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.Extensions.Logging;
using WorkerHost.Interfaces;

namespace WorkerHost.Controllers
{
    public class TelegramController : ITelegramController
    {
        private readonly IChannelManager channelManager;
        private readonly ILogger<TelegramController> logger;

        public TelegramController(IChannelManager channelManager, ILogger<TelegramController> logger)
        {
            this.channelManager = channelManager;
            this.logger = logger;
        }

        public async Task<string> GetAllChannels(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var channels = await this.channelManager.GetChannels(cancellationToken);

            var sb = new StringBuilder();
            foreach (var smppChannel in channels)
            {
                var assignedPart = smppChannel.TelegramChat != null ? $"is assigned to chat {smppChannel.TelegramChat.Id}" : "is not assigned";
                sb.AppendLine($"Channel '{smppChannel.Name ?? "unnamed"}' (id:{smppChannel.Id}) {assignedPart}");
            }

            return sb.ToString();
        }

        public async Task<string> AssignChannel(long chatId, int channelId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await this.channelManager.AssignChannel(chatId, channelId, cancellationToken);

            return "Done";
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
