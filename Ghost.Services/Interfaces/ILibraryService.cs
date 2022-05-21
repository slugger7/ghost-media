using Ghost.Dtos;

namespace Ghost.Services
{
  public interface ILibraryService
  {
    LibraryDto Create(string libraryName);
    LibraryDto AddDirectories(int id, AddPathsToLibraryDto pathsLibraryDto);
    PageResultDto<LibraryDto> GetLibraries(int page = 0, int limit = 10);
    LibraryDto GetLibrary(int id);
    Task Sync(int id);
    Task Delete(int id);
    Task SyncNfos(int id);
    Task GenerateThumbnails(int id, bool overwrite);
    Task GenerateChapters(int id, bool overwrite);
  }
}