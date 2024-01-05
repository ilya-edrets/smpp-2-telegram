using Infrastructure.Smpp.Abstractions;
using Infrastructure.Smpp.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared;

namespace Infrastructure.Smpp;

internal class SmppClientFactory : ISmppClientFactory, IDisposable
{
    private readonly IOptionsMonitor<SmppClientFactoryOptions> optionsMonitor;
    private readonly ILogger<SmppClientFactory> logger;

    private readonly Dictionary<int, SmppClient> clients = new();

    public SmppClientFactory(IOptionsMonitor<SmppClientFactoryOptions> optionsMonitor, ILogger<SmppClientFactory> logger)
    {
        Guard.NotNull(optionsMonitor, nameof(optionsMonitor));
        Guard.NotNull(logger, nameof(logger));

        this.optionsMonitor = optionsMonitor;
        this.logger = logger;
    }

    public ISmppClient GetSmppClient(int channelId)
    {
        if (this.clients.TryGetValue(channelId, out var client))
        {
            return client;
        }

        if (this.optionsMonitor.CurrentValue.SmppChannelConfigurations.TryGetValue(channelId, out var configuration))
        {
            client = new SmppClient(configuration);
            this.clients.Add(channelId, client);

            return client;
        }

        throw new UnknownChannelIdException(channelId);
    }

    public void Dispose()
    {
        foreach (var smppClient in this.clients.Values)
        {
            smppClient.Dispose();
        }
    }
}
