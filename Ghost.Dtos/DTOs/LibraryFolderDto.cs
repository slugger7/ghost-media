using Ghost.Data.Entities;

namespace Ghost.Dtos
{
  public class LibraryFolderDto
  {
    public string? _id {get;set;}
    public string? Path {get;set;}

    public LibraryFolderDto(LibraryFolder libraryFolder)
    {
      if (libraryFolder != null)
      {
        this._id = libraryFolder._id?.ToString();
        this.Path = libraryFolder.Path;
      }
    }
  }
}