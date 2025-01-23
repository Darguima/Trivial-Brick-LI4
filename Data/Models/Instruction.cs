namespace TrivialBrick.Data.Models;

public record Instruction
{
    public required int Seq_num { get; set; }
    public required int Product_id { get; set; }
    public required string Image { get; set; }
    public required int Qnt_parts { get; set; }
}