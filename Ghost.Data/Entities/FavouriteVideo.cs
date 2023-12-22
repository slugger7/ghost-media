namespace Ghost.Data;
public class FavouriteVideo
{
  public int Id { get; set; }
  public virtual Video Video { get; set; } = null!;
  public virtual User User { get; set; } = null!;
}