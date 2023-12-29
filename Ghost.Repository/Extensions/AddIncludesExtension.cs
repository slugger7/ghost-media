using Microsoft.EntityFrameworkCore;

namespace Ghost.Repository.Extensions;

public static class AddIncludesExtension
{
  public static IQueryable<T> AddIncludes<T>(this IQueryable<T> query, List<string>? includes) where T : class
  {
    if (includes == null) return query;

    foreach (var include in includes)
    {
      query = query.Include(include);
    }

    return query;
  }
}