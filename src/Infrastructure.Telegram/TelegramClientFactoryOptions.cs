using System.Collections.Generic;
using Infrastructure.Telegram.Configuration;

namespace Infrastructure.Telegram
{
    public class TelegramClientFactoryOptions
    {
        public Dictionary<string, TelegramChatConfiguration> TelegramChatConfigurations { get; } = new();
    }
}
