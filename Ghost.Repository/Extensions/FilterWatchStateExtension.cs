using Ghost.Data;
using Ghost.Data.Enums;

namespace Ghost.Repository.Extensions;
public static class FilterWatchedStateExtension
{
  public static IEnumerable<Video> FilterWatchedState(this IEnumerable<Video> videos, string watchState, int userId)
  {
    if (!watchState.Equals(WatchState.All))
    {
      return videos.Where(v =>
      {
        var progress = v.WatchedBy.FirstOrDefault(p => p.User.Id == userId);
        if (progress == null)
        {
          return !watchState.Equals(WatchState.Watched) && !watchState.Equals(WatchState.InProgress);
        }

        var watchedPercentage = progress.Timestamp * 1000 / v.Runtime;
        if (watchState.Equals(WatchState.Unwatched))
        {
          return watchedPercentage <= 0.1;
        }
        else if (watchState.Equals(WatchState.InProgress))
        {
          return 0.1 < watchedPercentage && watchedPercentage < 0.9;
        }
        else
        {
          return 0.9 <= watchedPercentage;
        }
      });
    }
    return videos;
  }
}