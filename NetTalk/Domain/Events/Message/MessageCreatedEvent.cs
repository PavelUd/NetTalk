namespace Domain.Events.Message;


public class MessageCreatedEvent(Guid id, Guid idChat, byte[] text, Guid idUser, DateTime? updatedDate,
    DateTime? createdDate)
    : MessageBaseEvent(id, idChat, text, idUser)
{
    public DateTime? UpdatedDate { get; set; } = updatedDate;

    public DateTime? CreatedDate { get; set; } = createdDate;
    
    public bool IsPinned { get; set; } = false;
}