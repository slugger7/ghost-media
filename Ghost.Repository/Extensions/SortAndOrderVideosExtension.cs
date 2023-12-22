using Ghost.Data;

namespace Ghost.Repository.Extensions;
public static class SortAndOrderVideosExtension
{
  public static IEnumerable<Video> SortAndOrderVideos(this IEnumerable<Video> videos, string sortBy, bool ascending)
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
}