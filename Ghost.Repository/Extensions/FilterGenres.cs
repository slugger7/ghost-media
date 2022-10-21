using Ghost.Data;

namespace Ghost.Repository.Extensions
{
    public static class FilterGenresExtension
    {
        public static IEnumerable<Video> FilterGenres(this IEnumerable<Video> videos, string[]? genres)
        {
            if (genres == null || (genres != null && genres.Count() == 0))
            {
                return videos;
            }

            return videos
                .Where(video => genres
                    .ToArray()
                    .All(genre => video.VideoGenres
                        .Any(
                            videoGenre => videoGenre.Genre.Name
                                .ToLower()
                                .Equals(genre.ToLower())
                        )
                    )
                );
        }
    }
}