namespace Ghost.Data
{
  public class Image
  {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public virtual List<VideoImage> VideoImages { get; set; } = new List<VideoImage>();
  }
}