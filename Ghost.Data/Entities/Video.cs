namespace Ghost.Data;
public class Video
{
  public int Id { get; set; }
  public string FileName { get; set; } = string.Empty;
  public string Title { get; set; } = string.Empty;
  public string Path { get; set; } = string.Empty;
  public int Height { get; set; }
  public int Width { get; set; }
  public double Runtime { get; set; }
  public long Size { get; set; }
  public DateTime LastNfoScan { get; set; }
  public DateTime LastMetadataUpdate { get; set; }
  public DateTime DateAdded { get; set; } = DateTime.UtcNow;
  public DateTime Created { get; set; }
  public virtual LibraryPath LibraryPath { get; set; } = null!;
  public virtual List<VideoGenre> VideoGenres { get; set; } = new List<VideoGenre>();
  public virtual List<VideoActor> VideoActors { get; set; } = new List<VideoActor>();
  public virtual List<VideoImage> VideoImages { get; set; } = new List<VideoImage>();
  public virtual List<Chapter> Chapters { get; set; } = new List<Chapter>();
  public virtual List<FavouriteVideo> FavouritedBy { get; set; } = new List<FavouriteVideo>();
  public virtual List<Progress> WatchedBy { get; set; } = new List<Progress>();
  public virtual List<RelatedVideo> RelatedVideos { get; set; } = new List<RelatedVideo>();

  public static Func<string, Func<Video, string>> SortByPredicate = sortBy => video =>
    sortBy.ToLower().Equals("title") ? video.Title
      : sortBy.ToLower().Equals("date-added") ? video.DateAdded.Ticks.ToString()
      : sortBy.ToLower().Equals("date-created") ? video.Created.Ticks.ToString()
      : sortBy.ToLower().Equals("size") ? video.Size.ToString()
      : sortBy.ToLower().Equals("runtime") ? video.Runtime.ToString() : video.Title;
}