namespace ChatApp.Models;

public class Message
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public DateTime TimeStamp { get; set; } = DateTime.Now;
    public int ChatId { get; set; }
    public Chat Chat { get; set; }
}