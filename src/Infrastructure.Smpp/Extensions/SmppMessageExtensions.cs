using JamaaTech.Smpp.Net.Client;

namespace Infrastructure.Smpp.Extensions
{
    internal static class SmppMessageExtensions
    {
        internal static SmppMessage ToSmppMessage(this TextMessage? textMessage)
        {
            return textMessage != null ? new SmppMessage(textMessage.SourceAddress, textMessage.DestinationAddress, textMessage.Text) : SmppMessage.EmptyMessage;
        }

        internal static TextMessage ToTextMessage(this SmppMessage smppMessage)
        {
            return new TextMessage
            {
                DestinationAddress = smppMessage.Receiver,
                Text = smppMessage.Message,
            };
        }
    }
}
