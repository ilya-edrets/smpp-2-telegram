using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IMessageManager
    {
        Task ProcessSmppMessage(int channelId, string sender, string message);

        Task ProcessTelegramMessage(long chatId, string receiver, string message);
    }
}
