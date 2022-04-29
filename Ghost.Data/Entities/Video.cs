namespace Ghost.Data
{
  public class Video
  {
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public DateTime DateAdded { get; set; } = DateTime.UtcNow;
    public virtual LibraryPath LibraryPath { get; set; }
    public virtual List<VideoGenre> VideoGenres { get; set; } = new List<VideoGenre>();
    public virtual List<VideoActor> VideoActors { get; set; } = new List<VideoActor>();
    public virtual List<VideoImage> VideoImages { get; set; } = new List<VideoImage>();
  }
}