using Ghost.Data;

namespace Ghost.Dtos;
public class LibraryDto
{
  public string? Id { get; set; }
  public string? Name { get; set; }
  public List<LibraryPathDto> Paths { get; set; } = new List<LibraryPathDto>();

  public LibraryDto(Library library)
  {
    this.Id = library.Id.ToString();
    this.Name = library.Name;
    if (library.Paths != null)
    {
      this.Paths = library.Paths
        .Select(p => new LibraryPathDto(p))
        .ToList();
    }
  }
}