using Ghost.Data;

namespace Ghost.Repository.Extensions;
public static class SearchExtension
{
  public static IEnumerable<Video> Search(this IEnumerable<Video> videos, string search)
  {
    var normalisedSearch = search.Trim().ToUpper();
    return videos
        .Where(v => v.Title.ToUpper().Contains(normalisedSearch)
          || v.Path.ToUpper().Contains(normalisedSearch));
  }
}