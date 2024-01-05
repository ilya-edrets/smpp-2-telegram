using Infrastructure.Telegram.Abstractions;
using Infrastructure.Telegram.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared;

namespace Infrastructure.Telegram.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTelegramBot(this IServiceCollection services, TelegramBotConfiguration configuration)
    {
        Guard.NotNull(services, nameof(services));
        Guard.NotNull(configuration, nameof(configuration));

        services.AddOptions();
        services.AddLogging();

        services.TryAddSingleton<ITelegramBotFactory, TelegramBotFactory>();

        services.Configure<TelegramBotFactoryOptions>(options => options.TelegramBotConfigurations.Add(configuration.Name, configuration));

        return services;
    }
}
