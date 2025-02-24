using Application.Common.Interfaces;
using AutoMapper;
using Domain.Events.Chat;
using Domain.Events.User;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Queries.QueryModels;

public class UserQueryModel : IQueryModel
{
    
    [BsonId]
    [BsonIgnoreIfDefault]
    public ObjectId InternalId { get; init; }
    public string Username { get; init; }
    
    public string Email { get; init; }
    
    public string Avatar { get; init; }
    
    public List<int> PinnedChats { get; init; }
    
    public Guid  Id { get; init; }
    
    public UserQueryModel()
    {
        
    }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UserQueryModel, Domain.Entities.User>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Username));
            
            CreateMap<UserCreatedEvent, UserQueryModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AggregateId));
        }
    }
}