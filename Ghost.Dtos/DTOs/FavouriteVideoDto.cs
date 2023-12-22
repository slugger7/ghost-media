using Ghost.Data;

namespace Ghost.Dtos;
public class FavouriteVideoDto
{
  public int Id { get; set; }
  public VideoDto Video { get; set; }

  public FavouriteVideoDto(FavouriteVideo favouriteVideo)
  {
    this.Id = favouriteVideo.Id;
    this.Video = new VideoDto(favouriteVideo.Video);
  }
}