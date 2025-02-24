using Application.Queries.QueryModels;
using AutoMapper;
using Domain.Entities;

namespace Application.Chat.Dto;

public class ChatSummary
{
    public Guid Id { get; init; }
    public string Name { get; set; }
    public string Url{ get; set; }
    public bool IsActive { get; set; }
    public string Type { get; init; }
    public Guid Owner { get; init; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Chat, ChatSummary>();
            CreateMap<ChatQueryModel, ChatSummary>();
        }
    }
}