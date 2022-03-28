using Ghost.Dtos;

namespace Ghost.Services.Interfaces
{
  public interface ILibraryService
  {
    LibraryDto CreateLibrary(string libraryName);
    LibraryDto AddDirectoryToLibrary(string id, AddPathsToLibraryDto pathsLibraryDto);
    PageResultDto<LibraryDto> GetLibraries(int page, int limit);
  }
}