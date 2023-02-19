using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IChannelManager
    {
        Task<IEnumerable<SmppChannel>> GetChannels(CancellationToken cancellationToken);

        Task AssignChannel(long chatId, int channelId, CancellationToken cancellationToken);
    }
}
