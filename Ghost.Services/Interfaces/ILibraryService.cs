using Ghost.Dtos;

namespace Ghost.Services.Interfaces
{
  public interface ILibraryService
  {
    LibraryDto CreateLibrary(string libraryName);
    LibraryDto AddDirectoryToLibrary(string id, AddFolderToLibraryDto folderLibraryDto);
    PageResultDto<LibraryDto> GetLibraries(int page, int limit);
  }
}