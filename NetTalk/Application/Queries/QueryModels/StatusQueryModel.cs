using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Queries.QueryModels;

public class StatusQueryModel
{
    [BsonId]
    public ObjectId ObjectId { get; init; }
    
    public int Id { get; set; }
    
    public int MessageId { get; set; }
    
    public int OwnerId { get; set; }
    
    public string Status { get; set; }
}