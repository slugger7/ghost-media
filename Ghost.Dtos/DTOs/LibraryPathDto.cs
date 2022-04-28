using Ghost.Data;

namespace Ghost.Dtos
{
  public class LibraryPathDto
  {
    public string? _id { get; set; }
    public string? Path { get; set; }

    public LibraryPathDto(LibraryPath libraryPath)
    {
      if (libraryPath != null)
      {
        this._id = libraryPath.Id.ToString();
        this.Path = libraryPath.Path;
      }
    }
  }
}