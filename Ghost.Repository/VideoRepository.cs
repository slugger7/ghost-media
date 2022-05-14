using Ghost.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ghost.Repository
{
  public class VideoRepository : IVideoRepository
  {
    private readonly GhostContext context;
    private readonly ILogger<VideoRepository> logger;
    private readonly IActorRepository actorRepository;
    private readonly IGenreRepository genreRepository;
    private readonly IImageRepository imageRepository;
    private Func<String, Func<Video, bool>> videoSearch = search => v => v.Title.ToUpper().Contains(search.Trim().ToUpper());

    public VideoRepository(
      GhostContext context,
      ILogger<VideoRepository> logger,
      IActorRepository actorRepository,
      IGenreRepository genreRepository,
      IImageRepository imageRepository)
    {
      this.context = context;
      this.logger = logger;
      this.actorRepository = actorRepository;
      this.genreRepository = genreRepository;
      this.imageRepository = imageRepository;
    }

    public Video SetActors(int id, IEnumerable<Actor> actors)
    {
      var video = context.Videos.Find(id);
      if (video == null) throw new NullReferenceException("Video was null");

      var videoActors = actors.Select(a => new VideoActor
      {
        Actor = a,
        Video = video
      });

      context.VideoActors.RemoveRange(video.VideoActors);
      video.VideoActors = videoActors.ToList();
      context.SaveChanges();

      return video;
    }

    public Video? FindById(int id)
    {
      return context.Videos
        .Include("VideoGenres.Genre.VideoGenres")
        .Include("VideoActors.Actor.VideoActors")
        .Include("VideoImages.Image")
        .FirstOrDefault(v => v.Id == id);
    }
    public static IEnumerable<Video> SortAndOrderVideos(IEnumerable<Video> videos, string sortBy, bool ascending)
    {
      var orderByPredicate = Video.SortByPredicate(sortBy);
      if (ascending)
      {
        return videos.OrderBy(orderByPredicate);
      }
      else
      {
        return videos.OrderByDescending(orderByPredicate);
      }
    }

    public PageResult<Video> GetForGenre(string name, int page = 0, int limit = 10, string search = "", string sortBy = "title", bool ascending = true)
    {
      var genre = genreRepository.GetByName(name);

      if (genre == null) throw new NullReferenceException("Genre not found");
      var videos = genre.VideoGenres
          .Select(vg => vg.Video)
          .Where(videoSearch(search));

      videos = SortAndOrderVideos(videos, sortBy, ascending);

      return new PageResult<Video>
      {
        Total = videos.Count(),
        Page = page,
        Content = videos
          .Skip(limit * page)
          .Take(limit)
      };
    }

    public PageResult<Video> GetForActor(int actorId, int page = 0, int limit = 10, string search = "", string sortBy = "title", bool ascending = true)
    {
      var actor = actorRepository.FindById(actorId);

      if (actor == null) throw new NullReferenceException("Actor not found");
      var videos = actor.VideoActors
          .Select(va => va.Video)
          .Where(videoSearch(search));

      videos = SortAndOrderVideos(videos, sortBy, ascending);
      return new PageResult<Video>
      {
        Total = videos.Count(),
        Page = page,
        Content = videos
          .Skip(limit * page)
          .Take(limit)
      };
    }

    public Video SetGenres(int id, IEnumerable<Genre> genres)
    {
      var video = context.Videos.Find(id);
      if (video == null) throw new NullReferenceException("Video was null");

      var videoGenres = genres.Select(g => new VideoGenre
      {
        Genre = g,
        Video = video
      });

      context.VideoGenres.RemoveRange(video.VideoGenres);
      video.VideoGenres = videoGenres.ToList();
      context.SaveChanges();

      return video;
    }

    public PageResult<Video> SearchVideos(int page = 0, int limit = 10, string search = "", string sortBy = "title", bool ascending = true)
    {
      var videos = context.Videos
          .Include("VideoActors.Actor")
          .Include("VideoGenres.Genre")
          .Include("VideoImages.Image")
          .Where(videoSearch(search));

      videos = SortAndOrderVideos(videos, sortBy, ascending);

      return new PageResult<Video>
      {
        Total = videos.Count(),
        Page = page,
        Content = videos
          .Skip(limit * page)
          .Take(limit)
      };
    }

    public async Task<Video> Delete(int id)
    {
      var video = context.Videos
        .Include("VideoActors")
        .Include("VideoGenres")
        .Include("VideoImages.Image")
        .FirstOrDefault(v => v.Id == id);
      if (video == null) throw new NullReferenceException();

      context.VideoGenres.RemoveRange(video.VideoGenres);
      context.VideoActors.RemoveRange(video.VideoActors);
      context.Images.RemoveRange(video.VideoImages.Select(vi => vi.Image));
      context.VideoImages.RemoveRange(video.VideoImages);
      context.Videos.Remove(video);

      await context.SaveChangesAsync();
      logger.LogDebug("Saving changes");
      return video;
    }

    public async Task<Video> UpdateTitle(int id, string title)
    {
      var video = this.FindById(id);
      if (video == null) throw new NullReferenceException("Video not fonud");

      video.Title = title.Trim();

      await context.SaveChangesAsync();

      return video;
    }

    public async Task<Video> UpdateVideo(Video video)
    {
      var videoEntity = this.FindById(video.Id);
      if (videoEntity is null) throw new NullReferenceException("Video not found to update");

      videoEntity.Title = video.Title.Trim();
      videoEntity.DateAdded = video.DateAdded;
      videoEntity.Created = video.Created;
      videoEntity.Size = video.Size;
      videoEntity.Runtime = video.Runtime;
      videoEntity.Height = video.Height;
      videoEntity.Width = video.Width;
      videoEntity.LastMetadataUpdate = video.LastMetadataUpdate;
      videoEntity.LastNfoScan = video.LastNfoScan;

      await context.SaveChangesAsync();

      return video;
    }
  }
}