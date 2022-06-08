namespace Ghost.Data
{
  public class User
  {
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public virtual List<FavouriteVideo> FavouriteVideos { get; set; } = new List<FavouriteVideo>();
    public virtual List<FavouriteActor> FavouriteActors { get; set; } = new List<FavouriteActor>();
  }
}