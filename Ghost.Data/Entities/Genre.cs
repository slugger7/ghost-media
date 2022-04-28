namespace Ghost.Data
{
  public class Genre
  {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<VideoGenre> VideoGenres { get; set; } = new List<VideoGenre>();
  }
}