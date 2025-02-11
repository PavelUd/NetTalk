using Application.Chat.Dto;
using Application.Commands.Chat.Dto;

namespace NetTalk.Models;

public class ChatViewModel
{
    public ChatDto Chat { get; set; }
    public ChatListModel AllChats { get; set; }
}