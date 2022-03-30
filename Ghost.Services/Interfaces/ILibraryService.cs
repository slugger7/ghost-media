using Ghost.Dtos;

namespace Ghost.Services.Interfaces
{
  public interface ILibraryService
  {
    LibraryDto Create(string libraryName);
    LibraryDto AddDirectory(string id, AddPathsToLibraryDto pathsLibraryDto);
    PageResultDto<LibraryDto> GetMany(int page, int limit);
    LibraryDto GetDto(string id);
    void Sync(string id);
    void Delete(string id);
  }
}