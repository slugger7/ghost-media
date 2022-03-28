using LiteDB;

namespace Ghost.Data.Entities
{
  public class Library
  {
    public ObjectId? _id { get; set; }
    public string? Name { get; set; }
    [BsonRef("paths")]
    public List<LibraryPath> Paths {get;set;} = new List<LibraryPath>();
  }
}