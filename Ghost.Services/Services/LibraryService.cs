using Ghost.Data.Entities;
using Ghost.Dtos;
using Ghost.Services.Interfaces;
using LiteDB;

namespace Ghost.Services
{
  public class LibraryService : ILibraryService
  {
    private string connectionString = @"..\Ghost.Data\Ghost.db";
    private string collectionName = "libraries";

    public ILiteCollection<Library> GetCollection(LiteDatabase db)
    {
      var col = db.GetCollection<Library>(collectionName);
      col.EnsureIndex(x => x.Name);

      return col;
    }

    public void AddDirectoryToLibrary(string _id, string directory)
    {
      throw new NotImplementedException();
    }

    public LibraryDto CreateLibrary(string libraryName)
    {
      using (var db = new LiteDatabase(connectionString))
      {
        var col = GetCollection(db);

        var library = new Library
        {
          Name = libraryName
        };
        col.Insert(library);

        return new LibraryDto(library);
      }
    }

    public PageResultDto<LibraryDto> GetLibraries()
    {
      using (var db = new LiteDatabase(connectionString))
      {
        var col = GetCollection(db);

        var libraries = col.Query()
          .ToEnumerable()
          .Select(l => new LibraryDto(l))
          .ToList();
        return new PageResultDto<LibraryDto>
        {
          Content = libraries
        };
      }
    }
  }
}