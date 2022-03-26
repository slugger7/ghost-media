using LiteDB;

namespace Ghost.Data.Entities
{
  public class Library
  {
    public ObjectId? _id { get; set; }
    public string? Name { get; set; }
    [BsonRef("folders")]
    public List<LibraryFolder> Folders {get;set;} = new List<LibraryFolder>();
  }
}