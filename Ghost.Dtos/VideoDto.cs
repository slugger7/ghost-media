using Ghost.Data.Entities;

namespace Ghost.Dtos
{
  public class VideoDto
  {
    public string? _id {get;set;}
    public string? Path {get;set;}
    public string? Title {get;set;}
    public string? Type {get;set;}

    public VideoDto(Video video) 
    {
      if (video != null) {
        this._id = video._id?.ToString();
        this.Path = video.Path;
        this.Title = video.Title;
        this.Type = "video/mp4";
      }
    }
  }
}