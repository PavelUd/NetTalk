using Application.Common.Interfaces.Repositories;
using Application.Queries.QueryModels;
using AutoMapper;
using Domain.Entities;
using Domain.Events.Chat;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.EventHandlers;

public class ChatEventHandler(
    ILogger<ChatEventHandler> logger, 
    IMapper mapper, 
    ISynchronizeDb synchronizeDb)
    : INotificationHandler<ChatCreatedEvent>
    ,INotificationHandler<ChatLastMessageUpdatedEvent>
{
    public async Task Handle(ChatCreatedEvent notification, CancellationToken cancellationToken)
    {
        
        LogEvent(notification);
        var customerQueryModel = mapper.Map<ChatQueryModel>(notification);
        await synchronizeDb.UpsertAsync(customerQueryModel, filter => filter.Id == customerQueryModel.Id);
    }
    
    public async Task Handle(ChatLastMessageUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var message = mapper.Map<MessageQueryModel>(notification.LastMessage);
        var update = Builders<ChatQueryModel>.Update.Set<MessageQueryModel>(user => user.LastMessage, message);
        await synchronizeDb.UpdateAsync(filter => filter.Id == notification.IdChat, update);
    }
    
    private void LogEvent<TEvent>(TEvent @event) where TEvent : ChatBaseEvent =>
        logger.LogInformation("----- Triggering the event {EventName}, model: {EventModel}", typeof(TEvent).Name, @event.ToJson());
    
}