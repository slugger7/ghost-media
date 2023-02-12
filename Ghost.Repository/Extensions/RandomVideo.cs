using Ghost.Data;
using Ghost.Dtos;

namespace Ghost.Repository.Extensions
{
    public static class RandomVideoExtension
    {
        public static IEnumerable<Video> RandomVideo(
            this IQueryable<Video> videos,
            int userId,
            RandomVideoRequestDto randomVideoRequest
        )
        {
            return videos
                .FilterWatchedState(randomVideoRequest.WatchState, userId)
                .TitleSearch(randomVideoRequest.Search)
                .FilterGenres(randomVideoRequest.Genres);
        }
    }
}