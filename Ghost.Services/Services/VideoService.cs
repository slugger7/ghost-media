using LiteDB;
using Ghost.Services.Interfaces;
using Ghost.Data.Entities;
using Ghost.Dtos;
using Ghost.Media;

namespace Ghost.Services
{
  public class VideoService : IVideoService
  {
    private static string connectionString = $"..{Path.DirectorySeparatorChar}Ghost.Data{Path.DirectorySeparatorChar}Ghost.db";

    private static string collectionName = "videos";

    private readonly IGenreService genreService;

    public VideoService(IGenreService genreService)
    {
      this.genreService = genreService;
    }

    internal static ILiteCollection<Video> GetCollection(LiteDatabase db)
    {
      var col = db.GetCollection<Video>(collectionName);
      col.EnsureIndex(v => v.Path);

      return col;
    }

    public PageResultDto<VideoDto> GetVideos(int page, int limit)
    {
      using (var db = new LiteDatabase(connectionString))
      {
        var col = VideoService.GetCollection(db);

        var total = col.Count();

        var videos = col.Query()
          .Limit(limit)
          .Skip(limit * page)
          .ToEnumerable()
          .Select(v => new VideoDto(v))
          .ToList();

        db.Dispose();

        return new PageResultDto<VideoDto>
        {
          Total = total,
          Page = page,
          Content = videos
        };
      }
    }

    public VideoDto GetVideoById(string id)
    {
      var _id = new ObjectId(id);

      using (var db = new LiteDatabase(connectionString))
      {
        var video = GetVideoEntityById(db, new ObjectId(id));

        return new VideoDto(video);
      }
    }

    private Video GetVideoEntityById(LiteDatabase db, ObjectId id)
    {
      var col = GetCollection(db);
      return col
        .Include(v => v.Genres)
        .FindById(id);
    }

    internal static void DeleteRange(IEnumerable<ObjectId?> ids)
    {
      using (var db = new LiteDatabase(connectionString))
      {
        var col = GetCollection(db);

        foreach (var id in ids)
        {
          col.Delete(id);
        }
      }
    }

    public string GenerateThumbnail(string id)
    {
      using (var db = new LiteDatabase(connectionString))
      {
        var col = GetCollection(db);

        var video = col.FindById(new ObjectId(id));

        if (video == null) throw new NullReferenceException("Video not found");
        if (video.Path == null) throw new NullReferenceException("Path was null");

        var basePath = video.Path
          .Substring(0, video.Path.LastIndexOf(Path.DirectorySeparatorChar)) + Path.DirectorySeparatorChar + video.Title + ".png";

        ImageFns.GenerateImage(video.Path, basePath);

        return basePath;
      }
    }

    public VideoMetaDataDto? GetVideoMetaData(string id)
    {
      var video = GetVideoById(id);

      if (video == null) throw new NullReferenceException("Video not found");
      if (video.Path == null) return default;

      return VideoFns.GetVideoInformation(video.Path);
    }

    public VideoDto AddGenreByNameToVideo(string id, string genre)
    {
      if (genre == string.Empty) throw new NullReferenceException("No genre");
      using (var db = new LiteDatabase(connectionString))
      {
        var video = GetVideoEntityById(db, new ObjectId(id));
        if (video == null) throw new NullReferenceException("Video not found");
        var genreEntity = GenreService.UpsertGenreByNameEntity(db, genre, video);
        if (genre == null) throw new NullReferenceException("Genre was not upserted");
        video.Genres.Add(genreEntity);

        var col = GetCollection(db);
        col.Update(video);

        return new VideoDto(video);
      }
    }
  }
}