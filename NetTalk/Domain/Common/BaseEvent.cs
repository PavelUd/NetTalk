namespace Domain.Common;
using MediatR;

public class BaseEvent : INotification
{
    /// <summary>
    /// Gets the type of the message.
    /// </summary>
     public int Id { get; set; }
    public string MessageType { get; protected init; }

    /// <summary>
    /// Gets the aggregate ID.
    /// </summary>
    public int AggregateId { get; protected init; }

    /// <summary>
    /// Gets the date and time when the event occurred.
    /// </summary>
    public DateTime OccurredOn { get; private init; } = DateTime.Now.ToUniversalTime();
}