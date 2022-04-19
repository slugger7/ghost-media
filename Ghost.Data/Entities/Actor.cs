using LiteDB;

namespace Ghost.Data.Entities
{
  public class Actor
  {
    public ObjectId _id { get; set; } = new ObjectId();
    public string Name { get; set; } = string.Empty;
  }
}