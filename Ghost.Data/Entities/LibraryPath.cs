using LiteDB;

namespace Ghost.Data.Entities
{
  public class LibraryPath
  {
    public ObjectId? _id { get; set; }
    public string? Path { get; set; }
    [BsonRef("videos")]
    public List<Video> Videos { get; set; } = new List<Video>();
  }
}