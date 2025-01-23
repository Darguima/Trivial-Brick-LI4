namespace TrivialBrick.Data.Models;

public record ProductPart
{
    public required int Part_id { get; set; }
    public required int Product_id { get; set; }
    public required int Quantity { get; set; }
}