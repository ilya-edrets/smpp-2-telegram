namespace Infrastructure.Smpp.Abstractions
{
    public interface ISmppClientFactory
    {
        ISmppClient GetSmppClient(int channelId);
    }
}
