namespace TrivialBrick.Authentication;

public record UserSession 
{
    public required string ID { get; set; }
    public required bool IsAdmin { get; set; }
}

