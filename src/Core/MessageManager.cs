using System;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Core
{
    internal class MessageManager : IMessageManager
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ITelegramClient telegramClient;
        private readonly ILogger<MessageManager> logger;

        public MessageManager(ApplicationDbContext dbContext, ITelegramClient telegramClient, ILogger<MessageManager> logger)
        {
            this.dbContext = dbContext;
            this.telegramClient = telegramClient;
            this.logger = logger;
        }

        public async Task ProcessSmppMessage(int channelId, string sender, string message)
        {
            this.logger.LogInformation("Process Smpp Message from {channelId} channel", channelId);

            var channel = await this.dbContext.SmppChannels
                .Include(x => x.TelegramChat)
                .SingleOrDefaultAsync(x => x.Id == channelId);

            if (channel?.TelegramChat == null)
            {
                this.logger.LogWarning("Channel {channelId} isn't assigned to any chat", channelId);
                return;
            }

            await this.telegramClient.SendMessage(channel.TelegramChat, sender, message);

            this.logger.LogInformation("Smpp Message from {channelId} channel is processed", channelId);
        }

        public Task ProcessTelegramMessage(long chatId, string receiver, string message)
        {
            throw new NotImplementedException();
        }
    }
}
