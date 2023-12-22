namespace Ghost.Data;

public class VideoActor
{
  public int Id { get; set; }
  public virtual Video Video { get; set; } = null!;
  public virtual Actor Actor { get; set; } = null!;
}