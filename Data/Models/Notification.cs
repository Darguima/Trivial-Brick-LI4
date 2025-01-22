namespace TrivialBrick.Data.Models;

public record Notification
{
    public required int Notification_id { get; set; }
    public required string Message { get; set; }
    public required DateTime Datetime { get; set; }
    public required int Client_id { get; set; }
    public required int Order_id { get; set; }
}