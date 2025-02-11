using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common.Interfaces;

namespace Domain.Common;

public class BaseEntity: IEntity
{
    [Column("id")]
    public int Id { get; set; }
    
    private readonly List<BaseEvent> _domainEvents = [];
    
    [NotMapped]
    public IEnumerable<BaseEvent> DomainEvents =>
        _domainEvents.AsReadOnly();
    
    protected void AddDomainEvent(BaseEvent domainEvent) =>
        _domainEvents.Add(domainEvent);
    
    public void ClearDomainEvents() =>
        _domainEvents.Clear();
    
}
