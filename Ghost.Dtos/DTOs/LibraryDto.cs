using Ghost.Data.Entities;

namespace Ghost.Dtos
{
  public class LibraryDto
  {
    public string? _id {get;set;}
    public string? Name {get;set;}
    public List<LibraryPathDto> Paths {get;set;} = new List<LibraryPathDto>();

    public LibraryDto(Library library)
    {
      if (library != null)
      {
        this._id = library._id?.ToString();
        this.Name = library.Name;
        this.Paths = library.Paths
          .Select(f => new LibraryPathDto(f))
          .ToList();
      }
    }
  }
}