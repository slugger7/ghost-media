using Ghost.Data.Entities;

namespace Ghost.Dtos
{
  public class GenreDto
  {
    public string? _id { get; set; }
    public string? Name { get; set; }

    public GenreDto(Genre genre)
    {
      this._id = genre._id.ToString();
      this.Name = genre.Name;
    }

  }
}