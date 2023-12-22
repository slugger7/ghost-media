using Ghost.Data;

namespace Ghost.Dtos;
public class GenreViewDto
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public int VideoCount { get; set; }

  public GenreViewDto(Genre genre)
  {
    this.Id = genre.Id;
    this.Name = genre.Name;
    this.VideoCount = genre.VideoGenres.Count;
  }
}