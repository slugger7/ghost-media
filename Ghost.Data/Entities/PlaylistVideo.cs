namespace Ghost.Data;

public class PlaylistVideo
{
  public int Id { get; set; }
  public DateTime CreatedAt { get; set; }
  public virtual Video Video { get; set; } = null!;
  public virtual Playlist Playlist { get; set; } = null!;
}