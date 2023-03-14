using ChatApp.Enums;

namespace ChatApp.Models;

public class Chat
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Message> Messages { get; set; }
    public List<ChatUser> Users { get; set; }
    public ChatType Type { get; set; }
}