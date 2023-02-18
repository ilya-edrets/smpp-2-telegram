using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WorkerHost.Extensions;
using WorkerHost.Interfaces;
using WorkerHost.Models;

namespace WorkerHost
{
    public class TelegramWorker : IHostedService
    {
        private readonly ITelegramController controller;
        private readonly TelegramConfiguration configuration;
        private readonly ILogger<TelegramWorker> logger;

        public TelegramWorker(ITelegramController controller, TelegramConfiguration configuration, ILogger<TelegramWorker> logger)
        {
            this.controller = controller;
            this.configuration = configuration;
            this.logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var telegramClient = new TelegramBotClient(this.configuration.Token);
            var options = new ReceiverOptions { AllowedUpdates = new[] { UpdateType.Message } };

            telegramClient.StartReceiving(this.UpdateHandler, this.PollingErrorHandler, options, cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!this.IsMessageUpdate(update)
             || !this.IsAuthorized(update)
             || !this.IsNotEmptyMessage(update))
            {
                return;
            }

            this.logger.LogInformation("Chat id: {chatId}, ThreadId: {messageThreadId}, Message: {message}", update.Message!.Chat.Id, update.Message.MessageThreadId, update.Message.Text);

            var response = await this.CallController(update, cancellationToken);

            if (response != null)
            {
                await client.SendTextMessageAsync(update.Message.Chat.Id, response, update.Message.MessageThreadId, cancellationToken: cancellationToken);
            }
        }

        private Task PollingErrorHandler(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
        {
            this.logger.LogError(exception, "Unknown error is raised");
            return Task.CompletedTask;
        }

        private bool IsMessageUpdate(Update update)
        {
            return update.Message != null;
        }

        private bool IsAuthorized(Update update)
        {
            return update.Message?.From?.Username != null && update.Message.From.Username == this.configuration.Owner;
        }

        private bool IsNotEmptyMessage(Update update)
        {
            return update.Message?.Text != null;
        }

        private Task<string?> CallController(Update update, CancellationToken cancellationToken)
        {
            var message = update.Message!.Text!;

            if (message.StartsWith("/getchannels", StringComparison.OrdinalIgnoreCase))
            {
                return this.controller.GetAllChannels(cancellationToken).AsNullable();
            }

            if (message.StartsWith("/assignchannel", StringComparison.OrdinalIgnoreCase))
            {
                var splits = message.Split(' ');
                if (splits.Length != 2 || !int.TryParse(splits[1], out var channel))
                {
                    this.logger.LogError("Incorrect format of /assign-channel command: {message}", message);
                    return "Incorrect format of /assign-channel command".AsTask<string?>();
                }

                return this.controller.AssignChannel(update.Message.Chat.Id, channel, cancellationToken).AsNullable();
            }

            return this.controller.SendMessage(update.Message.Chat.Id, update.Message.MessageThreadId, message, cancellationToken);
        }
    }
}