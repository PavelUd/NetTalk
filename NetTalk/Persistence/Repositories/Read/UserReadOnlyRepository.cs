

using Application.Common.Interfaces.Repositories.Query;
using Application.Queries.QueryModels;
using MongoDB.Driver;

namespace Persistence.Repositories.Read;

public class UserReadOnlyRepository(IReadDbContext readDbContext) : BaseReadOnlyRepository<UserQueryModel, Guid>(readDbContext), IUserReadOnlyRepository
{

}