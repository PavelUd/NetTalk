namespace Application.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    public IChatRepository ChatRepository { get; }
    public IUserRepository UserRepository{ get; }
    public void Commit();

    public Task SaveChangesAsync();
    public void Rollback();

}