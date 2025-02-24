using Application.Queries.QueryModels;
using MongoDB.Bson.Serialization;

namespace Persistence.Mappings;

public class UserMap
{
    public void Configure()
    {
        BsonClassMap.TryRegisterClassMap<UserQueryModel>(classMap =>
        {
            classMap.AutoMap();
            classMap.SetIgnoreExtraElements(true);

            classMap.MapMember(chat => chat.Id)
                .SetIsRequired(true);

            classMap.MapMember(chat => chat.Avatar)
                .SetIsRequired(false);

            classMap.MapMember(chat => chat.Email)
                .SetIsRequired(true);

            classMap.MapMember(chat => chat.Username)
                .SetIsRequired(true);
            
            classMap.MapMember(chat => chat.PinnedChats)
                .SetIsRequired(true);
        });
    }
}