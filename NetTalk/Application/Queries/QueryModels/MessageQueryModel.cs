using Application.Common.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Queries.QueryModels;

public class MessageQueryModel : IQueryModel
{
    [BsonId]
    public ObjectId ObjectId { get; init; }
    
    public Guid Id { get; set; }
    
    public Guid ChatId { get; set; }

    public Guid UserId { get; set; }

    public bool IsPinned { get; set; }
    
    public DateTime? UpdatedDate { get; set; }
    
    public DateTime? CreatedDate { get; set; }
}