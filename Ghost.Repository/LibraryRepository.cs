using Ghost.Data;
using Microsoft.EntityFrameworkCore;

namespace Ghost.Repository
{
  public class LibraryRepository : ILibraryRepository
  {
    private readonly GhostContext context;

    public LibraryRepository(GhostContext context)
    {
      this.context = context;
    }
    public Library AddPaths(int id, IEnumerable<LibraryPath> paths)
    {
      var library = context.Libraries.Include(l => l.Paths).FirstOrDefault(l => l.Id == id);
      if (library == null) throw new NullReferenceException("Library not found");

      context.LibraryPaths.AddRange(paths);
      library.Paths.AddRange(paths);

      context.SaveChanges();

      return library;
    }

    public LibraryPath AddVideosToPath(int pathId, IEnumerable<Video> videos)
    {
      var path = context.LibraryPaths.Find(pathId);
      if (path == null) throw new NullReferenceException("Path not found");

      context.Videos.AddRange(videos);
      path.Videos.AddRange(videos);

      context.SaveChanges();
      return path;
    }

    public Library Create(Library library)
    {
      context.Libraries.Add(library);
      context.SaveChanges();

      return library;
    }

    public Library? FindById(int id)
    {
      return context.Libraries.Include(l => l.Paths).FirstOrDefault(l => l.Id == id);
    }

    public PageResult<Library> GetLibraries(int page = 0, int limit = 10)
    {
      return new PageResult<Library>
      {
        Total = context.Libraries.Count(),
        Page = page,
        Content = context.Libraries
          .Include(l => l.Paths)
          .OrderBy(l => l.Name)
          .Skip(limit * page)
          .Take(limit)
      };
    }
  }
}