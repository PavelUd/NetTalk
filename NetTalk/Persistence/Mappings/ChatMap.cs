using Application.Queries.QueryModels;
using MongoDB.Bson.Serialization;

namespace Persistence.Mappings;

public class ChatMap
{
    public void Configure()
    {
        BsonClassMap.TryRegisterClassMap<ChatQueryModel>(classMap =>
        {
            classMap.AutoMap();
            classMap.SetIgnoreExtraElements(true);

            classMap.MapMember(chat => chat.Id)
                .SetIsRequired(true);

            classMap.MapMember(chat => chat.Url)
                .SetIsRequired(false);

            classMap.MapMember(chat => chat.Owner)
                .SetIsRequired(true);

            classMap.MapMember(chat => chat.IsActive)
                .SetIsRequired(true);
        });
    }
}