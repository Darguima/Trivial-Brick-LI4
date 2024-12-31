namespace TrivialBrick.DataLayer.Models;

public record Client : User
{
    public int NIF { get; set; } = 0;
}