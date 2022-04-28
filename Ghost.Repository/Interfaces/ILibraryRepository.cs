using Ghost.Data;

namespace Ghost.Repository
{
  public interface ILibraryRepository
  {
    Library? FindById(int id);
    Library Create(Library library);
    Library AddPaths(int id, IEnumerable<LibraryPath> paths);
    PageResult<Library> GetLibraries(int page = 0, int limit = 10);
    LibraryPath AddVideosToPath(int pathId, IEnumerable<Video> videos);
  }
}