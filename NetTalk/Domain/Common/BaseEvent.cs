namespace Domain.Common;
using MediatR;

public class BaseEvent : INotification
{
    /// <summary>
    /// Gets the type of the message.
    /// </summary>
     public Guid Id { get; set; } = Guid.NewGuid();
    public string MessageType { get; protected init; }

    /// <summary>
    /// Gets the aggregate ID.
    /// </summary>
    public Guid AggregateId { get; protected init; }

    /// <summary>
    /// Gets the date and time when the event occurred.
    /// </summary>
    public DateTime OccurredOn { get; private init; } = DateTime.Now.ToUniversalTime();
}