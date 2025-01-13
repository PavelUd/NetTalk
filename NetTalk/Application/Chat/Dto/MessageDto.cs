using Domain.Entities;

namespace Application.Chat.Dto;

public class MessageDto
{
    public int Id{ get; set; }
    public int IdChat { get; set; }
    
    public string Text { get; set; }    
    
    public int IdUser { get; set; }
    
    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
    
    public MessageDto(){}

    public MessageDto(Message message, string text)
    {
        Id = message.Id;
        IdChat = message.IdChat;
        IdUser = message.IdUser;
        Text = text;
        CreatedDate = message.CreatedDate;
        UpdatedDate = message.UpdatedDate;
    }
}