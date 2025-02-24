using Application.Chat.Dto;
using Application.Queries.QueryModels;
using AutoMapper;
using Domain.Enums;

namespace Application.Commands.Chat.Dto;

public class ChatDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Url { get; init; }
    public bool IsActive { get; set; }
    public Guid Owner { get; set; }
    public string Type { get; set; }
    public List<MessageQueryModel> Messages { get; set; }
    public List<UserQueryModel> Users { get; set; }
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Chat, ChatDto>();
        }
    }
}