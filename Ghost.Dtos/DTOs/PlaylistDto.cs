using Ghost.Data;

namespace Ghost.Dtos;

public class PlaylistDto
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public DateTime CreatedAt { get; set; }
  public List<PlaylistVideoDto> PlaylistVideos { get; set; } = new List<PlaylistVideoDto>();

  public PlaylistDto(Playlist playlist)
  {
    Id = playlist.Id;
    Name = playlist.Name;
    CreatedAt = playlist.CreatedAt;

    if (playlist.PlaylistVideos != null)
    {
      PlaylistVideos = playlist.PlaylistVideos.Select(pv => new PlaylistVideoDto(pv)).ToList();
    }
  }
}