using Application.Queries.QueryModels;

namespace Application.Common.Interfaces.Repositories.Query;

public interface IChatReadOnlyRepository
{
    public Task<IEnumerable<ChatQueryModel>> GetAllAsync();
    
    public Task<ChatQueryModel> GetByIdAsync(Guid id);
}