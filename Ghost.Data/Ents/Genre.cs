namespace Ghost.Data.Ents
{
  public class Genre
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public List<VideoGenre> VideoGenres { get; set; }
  }
}