namespace Ghost.Data.Ents
{
  public class VideoGenre
  {
    public virtual Video Video { get; set; }
    public virtual Genre Genre { get; set; }
  }
}