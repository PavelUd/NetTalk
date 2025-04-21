using System.Linq.Expressions;
using MongoDB.Driver;

namespace Application.Common.Interfaces.Repositories;

public interface ISynchronizeDb
{
    public Task UpsertAsync<TQueryModel>(TQueryModel queryModel, Expression<Func<TQueryModel, bool>> upsertFilter)
        where TQueryModel : class, IQueryModel;

    public Task UpdateAsync<TQueryModel>(Expression<Func<TQueryModel, bool>> filter,
        UpdateDefinition<TQueryModel> update)
        where TQueryModel : class, IQueryModel;

    public Task DeleteAsync<TQueryModel>(Expression<Func<TQueryModel, bool>> deleteFilter)
        where TQueryModel : class, IQueryModel;

}