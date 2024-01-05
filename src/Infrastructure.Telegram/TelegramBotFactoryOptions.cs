using Infrastructure.Telegram.Configuration;

namespace Infrastructure.Telegram;

public class TelegramBotFactoryOptions
{
    public Dictionary<string, TelegramBotConfiguration> TelegramBotConfigurations { get; } = new();
}
