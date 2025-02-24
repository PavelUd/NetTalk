using Application.Queries.QueryModels;

namespace Application.Common.Interfaces.Repositories.Query;

public interface IUserReadOnlyRepository
{
    public Task<IEnumerable<UserQueryModel>> GetAllAsync();
    
    public Task<UserQueryModel> GetByIdAsync(Guid id);
}