namespace Ghost.Data
{
  public class FavouriteActor
  {
    public int Id { get; set; }
    public virtual Actor Actor { get; set; }
    public virtual User User { get; set; }
  }
}