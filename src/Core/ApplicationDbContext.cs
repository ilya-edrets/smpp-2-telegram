using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }

        public DbSet<SmppChannel> SmppChannels { get; set; } = null!;

        public DbSet<TelegramChat> TelegramChats { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TelegramChat>()
                .HasOne(x => x.SmppChannel)
                .WithOne(x => x.TelegramChat)
                .HasForeignKey<SmppChannel>(x => x.TelegramChatId);

            modelBuilder.Entity<SmppChannel>().Property(b => b.Id).ValueGeneratedNever();
            modelBuilder.Entity<TelegramChat>().Property(b => b.Id).ValueGeneratedNever();
            modelBuilder.Entity<TelegramThread>().Property(b => b.Id).ValueGeneratedNever();
        }
    }
}
