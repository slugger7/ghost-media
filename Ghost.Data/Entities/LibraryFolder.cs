using LiteDB;

namespace Ghost.Data.Entities
{
  public class LibraryFolder
  {
    public ObjectId? _id {get;set;}
    public string? Path {get;set;}
  }
}