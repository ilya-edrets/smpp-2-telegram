using Telegram.Bot.Types;

namespace Infrastructure.Telegram.Extensions;

internal static class TelegramMessageExtensions
{
    internal static TelegramMessage ToTelegramMessage(this Update update)
    {
        return new TelegramMessage(update.Message!.Chat.Id, update.Message.MessageThreadId, update.Message.Text!);
    }
}
