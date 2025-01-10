using Application.Interfaces.Repositories;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class ChatRepository : BaseRepository<Chat>, IChatRepository
{
    public ChatRepository(NetTalkDbContext context) : base(context)
    {
    }
}