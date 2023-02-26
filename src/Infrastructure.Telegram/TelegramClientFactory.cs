using System.Collections.Generic;
using Infrastructure.Telegram.Abstractions;
using Infrastructure.Telegram.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Shared;

namespace Infrastructure.Telegram
{
    public class TelegramClientFactory : ITelegramClientFactory
    {
        private readonly IOptionsMonitor<TelegramClientFactoryOptions> optionsMonitor;
        private readonly ILogger<TelegramClientFactory> logger;

        private readonly Dictionary<string, TelegramClient> clients = new();

        public TelegramClientFactory(IOptionsMonitor<TelegramClientFactoryOptions> optionsMonitor, ILogger<TelegramClientFactory> logger)
        {
            Guard.NotNull(optionsMonitor, nameof(optionsMonitor));
            Guard.NotNull(logger, nameof(logger));

            this.optionsMonitor = optionsMonitor;
            this.logger = logger;
        }

        public ITelegramClient GetTelegramClient(string clientName)
        {
            if (this.clients.TryGetValue(clientName, out var client))
            {
                return client;
            }

            if (this.optionsMonitor.CurrentValue.TelegramChatConfigurations.TryGetValue(clientName, out var configuration))
            {
                client = new TelegramClient(configuration, new NullLogger<TelegramClient>());
                this.clients.Add(clientName, client);

                return client;
            }

            throw new UnknownTelegramClientException(clientName);
        }
    }
}