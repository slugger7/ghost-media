namespace Ghost.Dtos;
public class VideoMetaDataDto
{
  public TimeSpan Duration { get; set; }
  public string Format { get; set; } = String.Empty;
  public string FormatLong { get; set; } = String.Empty;
  public int Width { get; set; }
  public int Height { get; set; }
  public long Size { get; set; }
  public DateTime Created { get; set; }
}