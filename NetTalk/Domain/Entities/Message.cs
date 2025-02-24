using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Events.Message;

namespace Domain.Entities;

[Table("messages")]
public class Message : BaseAuditableEntity
{
    private bool _isDeleted;
    
    public Message()
    {
        
    }

    public Message(Guid idChat, Guid  idUser, byte[] text)
    {
        IdChat = idChat;
        IdUser = idUser;
        Text = text;
        StatusList = new List<MessageStatus>();
        Files = new List<File>();
        
        AddDomainEvent(new MessageCreatedEvent(Id, idChat,text, idUser));
    }
    
    [Column("id_chat")]
    public Guid   IdChat { get; set; }
    
    [Column("message")]
    public byte[] Text { get; set; }    
    
    [Column("id_user")]
    public Guid  IdUser { get; set; }
    public List<File> Files { get; set; }
    public List<MessageStatus> StatusList { get; set; }
    
    public void MarkAsDeleted()
    {
        if (_isDeleted) return;

        _isDeleted = true;
 //       AddDomainEvent(new MessageDeletedEvent(Id, Id, IdChat,Text, IdUser));
    }
}
