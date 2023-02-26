using System;
using System.Threading.Tasks;

namespace Infrastructure.Telegram.Abstractions
{
    public interface ITelegramClient
    {
        IObservable<TelegramMessage> Messages { get; }

        Task SendMessage(TelegramMessage message);
    }
}
