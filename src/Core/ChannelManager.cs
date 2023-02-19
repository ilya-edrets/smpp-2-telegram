using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Core
{
    public class ChannelManager : IChannelManager
    {
        private readonly ApplicationDbContext dbContext;

        public ChannelManager(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<SmppChannel>> GetChannels(CancellationToken cancellationToken)
        {
            var channels = await this.dbContext.SmppChannels.Include(x => x.TelegramChat).ToListAsync(cancellationToken);
            return channels;
        }

        public async Task AssignChannel(long chatId, int channelId, CancellationToken cancellationToken)
        {
            var chatTask = this.EnsureTelegramChat(chatId, cancellationToken);
            var channelTask = this.EnsureSmppChannel(channelId, cancellationToken);
            await Task.WhenAll(chatTask, channelTask);

            var chat = await chatTask;
            var channel = await channelTask;

            chat.SmppChannel = channel;
            channel.TelegramChat = chat;

            await this.dbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task<TelegramChat> EnsureTelegramChat(long chatId, CancellationToken cancellationToken)
        {
            var chat = await this.dbContext.TelegramChats
                .Include(x => x.SmppChannel)
                .SingleOrDefaultAsync(x => x.Id == chatId, cancellationToken);

            if (chat == null)
            {
                chat = new TelegramChat(chatId);
                await this.dbContext.TelegramChats.AddAsync(chat, cancellationToken);
            }

            return chat;
        }

        private async Task<SmppChannel> EnsureSmppChannel(int channelId, CancellationToken cancellationToken)
        {
            var channel = await this.dbContext.SmppChannels
                .Include(x => x.TelegramChat)
                .SingleOrDefaultAsync(x => x.Id == channelId, cancellationToken);

            if (channel == null)
            {
                channel = new SmppChannel(channelId);
                await this.dbContext.SmppChannels.AddAsync(channel, cancellationToken);
            }

            return channel;
        }
    }
}
