namespace TrivialBrick.Data.Models;

public record Part
{
    public required int Part_id { get; set; }
    public string? Image { get; set; }
    public required int Stock { get; set; }
}