using Ghost.Data;
using Ghost.Dtos;
using Ghost.Exceptions;
using Ghost.Repository.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Ghost.Repository;
public class UserRepository : IUserRepository
{
  private readonly GhostContext context;
  private readonly IVideoRepository videoRepository;
  private readonly IActorRepository actorRepository;

  public UserRepository(
    GhostContext context,
    IVideoRepository videoRepository,
    IActorRepository actorRepository)
  {
    this.context = context;
    this.videoRepository = videoRepository;
    this.actorRepository = actorRepository;
  }

  public async Task<User> Create(User user)
  {
    var userEntity = context.Users
      .FirstOrDefault(u => u.Username.ToLower().Equals(user.Username.Trim().ToLower()));
    if (userEntity != null) throw new UserExistsException();
    context.Users.Add(user);
    await context.SaveChangesAsync();

    return user;
  }

  public async Task<User> Delete(int id)
  {
    var user = context.Users.Find(id);
    if (user == null) throw new NullReferenceException("User was not found");

    context.Users.Remove(user);

    await context.SaveChangesAsync();

    return user;
  }

  public User? FindUserByLogin(string username, string password)
  {
    var user = context.Users
        .FirstOrDefault(u => u.Username.ToLower().Equals(username.ToLower())
            && u.Password.Equals(password)
        );

    return user;
  }

  public User? FindById(int id)
  {
    return this.FindById(id, null);
  }

  public User? FindById(int id, List<string>? includes)
  {
    return context.Users
      .AddIncludes(includes)
      .FirstOrDefault(v => v.Id == id);
  }

  public IEnumerable<User> GetUsers()
  {
    return context.Users;
  }

  public async Task<bool> ToggleFavouriteVideo(int id, int videoId)
  {
    var user = context.Users
      .Include("FavouriteVideos.Video")
      .FirstOrDefault(u => u.Id == id);
    if (user == null) throw new NullReferenceException("User not found");
    var video = videoRepository.FindById(videoId, null);
    if (video == null) throw new NullReferenceException("Video not found");

    var favourite = user.FavouriteVideos.FirstOrDefault(fv => fv.Video.Id == videoId);
    if (favourite == null)
    {
      favourite = new FavouriteVideo
      {
        User = user,
        Video = video
      };

      context.FavouriteVideos.Add(favourite);

      await context.SaveChangesAsync();

      return true;
    }
    else
    {
      context.FavouriteVideos.Remove(favourite);

      await context.SaveChangesAsync();

      return false;
    }
  }

  public async Task LogProgress(int id, int userId, ProgressUpdateDto progress)
  {
    var video = videoRepository.FindById(id, new List<string> { "WatchedBy.User" });
    if (video == null) throw new NullReferenceException("Video not found");

    var existingProgress = video.WatchedBy.FirstOrDefault(w => w.User.Id == userId);
    if (existingProgress == null)
    {
      var user = this.FindById(userId);
      if (user == null) throw new NullReferenceException("User not found");

      var newProgress = new Progress
      {
        User = user,
        Timestamp = progress.Progress,
        Video = video
      };
      existingProgress = newProgress;

    }
    else
    {
      if (progress.ReduceProgress)
      {
        existingProgress.Timestamp = progress.Progress;
      }
      else
      {
        existingProgress.Timestamp = progress.Progress > existingProgress.Timestamp
            ? progress.Progress
            : existingProgress.Timestamp;
      }

    }


    video.WatchedBy.Add(existingProgress);

    await context.SaveChangesAsync();
  }

  public PageResult<Video> Favourites(int userId, string watchState, string[]? genresFilter, int page = 0, int limit = 10, string search = "", string sortBy = "title", bool ascending = true)
  {
    var userIncludes = new List<string>
      {
          "FavouriteVideos.Video.VideoImages.Image",
          "FavouriteVideos.Video.VideoActors.Actor.FavouritedBy.User",
          "FavouriteVideos.Video.WatchedBy.User"
      };

    if (genresFilter != null)
    {
      userIncludes.Add("FavouriteVideos.Video.VideoGenres.Genre");
    }

    var user = this.FindById(userId, userIncludes);

    if (user == null) throw new NullReferenceException("User not found");
    var videos = user.FavouriteVideos
        .Select(fv => fv.Video)
        .Search(search)
        .FilterWatchedState(watchState, userId)
        .FilterGenres(genresFilter)
        .SortAndOrderVideos(sortBy, ascending);

    return new PageResult<Video>
    {
      Total = videos.Count(),
      Page = page,
      Content = videos
        .Skip(limit * page)
        .Take(limit)
    };
  }

  public async Task<bool> ToggleFavouriteActor(int id, int actorId)
  {
    var user = context.Users
      .Include("FavouriteActors.Actor")
      .FirstOrDefault(u => u.Id == id);
    if (user == null) throw new NullReferenceException("User not found");
    var actor = actorRepository.FindById(actorId, null);
    if (actor == null) throw new NullReferenceException("Video not found");

    var favourite = user.FavouriteActors.FirstOrDefault(fa => fa.Actor.Id == actorId);
    if (favourite == null)
    {
      favourite = new FavouriteActor
      {
        User = user,
        Actor = actor
      };

      context.FavouriteActors.Add(favourite);

      await context.SaveChangesAsync();

      return true;
    }
    else
    {
      context.FavouriteActors.Remove(favourite);

      await context.SaveChangesAsync();

      return false;
    }
  }

  public Video GetRandomVideoFromFavourites(int userId, RandomVideoRequestDto randomVideoRequest)
  {
    var userIncludes = new List<string> {
      "FavouriteVideos.Video",
      "FavouriteVideos.Video.WatchedBy.User",
      "FavouriteVideos.Video.VideoGenres.Genre"
    };

    var user = this.FindById(userId, userIncludes);

    if (user == null) throw new NullReferenceException("User not found when searching for random video");

    var video = user.FavouriteVideos
        .Select(vg => vg.Video)
        .RandomVideo(userId, randomVideoRequest);

    return video;
  }
}