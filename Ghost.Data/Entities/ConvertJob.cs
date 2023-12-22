namespace Ghost.Data;
public class ConvertJob
{
  public int Id { get; set; }
  public string Title { get; set; } = string.Empty;
  public string Path { get; set; } = string.Empty;
  public int? ConstantRateFactor { get; set; }
  public int? VariableBitrate { get; set; }
  public string? ForcePixelFormat { get; set; }
  public int Height { get; set; }
  public int Width { get; set; }
  public Video Video { get; set; } = null!;
  public Job Job { get; set; } = null!;
}
