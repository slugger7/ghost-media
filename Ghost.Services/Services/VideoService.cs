using LiteDB;
using Ghost.Services.Interfaces;
using Ghost.Data.Entities;
using Ghost.Dtos;
using Ghost.Media;

namespace Ghost.Services
{
  public class VideoService : IVideoService
  {
    private static string connectionString = @"..\Ghost.Data\Ghost.db";
    private static string collectionName = "videos";

    internal static ILiteCollection<Video> GetCollection(LiteDatabase db)
    {
      var col = db.GetCollection<Video>("videos");
      col.EnsureIndex(v => v.Path);

      return col;
    }

    public PageResultDto<VideoDto> GetVideos(int page, int limit)
    {
      using (var db = new LiteDatabase(connectionString))
      {
        var col = db.GetCollection<Video>(collectionName);

        var size = col.Count();

        var videos = col.Query()
          .Limit(limit)
          .Skip(limit * page)
          .ToEnumerable()
          .Select(v => new VideoDto(v))
          .ToList();

        db.Dispose();

        return new PageResultDto<VideoDto>
        {
          Total = size,
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
        var col = db.GetCollection<Video>(collectionName);

        return new VideoDto(col.FindOne(v => v._id == _id));
      }
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
  }
}