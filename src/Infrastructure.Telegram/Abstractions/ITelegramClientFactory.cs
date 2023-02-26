namespace Infrastructure.Telegram.Abstractions
{
    public interface ITelegramClientFactory
    {
        ITelegramClient GetTelegramClient(string name);
    }
}
