using Application.Queries.QueryModels;

namespace Application.Common.Interfaces.Repositories.Query;

public interface IMessageReadOnlyRepository
{
    public Task<MessageQueryModel> GetByIdAsync(Guid id);
    
    public Task<IEnumerable<MessageQueryModel>> GetChatMessages(Guid idChat);
}