using LiteDB;

namespace Ghost.Data.Entities
{
  public class Video
  {
    public ObjectId? _id { get; set; }
    public string? FileName { get; set; }
    public string? Title { get; set; }
    public string? Path { get; set; }
    public List<Genre> Genres { get; set; } = new List<Genre>();
  }
}