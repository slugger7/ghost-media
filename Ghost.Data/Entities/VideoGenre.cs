namespace Ghost.Data
{
  public class VideoGenre
  {
    public int Id { get; set; }
    public virtual Video Video { get; set; }
    public virtual Genre Genre { get; set; }
  }
}