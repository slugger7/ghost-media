namespace Ghost.Data;

public class Playlist
{
  public int Id { get; set; }
  public string Name { get; set; } = null!;
  public DateTime CreatedAt { get; set; }
  public User User { get; set; } = null!;
  public virtual ICollection<PlaylistVideo> PlaylistVideos { get; set; } = null!;
}