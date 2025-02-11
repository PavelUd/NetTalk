using Domain.Entities;

namespace Application.Chat.Dto;

public class MessageDto
{
    public int Id{ get; set; }
    public int IdChat { get; set; }
    
    public string Text { get; set; }    
    
    public MessageUserDto User { get; set; }
    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
    
    public MessageDto(){}

    public MessageDto(Message message, string text, User user)
    {
        Id = message.Id;
        IdChat = message.IdChat;
        User = new MessageUserDto()
        {
            IdUser = user.Id,
            Name = user.FullName,
            Url = user.AvatarUrl
        };
        Text = text;
        CreatedDate = message.CreatedDate;
        UpdatedDate = message.UpdatedDate;
    }
}