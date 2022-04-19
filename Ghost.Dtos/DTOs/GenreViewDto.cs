using Ghost.Data.Entities;

namespace Ghost.Dtos
{
  public class GenreViewDto
  {
    public string _id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<VideoDto> Videos { get; set; } = new List<VideoDto>();

    public GenreViewDto(Genre genre)
    {
      this._id = genre._id.ToString();
      this.Name = genre.Name;
      // this.Videos = genre.Videos.Select(v => new VideoDto(v)).ToList();
    }
  }
}