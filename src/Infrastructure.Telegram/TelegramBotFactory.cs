using Infrastructure.Telegram.Abstractions;
using Infrastructure.Telegram.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Shared;

namespace Infrastructure.Telegram;

public class TelegramBotFactory : ITelegramBotFactory
{
    private readonly IOptionsMonitor<TelegramBotFactoryOptions> optionsMonitor;
    private readonly ILogger<TelegramBotFactory> logger;

    private readonly Dictionary<string, TelegramBot> bots = new();

    public TelegramBotFactory(IOptionsMonitor<TelegramBotFactoryOptions> optionsMonitor, ILogger<TelegramBotFactory> logger)
    {
        Guard.NotNull(optionsMonitor, nameof(optionsMonitor));
        Guard.NotNull(logger, nameof(logger));

        this.optionsMonitor = optionsMonitor;
        this.logger = logger;
    }

    public ITelegramBot GetTelegramBot(string botName)
    {
        if (this.bots.TryGetValue(botName, out var bot))
        {
            return bot;
        }

        if (this.optionsMonitor.CurrentValue.TelegramBotConfigurations.TryGetValue(botName, out var configuration))
        {
            bot = new TelegramBot(configuration, new NullLogger<TelegramBot>());
            this.bots.Add(botName, bot);

            return bot;
        }

        throw new UnknownTelegramClientException(botName);
    }
}
