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

    private ILiteCollection<Library> GetCollection(LiteDatabase db)
    {
      var col = db.GetCollection<Library>(collectionName);
      col.EnsureIndex(x => x.Name);

      return col;
    }

    public LibraryDto AddDirectoryToLibrary(string id, AddPathsToLibraryDto pathsToLibraryDto)
    {
      if (pathsToLibraryDto.Paths == null) throw new NullReferenceException("Paths were null");
      using (var db = new LiteDatabase(connectionString))
      {
        var col = GetCollection(db);

        var library = col.FindById(new ObjectId(id));
        if (library == null) throw new NullReferenceException("No library found");

        var folderCollection = db.GetCollection<LibraryPath>("paths");

        foreach (var path in pathsToLibraryDto.Paths)
        {
          var folder = new LibraryPath
          {
            Path = path
          };

          folderCollection.Insert(folder);
          library.Paths.Add(folder);
        }

        col.Update(library);

        return new LibraryDto(library);
      }
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

    public PageResultDto<LibraryDto> GetLibraries(int page, int limit)
    {
      using (var db = new LiteDatabase(connectionString))
      {
        var col = GetCollection(db);

        var libraries = col.Include(l => l.Paths)
          .Query()
          .Limit(limit)
          .Skip(limit * page)
          .ToEnumerable()
          .Select(l => new LibraryDto(l))
          .ToList();
        return new PageResultDto<LibraryDto>
        {
          Total = col.Count(),
          Page = page,
          Content = libraries
        };
      }
    }
  }
}