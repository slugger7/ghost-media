using LiteDB;

namespace Ghost.Data.Entities
{
  public class Genre
  {
    public ObjectId _id { get; set; } = new ObjectId();
    public string Name { get; set; } = "";
    // [BsonRef("videos")]
    // public List<Video> Videos { get; set; } = new List<Video>();
  }
}