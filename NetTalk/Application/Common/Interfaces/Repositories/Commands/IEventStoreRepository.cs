using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IEventStoreRepository
{
    public Task StoreAsync(IEnumerable<EventStore> eventStores);
}