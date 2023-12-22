using Ghost.Data;

namespace Ghost.Dtos;
public class LibraryPathDto
{
  public string? Id { get; set; }
  public string? Path { get; set; }

  public LibraryPathDto(LibraryPath libraryPath)
  {
    if (libraryPath != null)
    {
      this.Id = libraryPath.Id.ToString();
      this.Path = libraryPath.Path;
    }
  }
}