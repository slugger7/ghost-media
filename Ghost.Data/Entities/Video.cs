using LiteDB;

namespace Ghost.Data.Entities
{
  public class Video
  {
    public ObjectId? _id { get; set; }
    public string? FileName { get; set; }
    public string? Title { get; set; }
    public string? Path { get; set; }
    [BsonRef("genres")]
    public List<Genre> Genres { get; set; } = new List<Genre>();
    [BsonRef("actors")]
    public List<Actor> Actors { get; set; } = new List<Actor>();
  }
}