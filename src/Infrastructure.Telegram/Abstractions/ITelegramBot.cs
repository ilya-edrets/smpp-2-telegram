namespace Infrastructure.Telegram.Abstractions;

public interface ITelegramBot
{
    IObservable<TelegramMessage> Messages { get; }

    Task SendMessageAsync(TelegramMessage message);
}
