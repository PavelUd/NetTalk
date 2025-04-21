namespace Application.Common.Interfaces;

public interface ICacheService
{
    public TItem? Get<TItem>(Guid key);
    public void Set<TItem>(Guid key, in TItem value);
}