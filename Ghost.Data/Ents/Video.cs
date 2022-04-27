namespace Ghost.Data.Ents
{
  public class Video
  {
    public int Id { get; set; }
    public string FileName { get; set; }
    public string Title { get; set; }
    public string Path { get; set; }
    public virtual LibraryPath LibraryPath { get; set; }
    public virtual List<VideoGenre> VideoGenres { get; set; }
    public virtual List<VideoActor> VideoActors { get; set; }
  }
}