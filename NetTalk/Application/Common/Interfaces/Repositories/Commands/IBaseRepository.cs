using System.Linq.Expressions;
using Domain.Common;

namespace Application.Interfaces.Repositories;

public interface IBaseRepository<T>  where T : BaseEntity
{
    public Task<T> AddAsync(T entity);
    public Task UpdateAsync(T entity);
    public void Attach(T entity);
    
    public Task DeleteAsync(T entity);
    public IQueryable<T> FindAll();
    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
}