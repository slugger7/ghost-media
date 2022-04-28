using Ghost.Data;

namespace Ghost.Dtos
{
  public class GenreDto
  {
    public string? _id { get; set; }
    public string? Name { get; set; }

    public GenreDto(Genre genre)
    {
      this._id = genre.Id.ToString();
      this.Name = genre.Name;
    }
  }
}