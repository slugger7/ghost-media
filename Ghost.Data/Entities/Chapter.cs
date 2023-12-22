namespace Ghost.Data;
public class Chapter
{
  public int Id { get; set; }
  public string Description { get; set; } = string.Empty;
  public Image Image { get; set; } = null!;
  public Video Video { get; set; } = null!;
  public long Timestamp { get; set; }
}