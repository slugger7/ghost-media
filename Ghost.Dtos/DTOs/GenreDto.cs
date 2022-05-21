using Ghost.Data;

namespace Ghost.Dtos
{
  public class GenreDto
  {
    public string _id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int VideoCount { get; set; }

    public GenreDto(Genre genre)
    {
      this._id = genre.Id.ToString();
      this.Name = genre.Name;
      if (genre.VideoGenres != null)
      {
        this.VideoCount = genre.VideoGenres.Count();
      }
    }
  }
}