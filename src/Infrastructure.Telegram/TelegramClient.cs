using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Telegram.Abstractions;
using Infrastructure.Telegram.Configuration;
using Infrastructure.Telegram.Extensions;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Infrastructure.Telegram
{
    internal class TelegramClient : ITelegramClient
    {
        private readonly ILogger<TelegramClient> logger;

        private readonly TelegramBotClient innerClient;

        public TelegramClient(TelegramChatConfiguration configuration, ILogger<TelegramClient> logger)
        {
            this.logger = logger;

            this.innerClient = new TelegramBotClient(configuration.Token);
            var options = new ReceiverOptions { AllowedUpdates = new[] { UpdateType.Message } };

            var cts = new CancellationTokenSource();

            var rawObservable = Observable.FromEvent<Update>(
                handler => this.innerClient.StartReceiving((_, update, _) => handler(update), this.PollingErrorHandler, options, cts.Token),
                _ => cts.Cancel());

            this.Messages = rawObservable
                .Where(update => update.Message?.From?.Username == configuration.Owner)
                .Where(update => update.Message?.Text != null)
                .Select(update => update.ToTelegramMessage());
        }

        public IObservable<TelegramMessage> Messages { get; }

        public async Task SendMessage(TelegramMessage message)
        {
            await this.innerClient.SendTextMessageAsync(message.ChatId, message.Message, message.ThreadId);
        }

        private void PollingErrorHandler(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
        {
            this.logger.LogError(exception, "Unknown exception was raised");
        }
    }
}
