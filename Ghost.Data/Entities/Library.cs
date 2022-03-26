using LiteDB;

namespace Ghost.Data.Entities
{
  public class Library
  {
    public ObjectId? _id { get; set; }
    public string? Name { get; set; }
    public List<LibraryFolder>? Folders {get;set;}
  }
}