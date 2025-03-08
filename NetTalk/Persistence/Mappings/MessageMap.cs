using Application.Queries.QueryModels;
using MongoDB.Bson.Serialization;

namespace Persistence.Mappings;

public class MessageMap
{
    public void Configure()
    {
        BsonClassMap.TryRegisterClassMap<MessageQueryModel>(classMap =>
        {
            classMap.AutoMap();
            classMap.SetIgnoreExtraElements(true);

            classMap.MapMember(chat => chat.Id)
                .SetIsRequired(true);

            classMap.MapMember(chat => chat.IdChat)
                .SetIsRequired(true);

            classMap.MapMember(chat => chat.IdUser)
                .SetIsRequired(true);

            classMap.MapMember(chat => chat.IsPinned)
                .SetIsRequired(true);
            
            classMap.MapMember(chat => chat.CreatedDate)
                .SetIsRequired(true);
            classMap.MapMember(chat => chat.UpdatedDate)
                .SetIsRequired(true);
        });
    }
}