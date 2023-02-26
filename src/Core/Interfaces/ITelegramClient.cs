using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface ITelegramClient
    {
        Task SendMessage(TelegramChat chat, string sender, string message);
    }
}
