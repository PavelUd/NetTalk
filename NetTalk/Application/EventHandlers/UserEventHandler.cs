using Application.Common.Interfaces.Repositories;
using Application.Queries.QueryModels;
using AutoMapper;
using Domain.Events.User;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace Application.EventHandlers;

public class UserEventHandler(ILogger<UserCreatedEvent> logger, IMapper mapper, ISynchronizeDb synchronizeDb) : INotificationHandler<UserCreatedEvent>
{
    public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        LogEvent(notification);
        var queryModel = mapper.Map<UserQueryModel>(notification);
        await synchronizeDb.UpsertAsync(queryModel, filter => filter.Id == queryModel.Id);
    }
    
    private void LogEvent<TEvent>(TEvent @event) where TEvent : UserBaseEvent =>
        logger.LogInformation("----- Triggering the event {EventName}, model: {EventModel}", typeof(TEvent).Name, @event.ToJson());
}