using Application.Commands.Chat.Dto;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Events.Chat;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Queries.QueryModels;

public class ChatQueryModel : IQueryModel
{
    public ChatQueryModel(string name, string url, Guid  id, bool isActive, List<string> users, Guid  owner)
    {
        Name = name;
        Url = url;
        Id = id;
        IsActive = isActive;
        Owner = owner;
    }

    public ChatQueryModel()
    {
        
    }
    public string Name { get; set; }
    public string Url{ get;set; }
    public bool IsActive { get; init;}
    public Guid Owner { get; init; }
    public Guid Id { get; init; }
    
    public string Type { get; init; }

    public List<Guid> Participants { get; init; }
    
    [BsonId]
    [BsonIgnoreIfDefault]
    public ObjectId ObjectId { get; init; }
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Chat, ChatQueryModel>()
                .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.Users.Select(us => us.Id)));
            CreateMap<ChatCreatedEvent, ChatQueryModel>();
            CreateMap<ChatQueryModel, ChatDto>();
        }
    }
    
}