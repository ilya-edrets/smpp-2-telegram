using System.IO;
using Infrastructure.Smpp.Abstractions;
using Infrastructure.Smpp.Configuration;
using Infrastructure.Smpp.Extensions;
using Infrastructure.Telegram;
using Infrastructure.Telegram.Abstractions;
using Infrastructure.Telegram.Configuration;
using Infrastructure.Telegram.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Shared;
using WorkerHost.Configuration;

namespace WorkerHost;

public class Program
{
    public static Task Main()
    {
        var environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.local.json", optional: true)
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var smppChannelConfiguration = configuration.GetSection(nameof(SmppChannelConfiguration)).Get<SmppChannelConfiguration>()!;
        var telegramBotConfiguration = configuration.GetSection(nameof(TelegramBotConfiguration)).Get<TelegramBotConfiguration>()!;
        var telegramConversationConfiguration = configuration.GetSection(nameof(TelegramConversationConfiguration)).Get<TelegramConversationConfiguration>()!;

        var services = new ServiceCollection();
        services.AddLogging(builder => builder.AddSerilog(dispose: true).AddConsole());
        services.AddSmppClient(smppChannelConfiguration);
        services.AddTelegramBot(telegramBotConfiguration);
        var sp = services.BuildServiceProvider();

        var logger = sp.GetRequiredService<ILogger<Program>>();
        var smppClientFactory = sp.GetRequiredService<ISmppClientFactory>();
        var telegramClientFactory = sp.GetRequiredService<ITelegramBotFactory>();

        var smppClient = smppClientFactory.GetSmppClient(smppChannelConfiguration.ChannelId);
        var telegramClient = telegramClientFactory.GetTelegramBot(telegramBotConfiguration.Name);

        smppClient.Messages.Subscribe(smppMessage =>
        {
            logger.LogInformation("Received an icoming sms from: {Sender} with text: {Text}", smppMessage.Sender, smppMessage.Message);

            var message = $"{smppMessage.Sender}\n\n{smppMessage.Message}";
            var telegramMessage = new TelegramMessage(telegramConversationConfiguration.ChatId, telegramConversationConfiguration.ThreadId, message);
            telegramClient.SendMessageAsync(telegramMessage).ContinueWith(
                result =>
                {
                    if (result.Status == TaskStatus.Faulted)
                    {
                        logger.LogError(result.Exception, "Something went wrong during a sending sms from {Sender} to telegram chat ", smppMessage.Sender);
                    }
                    else
                    {
                        logger.LogInformation("Sms from {Sender} has been sent to telegram chat ", smppMessage.Sender);
                    }
                },
                TaskScheduler.Default).Forget();
        });

        logger.LogInformation("Application has been successfully started");
        logger.LogInformation("SMPP Endpoint: {Host}:{Port}", smppChannelConfiguration.Host, smppChannelConfiguration.Port);
        logger.LogInformation("Telegram ChatId: {ChatId}", telegramConversationConfiguration.ChatId);

        Console.WriteLine("Press Ctrl+C to stop the application");
        return Task.Delay(Timeout.Infinite);
    }
}
