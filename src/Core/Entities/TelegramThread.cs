namespace Core.Entities
{
    public class TelegramThread : EntityBase<int>
    {
        public TelegramThread(int id)
        : base(id)
        {
        }

        public TelegramChat? TelegramChat { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
