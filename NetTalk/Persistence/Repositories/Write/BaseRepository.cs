using System.Linq.Expressions;
using Application.Interfaces.Repositories;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly NetTalkDbContext Context;
    protected IQueryable<T> Entities => Context.Set<T>();
    
    public BaseRepository(NetTalkDbContext context)
    {
        Context = context;
    }
    

    public async Task<T> AddAsync(T entity)
    {
        await Context.Set<T>().AddAsync(entity);
        return entity;
    }

    public void Attach(T entity)
    {
        Context.Set<T>().Attach(entity);
    }
    
    public Task UpdateAsync(T entity)
    {
        var exist = Context.Set<T>().Find(entity.Id);
        Context.Entry(exist).CurrentValues.SetValues(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity)
    {
        Context.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }
    
    public IQueryable<T> FindAll()
    {
        return  Context.Set<T>();
    }
    
    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
    {
        return  Context.Set<T>().Where(expression);
    }
}