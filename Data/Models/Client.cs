namespace TrivialBrick.Data.Models;

public record Client : User
{
    public required int NIF { get; set; }
}