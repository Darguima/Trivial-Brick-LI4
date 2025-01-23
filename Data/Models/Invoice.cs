namespace TrivialBrick.Data.Models;

public record Invoice
{
    public required int Invoice_id { get; set; }
    public required DateTime Datetime { get; set; }
    public required int Client_id { get; set; }
    public required int Order_id { get; set; }
}