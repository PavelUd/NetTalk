using Application.Interfaces.Repositories;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories;

internal sealed class EventStoreRepository(NetTalkDbContext context) : IEventStoreRepository
{
    public async Task StoreAsync(IEnumerable<EventStore> eventStores)
    {
        await context.EventStores.AddRangeAsync(eventStores);
        await context.SaveChangesAsync();
    }

    #region IDisposable
    
    private bool _disposed;
    
    ~EventStoreRepository() => Dispose(false);


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    // Protected implementation of Dispose pattern.
    private void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        // Dispose managed state (managed objects).
        if (disposing)
            context.Dispose();

        _disposed = true;
    }

    #endregion
}