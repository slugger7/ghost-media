using Ghost.Data;
using Microsoft.EntityFrameworkCore;

namespace Ghost.Repository
{
  public class VideoRepository : IVideoRepository
  {
    private readonly GhostContext context;
    private readonly IActorRepository actorRepository;
    private readonly IGenreRepository genreRepository;

    public VideoRepository(
      GhostContext context,
      IActorRepository actorRepository,
      IGenreRepository genreRepository)
    {
      this.context = context;
      this.actorRepository = actorRepository;
      this.genreRepository = genreRepository;
    }

    public Video AddActors(int id, IEnumerable<Actor> actors)
    {
      var video = context.Videos.Find(id);
      if (video == null) throw new NullReferenceException("Video was null");

      var videoActors = actors.Select(a => new VideoActor
      {
        Actor = a,
        Video = video
      });

      video.VideoActors.AddRange(videoActors);
      context.SaveChanges();

      return video;
    }

    public Video? FindById(int id)
    {
      return context.Videos
        .Include("VideoGenres.Genre")
        .Include("VideoActors.Actor")
        .FirstOrDefault(v => v.Id == id);
    }

    public PageResult<Video> GetForGenre(string name, int page = 0, int limit = 10)
    {
      var genre = genreRepository.GetByName(name);

      if (genre == null) throw new NullReferenceException("Genre not found");

      return new PageResult<Video>
      {
        Total = genre.VideoGenres.Count(),
        Page = page,
        Content = genre.VideoGenres
          .OrderBy(vg => vg.Video.Title)
          .Skip(limit * page)
          .Take(limit)
          .Select(va => va.Video)
      };
    }

    public PageResult<Video> GetForActor(int actorId, int page = 0, int limit = 10)
    {
      var actor = actorRepository.FindById(actorId);

      if (actor == null) throw new NullReferenceException("Actor not found");

      return new PageResult<Video>
      {
        Total = actor.VideoActors.Count(),
        Page = page,
        Content = actor.VideoActors
          .OrderBy(va => va.Video.Title)
          .Skip(limit * page)
          .Take(limit)
          .Select(va => va.Video)
      };
    }

    public Video AddGenres(int id, IEnumerable<Genre> genres)
    {
      var video = context.Videos.Find(id);
      if (video == null) throw new NullReferenceException("Video was null");

      var videoGenres = genres.Select(g => new VideoGenre
      {
        Genre = g,
        Video = video
      });

      video.VideoGenres.AddRange(videoGenres);
      context.SaveChanges();

      return video;
    }

    public PageResult<Video> GetVideos(int page = 0, int limit = 10)
    {
      return new PageResult<Video>
      {
        Total = context.Videos.Count(),
        Page = page,
        Content = context.Videos
          .Include("VideoActors.Actor")
          .Include("VideoGenres.Genre")
          .OrderBy(v => v.Id)
          .Skip(limit * page)
          .Take(limit)
      };
    }
  }
}