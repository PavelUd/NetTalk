using Application.Commands.Chat.Dto;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Events.Chat;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Queries.QueryModels;

public class ChatQueryModel : IQueryModel
{
    public ChatQueryModel(string name, string url, int id, bool isActive, List<string> messages, List<string> users, int owner)
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
    public bool IsActive { get; set;}
    public int Owner { get; set; }
    public int Id { get; set; }
    public List<string> Messages { get; set; }
    
    public List<string> Users { get; set; }
    [BsonId]
    public ObjectId ObjectId { get; set; }
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Chat, ChatQueryModel>()
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.Messages.Select(us => us.Id)))
                .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users.Select(us => us.Id)));
            CreateMap<ChatCreatedEvent, ChatQueryModel>();
        }
    }
    
}