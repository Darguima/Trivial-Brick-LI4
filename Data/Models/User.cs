namespace TrivialBrick.Data.Models;

public record User
{
    public required string ID { get; set; }
    public required string Name { get; set; }
    public required string Mail { get; set; }
    public required string Password { get; set; }
}
