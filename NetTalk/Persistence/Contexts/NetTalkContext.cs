using System.Reflection;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using File = Domain.Entities.File;

namespace Persistence.Contexts;

public partial class NetTalkContext : DbContext
{

    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<MessageStatus> MessageStatuses { get; set; }
    public DbSet<File> Files { get; set; }
    public DbSet<ChatInvite> Invites { get; set; }


    public NetTalkContext(DbContextOptions<NetTalkContext> options)
        : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat>().HasMany<Message>()
            .WithOne()
            .HasForeignKey(m => m.IdChat);
        
        modelBuilder.Entity<Message>()
            .HasMany(m => m.StatusList)
            .WithOne()
            .HasForeignKey(s => s.IdMessage);
        
        modelBuilder.Entity<Message>()
            .HasMany(m => m.Files)
            .WithOne()
            .HasForeignKey(s => s.IdMessage);
        
        modelBuilder.Entity<Chat>()
            .HasMany(c => c.Users)  
            .WithMany(u => u.Chats) 
            .UsingEntity<Dictionary<string, object>>(
                "users_chats", 
                j => j.HasOne<User>().WithMany().HasForeignKey("UserId"),  // Внешний ключ для User
                j => j.HasOne<Chat>().WithMany().HasForeignKey("ChatId")  // Внешний ключ для Chat
            );
        
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
