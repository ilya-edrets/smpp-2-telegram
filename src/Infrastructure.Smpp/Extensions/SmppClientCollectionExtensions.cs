using Infrastructure.Smpp.Abstractions;
using Infrastructure.Smpp.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared;

namespace Infrastructure.Smpp.Extensions;

public static class SmppClientCollectionExtensions
{
    public static IServiceCollection AddSmppClient(this IServiceCollection services, SmppChannelConfiguration configuration)
    {
        Guard.NotNull(services, nameof(services));
        Guard.NotNull(configuration, nameof(configuration));

        services.AddOptions();
        services.AddLogging();

        services.TryAddSingleton<ISmppClientFactory, SmppClientFactory>();

        services.Configure<SmppClientFactoryOptions>(options => options.SmppChannelConfigurations.Add(configuration.ChannelId, configuration));

        return services;
    }
}
