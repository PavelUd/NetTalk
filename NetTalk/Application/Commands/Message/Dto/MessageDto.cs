using Application.Interfaces;
using Domain.Entities;

namespace Application.Chat.Dto;

public class MessageDto
{
    public Guid  Id{ get; set; }
    public Guid  IdChat { get; set; }
    
    public string Text { get; set; }    
    
    public IUser Sender { get; set; }
    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
    

    public MessageDto(Message message, string text, IUser user)
    {
        Id = message.Id;
        IdChat = message.IdChat;
        Sender = user;
        Text = text;
        CreatedDate = message.CreatedDate;
        UpdatedDate = message.UpdatedDate;
    }
}