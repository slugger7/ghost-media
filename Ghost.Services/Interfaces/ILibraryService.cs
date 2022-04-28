using Ghost.Dtos;

namespace Ghost.Services
{
  public interface ILibraryService
  {
    LibraryDto Create(string libraryName);
    LibraryDto AddDirectories(int id, AddPathsToLibraryDto pathsLibraryDto);
    PageResultDto<LibraryDto> GetLibraries(int page = 0, int limit = 10);
    LibraryDto GetLibrary(int id);
    void Sync(int id);
    Task Delete(int id);
  }
}