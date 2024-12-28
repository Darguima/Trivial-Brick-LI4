namespace TrivialBrick.DataLayer;

public record User
{
    public string? ID { get; set; }
    public string? Name { get; set; }
    public string? Mail { get; set; }
    public string? Password { get; set; }
}