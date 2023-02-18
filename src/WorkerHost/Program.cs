using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkerHost.Controllers;
using WorkerHost.Interfaces;
using WorkerHost.Models;

namespace WorkerHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(ConfigureServices)
                .Build();

            host.Run();
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            var telegramConfiguration = context.Configuration.GetSection("Telegram").Get<TelegramConfiguration>() ?? throw new ArgumentNullException(nameof(TelegramConfiguration));

            services.AddSingleton<ITelegramController, TelegramController>();
            services.AddSingleton<TelegramConfiguration>(telegramConfiguration);
            services.AddHostedService<TelegramWorker>();
        }
    }
}