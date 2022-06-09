using Ghost.Data;

namespace Ghost.Dtos
{
  public class VideoDto
  {
    public int Id { get; set; }
    public string Path { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public long Size { get; set; }
    public double Runtime { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public DateTime DateAdded { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastNfoScan { get; set; }
    public DateTime LastMetadataUpdate { get; set; }
    public ImageDto? Thumbnail { get; set; }
    public bool Favourite { get; set; }
    public double Progress { get; set; }
    public List<GenreDto> Genres { get; set; } = new List<GenreDto>();
    public List<ActorDto> Actors { get; set; } = new List<ActorDto>();
    public List<ImageDto> Images { get; set; } = new List<ImageDto>();
    public List<ChapterDto> Chapters { get; set; } = new List<ChapterDto>();

    public VideoDto(Video video)
    {
      initialize(video);
    }

    public VideoDto(Video video, int userId)
    {
      initialize(video);
      this.Favourite = video.FavouritedBy.Any(f => f.User.Id == userId);
      var progress = video.WatchedBy.FirstOrDefault(w => w.User.Id == userId);
      this.Progress = progress != null ? progress.Timestamp : 0;
    }

    private void initialize(Video video)
    {
      if (video != null)
      {
        this.Id = video.Id;
        this.Path = video.Path;
        this.FileName = video.FileName;
        this.Title = video.Title;
        this.Type = "video/mp4";
        this.Height = video.Height;
        this.Width = video.Width;
        this.Size = video.Size;
        this.Runtime = video.Runtime;
        this.Created = video.Created;
        this.LastNfoScan = video.LastNfoScan;
        this.LastMetadataUpdate = video.LastMetadataUpdate;
        this.DateAdded = video.DateAdded;

        var thumbnail = video.VideoImages.FirstOrDefault(vi => vi.Type.Equals("thumbnail"));
        if (thumbnail is not null)
        {
          this.Thumbnail = new ImageDto(thumbnail.Image);
        }
        if (video.VideoGenres != null)
        {
          this.Genres = video.VideoGenres
            .OrderBy(vg => vg.Genre.Name)
            .Select(vg => new GenreDto(vg.Genre))
            .ToList();
        }
        if (video.VideoActors != null)
        {
          this.Actors = video.VideoActors
            .OrderBy(va => va.Actor.Name)
            .Select(va => new ActorDto(va.Actor))
            .ToList();
        }
        if (video.VideoImages != null)
        {
          this.Images = video.VideoImages
            .Select(vi => new ImageDto(vi.Image))
            .ToList();
        }
        if (video.Chapters != null)
        {
          this.Chapters = video.Chapters
            .OrderBy(c => c.Timestamp)
            .Select(c => new ChapterDto(c))
            .ToList();
        }
      }
    }
  }
}