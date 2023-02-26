using System.Collections.Generic;
using Infrastructure.Smpp.Configuration;

namespace Infrastructure.Smpp
{
    public class SmppClientFactoryOptions
    {
        public Dictionary<int, SmppChannelConfiguration> SmppChannelConfigurations { get; } = new();
    }
}
