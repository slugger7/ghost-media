using Ghost.Data.Entities;
using Ghost.Dtos;
using Ghost.Services.Interfaces;
using LiteDB;

namespace Ghost.Services
{
  public class LibraryService : ILibraryService
  {
    private readonly IDirectoryService directoryService;
    private readonly IVideoService videoService;
    private string connectionString = @"..\Ghost.Data\Ghost.db";
    private static string collectionName = "libraries";

    public LibraryService(
      IDirectoryService directoryService,
      IVideoService videoService
    )
    {
      this.directoryService = directoryService;
      this.videoService = videoService;
    }

    internal static ILiteCollection<Library> GetCollection(LiteDatabase db)
    {
      var col = db.GetCollection<Library>(collectionName);
      col.EnsureIndex(x => x.Name);

      return col;
    }

    public LibraryDto AddDirectory(string id, AddPathsToLibraryDto pathsToLibraryDto)
    {
      if (pathsToLibraryDto.Paths == null) throw new NullReferenceException("Paths were null");
      using (var db = new LiteDatabase(connectionString))
      {
        var col = GetCollection(db);

        var library = col.FindById(new ObjectId(id));
        if (library == null) throw new NullReferenceException("No library found");

        var folderCollection = DirectoryService.GetCollection(db);

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

    public LibraryDto Create(string libraryName)
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

    public PageResultDto<LibraryDto> GetMany(int page, int limit)
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

    public void Sync(string id)
    {
      var library = this.Get(id);

      foreach (var path in library.Paths)
      {
        if (path.Path == null) continue;

        var directories = directoryService.GetDirectories(path.Path);
        var files = directoryService.GetFilesOfTypeInDirectory(path.Path, "mp4");

        var dirIndex = 0;

        while (directories.Count() > dirIndex)
        {
          var currentDirectory = directories.ElementAt(dirIndex++);
          files = files.Concat(directoryService.GetFilesOfTypeInDirectory(currentDirectory, "mp4")).ToList();
        }

        var videoEntities = files.Select(v =>
        {
          var videoSplit = v.Split(Path.DirectorySeparatorChar);
          var fileName = videoSplit[videoSplit.Length - 1];
          var initialTitle = fileName.Substring(0, fileName.LastIndexOf('.'));
          return new Video
          {
            Path = v,
            FileName = fileName,
            Title = initialTitle
          };
        })
        .ToList();


        using (var db = new LiteDatabase(connectionString))
        {
          var col = GetCollection(db);
          var videoCollection = VideoService.GetCollection(db);

          videoCollection.InsertBulk(videoEntities);

          library.Videos = videoEntities;

          col.Update(library);
        }
      }
    }

    internal Library Get(string id)
    {
      using (var db = new LiteDatabase(connectionString))
      {
        var col = GetCollection(db);

        var library = col
          .Include(l => l.Paths)
          .FindById(new ObjectId(id));

        if (library == null) throw new NullReferenceException("Library not found");

        return library;
      }
    }

    public LibraryDto GetDto(string id) => new LibraryDto(this.Get(id));

    public void Delete(string id)
    {
      Library library;
      using (var db = new LiteDatabase(connectionString))
      {
        var col = GetCollection(db);

        library = col
          .Include(l => l.Videos)
          .Include(l => l.Paths)
          .FindById(new ObjectId(id));

        if (library == null) throw new NullReferenceException("Library not found");

        col.Delete(library._id);
      }

      if (library.Paths == null) throw new NullReferenceException("Paths for library was null");
      if (library.Videos == null) throw new NullReferenceException("Videoas were null");

      DirectoryService.DeleteRange(library.Paths.Select(p => p._id));
      VideoService.DeleteRange(library.Videos.Select(v => v._id));
    }
  }
}