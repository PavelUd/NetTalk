namespace Application.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{

    public Task SaveChangesAsync();

}