
namespace TrivialBrick.UI.InputStructs;

public class ProductData
{
  public required List<InstructionInfo> instructions { get; set; }
  public required List<PartsInfo> parts { get; set; }
}

public class InstructionInfo
{
  public required int qnt_parts { get; set; }
  public required string image { get; set; }
}

public class PartsInfo
{
  public required int id { get; set; }
  public required int qnt { get; set; }
  public required string img { get; set; }
}