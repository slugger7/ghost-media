using Ghost.Dtos;

namespace Ghost.Services.Interfaces
{
  public interface ILibraryService
  {
    LibraryDto CreateLibrary(string libraryName);
    void AddDirectoryToLibrary(string _id, string directory);
    PageResultDto<LibraryDto> GetLibraries();
  }
}