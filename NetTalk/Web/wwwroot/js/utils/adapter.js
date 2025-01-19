export const adaptToClient = (message) => {
    const adaptedPoint = {
        id: message.Id,
        idChat: message.IdChat,
        text: message.Text,
        user: {
            idUser: message.User.IdUser,
            url: message.User.Url,
            name: message.User.Name,
        },
        createdDate: message.CreatedDate,
        UpdatedDate: message.UpdatedDate
    }
    return adaptedPoint
}

export const adaptUserToChatSummary = (user) =>{
    return {
        url: user.avatarUrl,
        name: user.fullName,
        userId: user.id,
        id: user.idChat
    }
}