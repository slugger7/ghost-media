namespace Ghost.Data;
public class VideoImage
{
  public int Id { get; set; }
  public string Type { get; set; } = string.Empty;
  public virtual Video Video { get; set; } = null!;
  public virtual Image Image { get; set; } = null!;
}