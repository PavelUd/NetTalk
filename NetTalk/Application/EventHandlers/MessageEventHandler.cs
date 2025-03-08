using Application.Common.Interfaces.Repositories;
using Application.Queries.QueryModels;
using AutoMapper;
using Domain.Events.Chat;
using Domain.Events.Message;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace Application.EventHandlers;

public class MessageEventHandler(
    ILogger<ChatEventHandler> logger, 
    IMapper mapper, 
    ISynchronizeDb synchronizeDb) : INotificationHandler<MessageCreatedEvent>
{

    public async Task Handle(MessageCreatedEvent notification, CancellationToken cancellationToken)
    {
        LogEvent(notification);
        var messageQueryModel = mapper.Map<MessageQueryModel>(notification);
        await synchronizeDb.UpsertAsync(messageQueryModel, filter => filter.Id == messageQueryModel.Id);
        
    }
    
    private void LogEvent<TEvent>(TEvent @event) where TEvent : MessageBaseEvent =>
        logger.LogInformation("----- Triggering the event {EventName}, model: {EventModel}", typeof(TEvent).Name, @event.ToJson());
}