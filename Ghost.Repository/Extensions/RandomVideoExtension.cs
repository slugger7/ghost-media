using Ghost.Data;
using Ghost.Dtos;

namespace Ghost.Repository.Extensions;
public static class RandomVideoExtension
{
  public static Video RandomVideo(
      this IEnumerable<Video> videos,
      int userId,
      RandomVideoRequestDto randomVideoRequest
  )
  {
    Random rnd = new Random();

    var result = videos
        .FilterWatchedState(randomVideoRequest.WatchState, userId)
        .Search(randomVideoRequest.Search)
        .FilterGenres(randomVideoRequest.Genres);

    var count = result.Count();

    return result.ElementAt(rnd.Next(0, count));
  }
}