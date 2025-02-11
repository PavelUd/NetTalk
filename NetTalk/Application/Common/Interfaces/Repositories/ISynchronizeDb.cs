using System.Linq.Expressions;

namespace Application.Common.Interfaces.Repositories;

public interface ISynchronizeDb
{
    public Task UpsertAsync<TQueryModel>(TQueryModel queryModel, Expression<Func<TQueryModel, bool>> upsertFilter)
        where TQueryModel : class, IQueryModel;
}