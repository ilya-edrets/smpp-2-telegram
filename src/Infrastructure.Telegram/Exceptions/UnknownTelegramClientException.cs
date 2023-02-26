using System;

namespace Infrastructure.Telegram.Exceptions
{
    public class UnknownTelegramClientException : Exception
    {
        public UnknownTelegramClientException(string clientName)
            : base($"Unknown telegram client {clientName}")
        {
            this.ClientName = clientName;
        }

        public string ClientName { get; }
    }
}
