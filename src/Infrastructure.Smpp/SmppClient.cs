using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Infrastructure.Smpp.Abstractions;
using Infrastructure.Smpp.Configuration;
using Infrastructure.Smpp.Extensions;
using JamaaTech.Smpp.Net.Client;

namespace Infrastructure.Smpp
{
    internal class SmppClient : ISmppClient
    {
        private readonly JamaaTech.Smpp.Net.Client.SmppClient innerClient;

        public SmppClient(SmppChannelConfiguration configuration)
        {
            this.innerClient = new JamaaTech.Smpp.Net.Client.SmppClient();

            var properties = this.innerClient.Properties;
            properties.Host = configuration.Host;
            properties.Port = configuration.Port;
            properties.SystemID = configuration.SystemId;
            properties.Password = configuration.Password;

            // Resume a lost connection after 30 seconds
            this.innerClient.AutoReconnectDelay = 3000;
            this.innerClient.KeepAliveInterval = 15000;

            this.Messages = Observable.FromEvent<EventHandler<MessageEventArgs>, SmppMessage>(
                conversion => (_, arg) => conversion((arg.ShortMessage as TextMessage).ToSmppMessage()),
                h =>
                {
                    this.innerClient.MessageReceived += h;
                    this.innerClient.Start();
                },
                h =>
                {
                    this.innerClient.MessageReceived -= h;
                    this.innerClient.Shutdown();
                });
        }

        public IObservable<SmppMessage> Messages { get; }

        public Task SendMessage(SmppMessage message)
        {
            this.innerClient.SendMessage(message.ToTextMessage());
            return Task.CompletedTask;
        }
    }
}
