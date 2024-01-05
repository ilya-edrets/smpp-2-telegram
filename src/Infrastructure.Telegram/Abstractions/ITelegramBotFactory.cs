namespace Infrastructure.Telegram.Abstractions;

public interface ITelegramBotFactory
{
    ITelegramBot GetTelegramBot(string botName);
}
