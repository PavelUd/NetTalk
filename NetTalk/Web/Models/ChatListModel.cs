using Application.Chat.Dto;
using Application.Users.Dto;

namespace NetTalk.Models;

public class ChatListModel
{
   public List<ChatSummary> Chats { get; set; }
   public List<UserDto> OtherUsers { get; set; }

}