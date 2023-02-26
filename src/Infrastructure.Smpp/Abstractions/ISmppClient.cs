using System;
using System.Threading.Tasks;

namespace Infrastructure.Smpp.Abstractions
{
    public interface ISmppClient
    {
        IObservable<SmppMessage> Messages { get; }

        Task SendMessage(SmppMessage message);
    }
}
