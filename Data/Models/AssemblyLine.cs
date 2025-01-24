namespace TrivialBrick.Data.Models;

public record AssemblyLine
{
    public required string Assembly_line_id { get; set; }
    public required AssemblyLineState State { get; set; }
    public int? Order_id { get; set; }
    public  DateTime? Mount_start_time {get; set;}
    public  DateTime? Expected_end_time {get; set;}
}

public enum AssemblyLineState
{
    Active,
    Inactive
}
