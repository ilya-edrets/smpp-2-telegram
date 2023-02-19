using System;
using Core;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
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
        public static void Main()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(ConfigureAppConfiguration)
                .ConfigureServices(ConfigureServices)
                .Build();

            host.Run();
        }

        private static void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder builder)
        {
            var env = context.HostingEnvironment;

            builder
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.local.json", optional: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            var config = context.Configuration;

            var telegramConfiguration = config.GetSection("Telegram").Get<TelegramConfiguration>() ?? throw new ArgumentNullException("telegramConfiguration");
            var connectionString = config.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("connectionString");

            services.AddScoped<IChannelManager, ChannelManager>();
            services.AddScoped<ITelegramController, TelegramController>();
            services.AddSingleton<TelegramConfiguration>(telegramConfiguration);
            services.AddHostedService<TelegramWorker>();

            services.AddDbContext<ApplicationDbContext>(builder => builder.UseSqlite(connectionString));
        }
    }
}