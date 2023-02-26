using JamaaTech.Smpp.Net.Client;

namespace Infrastructure.Smpp
{
    public record SmppMessage(string Sender, string Receiver, string Message)
    {
        public static readonly SmppMessage EmptyMessage = new(string.Empty, string.Empty, string.Empty);
    }
}
