using Application.Interfaces.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly NetTalkDbContext _context;
    private ChatRepository? _chatRepository;
    private UserRepository? _userRepository;

    public UnitOfWork(NetTalkDbContext context)
    {
        _context = context;
    }

    public IChatRepository ChatRepository
    {
        get { return _chatRepository ??= new ChatRepository(_context); }
    }

    public IUserRepository UserRepository
    {
         get {return _userRepository ??= new UserRepository(_context); }
         
    }

    public void Commit()
    {
        _context.SaveChanges();
    }

    public void Rollback()
    {
        _context.RollbackTransaction();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}