namespace Infrastructure.Smpp.Abstractions;

public interface ISmppClient
{
    IObservable<SmppMessage> Messages { get; }

    Task SendMessageAsync(SmppMessage message);
}
