using Ghost.Data;

namespace Ghost.Repository.Extensions
{
    public static class TitleSearchExtension
    {
        public static IEnumerable<Video> TitleSearch(this IEnumerable<Video> videos, string search)
        {
            return videos
                .Where(v => v.Title.ToUpper().Contains(search.Trim().ToUpper()));
        }
    }
}