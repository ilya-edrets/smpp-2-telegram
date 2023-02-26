namespace WorkerHost.Models
{
    public record SmppChannelConfiguration(int ChannelId, string Host, int Port, string SystemId, string Password)
    {
    }
}
