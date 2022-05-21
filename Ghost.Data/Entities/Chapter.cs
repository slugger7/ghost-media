namespace Ghost.Data
{
  public class Chapter
  {
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public Image Image { get; set; }
    public Video Video { get; set; }
    public long Timestamp { get; set; }
  }
}