namespace TrivialBrick.Data.Models;

public record Order
{
    public required int Order_id { get; set; }
    public required string Address { get; set; }
    public required OrderState State { get; set; }
    public int Product_id { get; set; }
    public int Client_id { get; set; }
    public decimal Price { get; set; }
    public DateTime Date { get; set; }
}

public enum OrderState
{
    Wait_line,
    Assembly_line,
    Finished
}