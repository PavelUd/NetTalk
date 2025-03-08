using Application.Common.Interfaces.Repositories.Commands;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories.Write;

public class MessageRepository : BaseRepository<Message>, IMessageRepository
{
    public MessageRepository(NetTalkDbContext context) : base(context)
    {
    }

    public new Task DeleteAsync(Message entity)
    {
        entity.MarkAsDeleted();
        base.DeleteAsync(entity);
        return Task.CompletedTask;
    }

    public new Task UpdateAsync(Message entity)
    {
        entity.MarkAsUpdated();
        base.UpdateAsync(entity);
        return Task.CompletedTask;
    }
}