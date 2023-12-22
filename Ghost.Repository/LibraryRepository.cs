using Ghost.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ghost.Repository;
public class LibraryRepository : ILibraryRepository
{
  private readonly GhostContext context;
  private readonly IVideoRepository videoRepository;
  private readonly ILogger<ILibraryRepository> logger;

  public LibraryRepository(
    GhostContext context,
    IVideoRepository videoRepository,
    ILogger<ILibraryRepository> logger
    )
  {
    this.context = context;
    this.videoRepository = videoRepository;
    this.logger = logger;
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

  public async Task Delete(int id)
  {
    var library = context.Libraries.Include("Paths.Videos").FirstOrDefault(l => l.Id == id);

    if (library == null) throw new NullReferenceException("Library not found");

    foreach (var path in library.Paths)
    {
      foreach (var video in path.Videos)
      {
        await videoRepository.Delete(video.Id);
      }
      context.LibraryPaths.Remove(path);
    }
    context.Libraries.Remove(library);
    await context.SaveChangesAsync();
  }

  public async Task<Library?> FindById(int id)
  {
    return await context.Libraries
        .Include("Paths.Videos.VideoImages")
        .FirstOrDefaultAsync(l => l.Id == id);
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

  public async Task<LibraryPath?> GetLibraryPathById(int id)
  {
    return await context.LibraryPaths.FirstOrDefaultAsync(l => l.Id == id);
  }

  public async Task<IEnumerable<Video>> GetVideos(int id)
  {
    var library = await FindById(id);
    if (library == null) throw new NullReferenceException("Library not found");

    logger.LogDebug("Library has {0} paths", library.Paths.Count());

    var videos = new List<Video>();
    foreach (var path in library.Paths)
    {
      logger.LogDebug("Path has {0} videos", path.Videos.Count());
      videos = videos.Concat(path.Videos).ToList();
    }

    return videos;
  }
}