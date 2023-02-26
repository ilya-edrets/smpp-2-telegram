namespace Infrastructure.Telegram
{
    public record TelegramMessage(long ChatId, int? ThreadId, string Message)
    {
        public static readonly TelegramMessage EmptyMessage = new(default, default, string.Empty);
    }
}
