using Application.Commands.Chat.Dto;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Events.Chat;
using Domain.Events.Message;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Queries.QueryModels;

public class MessageQueryModel : IQueryModel
{
    [BsonId]
    [BsonIgnoreIfDefault]
    public ObjectId ObjectId { get; init; }
    
    public Guid Id { get; set; }
    
    public Guid IdChat { get; set; }
    
    public byte[] EncryptText { get; set; }

    public string Text { get; set; }
    
    public Guid IdUser { get; set; }

    public bool IsPinned { get; set; }
    
    public DateTime? UpdatedDate { get; set; }
    
    public DateTime? CreatedDate { get; set; }
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            
            CreateMap<MessageCreatedEvent, MessageQueryModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AggregateId))
                .ForMember(dest => dest.EncryptText, opt => opt.MapFrom(src => src.Text)).ReverseMap();
        }
    }
}