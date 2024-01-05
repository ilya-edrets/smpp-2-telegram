namespace Infrastructure.Smpp.Configuration;

public record SmppChannelConfiguration(int ChannelId, string Host, int Port, string SystemId, string Password);
