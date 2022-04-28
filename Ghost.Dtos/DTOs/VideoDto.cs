using Ghost.Data;

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
    public List<ActorDto> Actors { get; set; } = new List<ActorDto>();

    public VideoDto(Video video)
    {
      if (video != null)
      {
        this._id = video.Id.ToString();
        this.Path = video.Path;
        this.FileName = video.FileName;
        this.Title = video.Title;
        this.Type = "video/mp4";
        this.Genres = video.VideoGenres
          .OrderBy(vg => vg.Genre.Name)
          .Select(vg => new GenreDto(vg.Genre))
          .ToList();
        this.Actors = video.VideoActors
          .OrderBy(va => va.Actor.Name)
          .Select(va => new ActorDto(va.Actor))
          .ToList();
      }
    }
  }
}