using System.Collections.Generic;

namespace Core.Entities
{
    public class TelegramChat : EntityBase<long>
    {
        public TelegramChat(long id)
        : base(id)
        {
            this.TelegramThreads = new List<TelegramThread>();
        }

        public SmppChannel? SmppChannel { get; set; }

        public List<TelegramThread> TelegramThreads { get; set; }
    }
}
