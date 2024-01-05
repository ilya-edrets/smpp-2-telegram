#pragma warning disable RCS1194 // Implement exception constructors.

namespace Infrastructure.Smpp.Exceptions;

public class UnknownChannelIdException : Exception
{
    public UnknownChannelIdException(int channelId)
        : base($"Unknown channel id {channelId}")
    {
        this.ChannelId = channelId;
    }

    public int ChannelId { get; }
}
