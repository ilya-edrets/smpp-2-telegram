using System.Collections.Generic;

namespace WorkerHost.Models
{
    public record SmppConfiguration(IReadOnlyCollection<SmppChannelConfiguration> Channels)
    {
    }
}
