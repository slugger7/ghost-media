using Ghost.Data;
using Microsoft.EntityFrameworkCore;

namespace Ghost.Repository
{
  public class VideoRepository : IVideoRepository
  {
    private readonly GhostContext context;
    private readonly IActorRepository actorRepository;
    private readonly IGenreRepository genreRepository;
    private Func<String, Func<Video, bool>> videoSearch = search => v => v.Title.ToUpper().Contains(search.Trim().ToUpper());

    public VideoRepository(
      GhostContext context,
      IActorRepository actorRepository,
      IGenreRepository genreRepository)
    {
      this.context = context;
      this.actorRepository = actorRepository;
      this.genreRepository = genreRepository;
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
    public PageResult<Video> GetForGenre(string name, int page = 0, int limit = 10, string search = "")
    {
      var genre = genreRepository.GetByName(name);

      if (genre == null) throw new NullReferenceException("Genre not found");
      var videos = genre.VideoGenres
          .OrderBy(vg => vg.Video.Title)
          .Select(vg => vg.Video)
          .Where(videoSearch(search));
      return new PageResult<Video>
      {
        Total = videos.Count(),
        Page = page,
        Content = videos
          .Skip(limit * page)
          .Take(limit)
      };
    }

    public PageResult<Video> GetForActor(int actorId, int page = 0, int limit = 10, string search = "")
    {
      var actor = actorRepository.FindById(actorId);

      if (actor == null) throw new NullReferenceException("Actor not found");
      var videos = actor.VideoActors
          .OrderBy(va => va.Video.Title)
          .Select(va => va.Video)
          .Where(videoSearch(search));
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

    public PageResult<Video> SearchVideos(int page = 0, int limit = 10, string search = "")
    {
      var videos = context.Videos
          .Include("VideoActors.Actor")
          .Include("VideoGenres.Genre")
          .Include("VideoImages.Image")
          .Where(videoSearch(search))
          .OrderBy(v => v.Title);
      return new PageResult<Video>
      {
        Total = videos.Count(),
        Page = page,
        Content = videos
          .Skip(limit * page)
          .Take(limit)
      };
    }

    public async Task Delete(int id)
    {
      var video = context.Videos
        .Include("VideoActors")
        .Include("VideoGenres")
        .FirstOrDefault(v => v.Id == id);
      if (video == null) throw new NullReferenceException();

      foreach (var videoGenre in video.VideoGenres)
      {
        context.VideoGenres.Remove(videoGenre);
      }

      foreach (var videoActor in video.VideoActors)
      {
        context.VideoActors.Remove(videoActor);
      }

      await context.SaveChangesAsync();
    }

    public async Task<Video> UpdateTitle(int id, string title)
    {
      var video = this.FindById(id);
      if (video == null) throw new NullReferenceException("Video not fonud");

      video.Title = title.Trim();

      await context.SaveChangesAsync();

      return video;
    }
  }
}