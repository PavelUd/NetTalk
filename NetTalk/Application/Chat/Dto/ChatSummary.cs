using AutoMapper;
using Domain.Entities;

namespace Application.Chat.Dto;

public class ChatSummary
{
    public int Id { get; init; }
    public string Name { get; init; }
    public bool IsActive { get; set; }
    public List<User> Users { get; set; }
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Chat, ChatSummary>();
        }
    }
}