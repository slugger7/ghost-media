using Ghost.Data;

namespace Ghost.Repository;
public interface ILibraryRepository
{
  Task<Library?> FindById(int id);
  Library Create(Library library);
  Library AddPaths(int id, IEnumerable<LibraryPath> paths);
  PageResult<Library> GetLibraries(int page = 0, int limit = 10);
  LibraryPath AddVideosToPath(int pathId, IEnumerable<Video> videos);
  Task<IEnumerable<Video>> GetVideos(int id);
  Task Delete(int id);
  Task<LibraryPath?> GetLibraryPathById(int id);
}