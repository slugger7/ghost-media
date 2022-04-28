using Ghost.Data;

namespace Ghost.Dtos
{
  public class GenreViewDto
  {
    public int _id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<VideoDto> Videos { get; set; } = new List<VideoDto>();

    public GenreViewDto(Genre genre)
    {
      this._id = genre.Id;
      this.Name = genre.Name;
    }
  }
}