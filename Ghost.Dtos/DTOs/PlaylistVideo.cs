using Ghost.Data;

namespace Ghost.Dtos;

public class PlaylistVideoDto
{
  public int Id { get; set; }
  public DateTime CreatedAt { get; set; }
  public VideoDto Video { get; set; } = null!;

  public PlaylistVideoDto(PlaylistVideo playlistVideo)
  {
    Id = playlistVideo.Id;
    CreatedAt = playlistVideo.CreatedAt;
    Video = new VideoDto(playlistVideo.Video);
  }
}