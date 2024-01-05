using System.Reactive.Linq;
using Infrastructure.Smpp.Abstractions;
using Infrastructure.Smpp.Configuration;
using Infrastructure.Smpp.Extensions;
using JamaaTech.Smpp.Net.Client;

namespace Infrastructure.Smpp;

internal class SmppClient : ISmppClient, IDisposable
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
            handler =>
            {
                this.innerClient.MessageReceived += handler;
                this.innerClient.Start();
            },
            handler =>
            {
                this.innerClient.MessageReceived -= handler;
                this.innerClient.Shutdown();
            });
    }

    public IObservable<SmppMessage> Messages { get; }

    public Task SendMessageAsync(SmppMessage message)
    {
        this.innerClient.SendMessage(message.ToTextMessage());
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        this.innerClient.Dispose();
    }
}
