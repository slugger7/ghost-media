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
        .Include("VideoGenres.Video.VideoImages.Image")
        .Include("VideoGenres.Video.FavouritedBy.User")
        .FirstOrDefault(g => g.Name.ToUpper().Equals(name.ToUpper()));
    }

    public IEnumerable<Genre> GetGenres()
    {
      return context.Genres.Include("VideoGenres").OrderByDescending(g => g.VideoGenres.Count());
    }

    public IEnumerable<Genre> GetGenresForVideo(int videoId)
    {
      return context.VideoGenres
      .Include("Genre.VideoGenres")
      .Where(vg => vg.Video.Id == videoId)
      .Select(vg => vg.Genre)
      .OrderByDescending(g => g.VideoGenres.Count());
    }

    public IEnumerable<Genre> Search(string search, int limit = 10)
    {
      return context.Genres
        .Where(g => g.Name.ToUpper().Contains(search.Trim().ToUpper()))
        .OrderBy(g => g.Name)
        .Take(limit);
    }

    public async Task<Genre> UpdateGenre(int id, string name)
    {
      var genre = this.FindById(id);
      if (genre == null) throw new NullReferenceException("Genre was not found");

      // What happens when the new genre name is equal to an existing genre name
      genre.Name = name;

      await context.SaveChangesAsync();

      return genre;
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