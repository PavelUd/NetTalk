using Application.Queries.QueryModels;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Persistence.Mappings;

public class ChatMap
{
    public void Configure()
    {
        BsonClassMap.TryRegisterClassMap<ChatQueryModel>(classMap =>
        {
            classMap.AutoMap();
            classMap.SetIgnoreExtraElements(true);

            classMap.MapMember(customer => customer.Id)
                .SetIsRequired(true);

            classMap.MapMember(customer => customer.Url)
                .SetIsRequired(true);

            classMap.MapMember(customer => customer.Owner)
                .SetIsRequired(true);

            classMap.MapMember(customer => customer.IsActive)
                .SetIsRequired(true);
        });
    }
}