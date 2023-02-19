namespace Core.Entities
{
    public class SmppChannel : EntityBase<int>
    {
        public SmppChannel(int id)
        : base(id)
        {
        }

        public string? Name { get; set; }

        public long? TelegramChatId { get; set; }

        public TelegramChat? TelegramChat { get; set; }
    }
}
