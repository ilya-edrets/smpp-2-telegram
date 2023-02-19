using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly IServiceProvider serviceProvider;
        private readonly TelegramConfiguration configuration;
        private readonly ILogger<TelegramWorker> logger;

        public TelegramWorker(IServiceProvider serviceProvider, TelegramConfiguration configuration, ILogger<TelegramWorker> logger)
        {
            this.serviceProvider = serviceProvider;
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
            try
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

                if (!string.IsNullOrWhiteSpace(response))
                {
                    await client.SendTextMessageAsync(update.Message.Chat.Id, response, update.Message.MessageThreadId, cancellationToken: cancellationToken);
                }
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "Unknown exception was raised");
            }
        }

        private Task PollingErrorHandler(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
        {
            this.logger.LogError(exception, "Unknown exception was raised");
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
            using var scope = this.serviceProvider.CreateScope();
            var controller = scope.ServiceProvider.GetRequiredService<ITelegramController>();

            var message = update.Message!.Text!;

            if (message.StartsWith("/help", StringComparison.OrdinalIgnoreCase))
            {
                return @"
                    Supported commands:
                    /help - this message
                    /getchannels - get all available smpp channels
                    /assignchannel - assign selected channel to the current chat
                    /setnumber - set phone number to the current thread

                    Debug:
                    /currentthread - get current thread id
                    /allnumbers - get all known phone number with associated thread ids
                ".AlignFromFirstLine()
                 .AsTask<string?>();
            }

            if (message.StartsWith("/getchannels", StringComparison.OrdinalIgnoreCase))
            {
                return controller.GetAllChannels(cancellationToken).AsNullable();
            }

            if (message.StartsWith("/assignchannel", StringComparison.OrdinalIgnoreCase))
            {
                var splits = message.Split(' ');
                if (splits.Length != 2 || !int.TryParse(splits[1], out var channel))
                {
                    this.logger.LogError("Incorrect format of /assignchannel command: {message}", message);
                    return "Incorrect format of /assign-channel command".AsTask<string?>();
                }

                return controller.AssignChannel(update.Message.Chat.Id, channel, cancellationToken).AsNullable();
            }

            if (message.StartsWith("/setnumber", StringComparison.OrdinalIgnoreCase))
            {
                var splits = message.Split(' ');
                if (splits.Length != 2)
                {
                    this.logger.LogError("Incorrect format of /setnumber command: {message}", message);
                    return "Incorrect format of /setnumber command".AsTask<string?>();
                }

                return controller.SetPhoneNumber(update.Message.Chat.Id, update.Message.MessageThreadId, splits[1], cancellationToken).AsNullable();
            }

            if (message.StartsWith("/currentthread", StringComparison.OrdinalIgnoreCase))
            {
                return controller.GetCurrentThreadInfo(update.Message.Chat.Id, update.Message.MessageThreadId, cancellationToken).AsNullable();
            }

            if (message.StartsWith("/allnumbers", StringComparison.OrdinalIgnoreCase))
            {
                return controller.GetAllPhoneNumbers(cancellationToken).AsNullable();
            }

            return controller.SendMessage(update.Message.Chat.Id, update.Message.MessageThreadId, message, cancellationToken);
        }
    }
}