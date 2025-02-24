using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities;

[Table("events")]
public class EventStore : BaseEvent
{
    public EventStore(Guid  aggregateId, string messageType, string data)
    {
        AggregateId = aggregateId;
        MessageType = messageType;
        Data = data;
    }
    
    public EventStore()
    {
    }
    
    public string Data { get; private init; }
}