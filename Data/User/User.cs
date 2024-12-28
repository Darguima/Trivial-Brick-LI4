namespace TrivialBrick.DataLayer;

public record UserModel
{
    public string? ID { get; set; }
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}