using Ghost.Data;
using Microsoft.EntityFrameworkCore;

namespace Ghost.Repository
{
  public class GenreRepository : IGenreRepository
  {
    private readonly GhostContext context;

    public GenreRepository(GhostContext context)
    {
      this.context = context;
    }
    public Genre? FindById(int id)
    {
      return context.Genres.Find(id);
    }

    public Genre? GetByName(string name)
    {
      return context.Genres
        .Include("VideoGenres.Video")
        .FirstOrDefault(g => g.Name.ToUpper().Equals(name.ToUpper()));
    }

    public IEnumerable<Genre> GetGenres()
    {
      return context.Genres.Include("VideoGenres").OrderBy(g => g.Name);
    }

    public IEnumerable<Genre> Search(string search, int limit = 10)
    {
      return context.Genres
        .Where(g => g.Name.ToUpper().Contains(search.Trim().ToUpper()))
        .OrderBy(g => g.Name)
        .Take(limit);
    }

    public Genre Upsert(string name)
    {
      var genre = context.Genres
        .FirstOrDefault(g => g.Name.ToUpper().Equals(name.Trim().ToUpper()));
      if (genre == null)
      {
        genre = new Genre
        {
          Name = name.Trim()
        };

        context.Genres.Add(genre);
        context.SaveChanges();
      }

      return genre;
    }
  }
}