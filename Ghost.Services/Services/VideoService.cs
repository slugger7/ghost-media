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
    private readonly IActorService actorService;

    public VideoService(IGenreService genreService, IActorService actorService)
    {
      this.genreService = genreService;
      this.actorService = actorService;
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

        var videos = col
          .Include(v => v.Genres)
          .Include(v => v.Actors)
          .Query()
          .OrderBy(v => v.Title)
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
        .Include(v => v.Actors)
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

    public VideoDto AddGenresByNameToVideo(string id, List<string> genres)
    {
      if (genres == null) throw new NullReferenceException("No genres");
      using (var db = new LiteDatabase(connectionString))
      {
        var video = GetVideoEntityById(db, new ObjectId(id));
        if (video == null) throw new NullReferenceException("Video not found");
        var genreEntities = genres.Select(g => GenreService.UpsertGenreByNameEntity(db, g, video));
        video.Genres = genreEntities.ToList();

        var col = GetCollection(db);
        col.Update(video);

        return new VideoDto(video);
      }
    }

    public PageResultDto<VideoDto> GetVideosForGenre(string genre, int page, int limit)
    {
      using (var db = new LiteDatabase(connectionString))
      {
        var col = GetCollection(db);

        var genreDto = genreService.GetGenreByName(genre);
        var genreId = new ObjectId(genreDto._id);

        var videos = col.Query()
          .Where(v => v.Genres.Select(g => g._id).Any(id => id.Equals(genreId)));

        var count = videos.Count();

        return new PageResultDto<VideoDto>
        {
          Total = count,
          Page = page,
          Content = videos
            .OrderBy(v => v.Title)
            .Limit(limit)
            .Skip(limit * page)
            .ToEnumerable()
            .Select(v => new VideoDto(v))
            .ToList()
        };
      }
    }

    public VideoDto AddActorsByNameToVideo(string id, List<string> actors)
    {
      if (actors == null) throw new NullReferenceException("Actors not found");
      using (var db = new LiteDatabase(connectionString))
      {
        var video = GetVideoEntityById(db, new ObjectId(id));
        if (video == null) throw new NullReferenceException("Video not found");
        var actorEntities = actors.Select(a => ActorService.UpsertActorByNameEntity(db, a, video));
        video.Actors = actorEntities.ToList();

        var col = GetCollection(db);
        col.Update(video);

        return new VideoDto(video);
      }
    }

    public PageResultDto<VideoDto> GetVideosForActor(string id, int page, int limit)
    {
      using (var db = new LiteDatabase(connectionString))
      {
        var col = GetCollection(db);

        var actorId = new ObjectId(id);

        var videos = col.Query()
          .Where(v => v.Actors.Select(a => a._id).Any(id => id.Equals(actorId)));

        var count = videos.Count();

        return new PageResultDto<VideoDto>
        {
          Total = count,
          Page = page,
          Content = videos
            .OrderBy(v => v.Title)
            .Limit(limit)
            .Skip(limit * page)
            .ToEnumerable()
            .Select(v => new VideoDto(v))
            .ToList()
        };
      }
    }
  }
}