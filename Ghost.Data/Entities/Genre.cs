using LiteDB;

namespace Ghost.Data.Entities
{
  public class Genre
  {
    public ObjectId _id { get; set; } = new ObjectId();
    public string Name { get; set; } = "";
  }
}