using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces;
using JamaaTech.Smpp.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WorkerHost.Models;

namespace WorkerHost
{
    public class SmppWorker : IHostedService, IDisposable
    {
        private readonly IServiceProvider serviceProvider;
        private readonly SmppConfiguration configuration;
        private readonly ILogger<SmppWorker> logger;

        private List<SmppClient> clients;

        public SmppWorker(IServiceProvider serviceProvider, SmppConfiguration configuration, ILogger<SmppWorker> logger)
        {
            this.serviceProvider = serviceProvider;
            this.configuration = configuration;
            this.logger = logger;

            this.clients = new List<SmppClient>(configuration.Channels.Count);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            this.logger.LogInformation("Starting to listen incoming messages...");

            foreach (var channel in this.configuration.Channels)
            {
                var client = this.CreateClient(channel, cancellationToken);
                this.clients.Add(client);
            }

            this.logger.LogInformation("Started");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Stopping to listen incoming messages...");

            foreach (var smppClient in this.clients)
            {
                smppClient.Shutdown();
            }

            this.logger.LogInformation("Stopped");

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            foreach (var smppClient in this.clients)
            {
                smppClient.Dispose();
            }
        }

        private SmppClient CreateClient(SmppChannelConfiguration channelConfiguration, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var client = new SmppClient();

            var properties = client.Properties;
            properties.Host = channelConfiguration.Host;
            properties.Port = channelConfiguration.Port;
            properties.SystemID = channelConfiguration.SystemId;
            properties.Password = channelConfiguration.Password;

            properties.SystemType = "mysystemtype";
            properties.DefaultServiceType = "mydefaultservicetype";

            // Resume a lost connection after 30 seconds
            client.AutoReconnectDelay = 3000;
            client.KeepAliveInterval = 15000;
            client.MessageReceived += (_, arg) => this.MessageReceived(channelConfiguration.ChannelId, arg.ShortMessage as TextMessage);

            client.Start();

            return client;
        }

        private void MessageReceived(int channelId, TextMessage? message)
        {
            if (message == null)
            {
                return;
            }

            this.logger.LogInformation("Received incoming message from channel {channelId}", channelId);

            using var scope = this.serviceProvider.CreateScope();
            var manager = scope.ServiceProvider.GetRequiredService<IMessageManager>();

            manager.ProcessSmppMessage(channelId, message.SourceAddress, message.Text);
        }
    }
}
