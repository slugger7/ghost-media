namespace Ghost.Data.Ents
{
  public class Actor
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public virtual List<VideoActor> VideoActors { get; set; }
  }
}