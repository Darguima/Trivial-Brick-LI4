namespace TrivialBrick.Data.Models;

public record Product
{
    public required int Model { get; set; }
    public required string Name { get; set; }
    public required decimal Price { get; set; }
    public required string Description { get; set; }
    public required string Image { get; set; }
}
