using Ghost.Data.Entities;

namespace Ghost.Dtos
{
  public class VideoDto
  {
    public string? _id { get; set; }
    public string? Path { get; set; }
    public string? Title { get; set; }
    public string? FileName { get; set; }
    public string? Type { get; set; }
    public List<GenreDto> Genres { get; set; } = new List<GenreDto>();

    public VideoDto(Video video)
    {
      if (video != null)
      {
        this._id = video._id?.ToString();
        this.Path = video.Path;
        this.FileName = video.FileName;
        this.Title = video.Title;
        this.Type = "video/mp4";
        this.Genres = video.Genres.Select(g => new GenreDto(g)).ToList();
      }
    }
  }
}