namespace Application.Queries.QueryModels;

public class FileQueryModel
{
    public int Id { get; set; }

    public int MessageId { get; set; }
    
    public int OwnerId { get; set; }
    
    public string Url { get; set; }
    
    public long Size { get; set; }
    
    public string Type { get; set; }
    
    public DateTime? UploadedAt { get; set; }
}