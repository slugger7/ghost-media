namespace Ghost.Data.Ents
{
  public class VideoActor
  {
    public virtual Video Video { get; set; }
    public virtual Actor Actor { get; set; }
  }
}