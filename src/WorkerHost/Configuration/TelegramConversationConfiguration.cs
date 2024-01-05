namespace WorkerHost.Configuration;

internal record TelegramConversationConfiguration(long ChatId, int? ThreadId = null);
