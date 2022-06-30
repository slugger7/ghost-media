namespace Ghost.Data
{
  public class Actor
  {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public virtual List<VideoActor> VideoActors { get; set; } = new List<VideoActor>();
    public virtual List<FavouriteActor> FavouritedBy { get; set; } = new List<FavouriteActor>();
  }
}