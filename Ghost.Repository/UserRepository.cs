using Ghost.Data;
using Ghost.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Ghost.Repository
{
  public class UserRepository : IUserRepository
  {
    private readonly GhostContext context;
    private readonly IVideoRepository videoRepository;
    public UserRepository(GhostContext context, IVideoRepository videoRepository)
    {
      this.context = context;
      this.videoRepository = videoRepository;
    }

    public async Task<User> Create(User user)
    {
      var userEntity = context.Users
        .FirstOrDefault(u => u.Username.ToLower().Equals(user.Username.Trim().ToLower()));
      if (userEntity != null) throw new UserExisistException();
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

    public User? FindById(int id)
    {
      return this.FindById(id, null);
    }

    public User? FindById(int id, List<string>? includes)
    {
      var users = context.Users;
      if (includes != null && includes.Count() > 0)
      {
        var userQueryable = users.Include(includes.ElementAt(0));
        for (int i = 1; i < includes.Count(); i++)
        {
          userQueryable = userQueryable.Include(includes.ElementAt(i));
        }
        return userQueryable.FirstOrDefault(v => v.Id == id);
      }

      return users.FirstOrDefault(v => v.Id == id);
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

    public async Task LogProgress(int id, int userId, double progress)
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
          Timestamp = progress,
          Video = video
        };

        video.WatchedBy.Add(newProgress);
      }
      else
      {
        existingProgress.Timestamp = progress;
      }

      await context.SaveChangesAsync();
    }

    public PageResult<Video> Favourites(int userId, int page = 0, int limit = 10, string search = "", string sortBy = "title", bool ascending = true)
    {
      var user = this.FindById(userId, new List<string>
      {
        "FavouriteVideos.Video.VideoImages.Image"
      });

      if (user == null) throw new NullReferenceException("User not found");
      var videos = user.FavouriteVideos
          .Select(fv => fv.Video)
          .Where(VideoRepository.videoSearch(search));

      videos = VideoRepository.SortAndOrderVideos(videos, sortBy, ascending);

      return new PageResult<Video>
      {
        Total = user.FavouriteVideos.Count(),
        Page = page,
        Content = videos
          .Skip(limit * page)
          .Take(limit)
      };
    }
  }
}