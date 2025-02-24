using Application.Common.Interfaces.Repositories.Query;
using Application.Queries.QueryModels;
using MongoDB.Driver;

namespace Persistence.Repositories.Read;

public class MessageReadOnlyRepository(IReadDbContext readDbContext) : BaseReadOnlyRepository<MessageQueryModel, Guid>(readDbContext), IMessageReadOnlyRepository
{
    public async Task<IEnumerable<MessageQueryModel>> GetChatMessages(Guid idChat)
    {
        using var asyncCursor = await Collection.FindAsync(queryModel => queryModel.ChatId.Equals(idChat));
        return await asyncCursor.ToListAsync();
    }
}