using System.Reflection;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using File = Domain.Entities.File;

namespace Persistence.Contexts;

public class NetTalkDbContext : DbContext
{

    public DbSet<Chat> Chats { get; set; }
    public DbSet<SymmetricKey> SymmetricKeys { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<MessageStatus> MessageStatuses { get; set; }
    public DbSet<File> Files { get; set; }
    public DbSet<ChatInvite> Invites { get; set; }


    public NetTalkDbContext(DbContextOptions<NetTalkDbContext> options)
        : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat>()
            .HasMany(c => c.Messages)
            .WithOne()
            .HasForeignKey(s => s.IdChat);
        
        modelBuilder.Entity<Message>()
            .HasMany(m => m.StatusList)
            .WithOne()
            .HasForeignKey(s => s.IdMessage);
        
        modelBuilder.Entity<Message>()
            .HasMany(m => m.Files)
            .WithOne()
            .HasForeignKey(s => s.IdMessage);
        modelBuilder.Entity<User>()
            .HasOne(us => us.Key)
            .WithOne()
            .HasForeignKey<SymmetricKey>(k => k.IdUser);
        
        modelBuilder.Entity<Chat>()
            .HasMany(c => c.Users)  
            .WithMany(u => u.Chats) 
            .UsingEntity<Dictionary<string, object>>(
                "users_chats", 
                j => j.HasOne<User>().WithMany().HasForeignKey("id_user"),  // Внешний ключ для User
                j => j.HasOne<Chat>().WithMany().HasForeignKey("id_chat")  // Внешний ключ для Chat
            );
        
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    
    public void BeginTransaction()
    {
        Database.BeginTransaction();
    }

    public void CommitTransaction()
    {
        Database.CommitTransaction();
    }

    public void RollbackTransaction()
    {
        Database.RollbackTransaction();
    }
}
