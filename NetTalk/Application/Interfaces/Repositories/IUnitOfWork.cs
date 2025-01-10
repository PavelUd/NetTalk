namespace Application.Interfaces.Repositories;

public interface IUnitOfWork
{
    public IChatRepository ChatRepository { get; }
    public IUserRepository UserRepository{ get; }
    public void Commit();
    public void Rollback();
    public void Dispose();

}