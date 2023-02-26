using Infrastructure.Telegram.Abstractions;
using Infrastructure.Telegram.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared;

namespace Infrastructure.Telegram.Extensions
{
    public static class TelegramClientCollectionExtensions
    {
        public static IServiceCollection AddTelegramClient(this IServiceCollection services, TelegramChatConfiguration configuration)
        {
            Guard.NotNull(services, nameof(services));
            Guard.NotNull(configuration, nameof(configuration));

            services.AddOptions();
            services.AddLogging();

            services.TryAddSingleton<ITelegramClientFactory, TelegramClientFactory>();

            services.Configure<TelegramClientFactoryOptions>(options => options.TelegramChatConfigurations.Add(configuration.Name, configuration));

            return services;
        }
    }
}
