using Ghost.Data.Entities;
using Ghost.Dtos;
using Ghost.Services.Interfaces;
using LiteDB;

namespace Ghost.Services
{
  public class GenreService : IGenreService
  {
    private static string connectionString = $"..{Path.DirectorySeparatorChar}Ghost.Data{Path.DirectorySeparatorChar}Ghost.db";
    private static string collectionName = "genres";

    internal static ILiteCollection<Genre> GetCollection(LiteDatabase db)
    {
      var col = db.GetCollection<Genre>(collectionName);
      col.EnsureIndex(g => g.Name);

      return col;
    }

    internal static Genre UpsertGenreByNameEntity(LiteDatabase db, string name, Video video)
    {
      var col = GenreService.GetCollection(db);

      var genre = col.FindOne(g => g.Name.ToUpper().Equals(name.ToUpper()));

      if (genre == null)
      {
        genre = new Genre
        {
          Name = name
          // Videos = new List<Video> { video }
        };

        col.Insert(genre);
      }
      // else
      // {
      //   genre.Videos.Add(video);

      //   col.Update(genre);
      // }

      return genre;
    }

    public GenreViewDto GetGenreByName(string name)
    {
      using (var db = new LiteDatabase(connectionString))
      {
        var col = GetCollection(db);

        var genre = col
          // .Include(g => g.Videos)
          .FindOne(g => g.Name.ToUpper().Equals(name.ToUpper()));

        if (genre == null) throw new NullReferenceException("Genre not found");

        return new GenreViewDto(genre);
      }
    }

    public List<GenreDto> GetGenres()
    {
      using (var db = new LiteDatabase(connectionString))
      {
        var col = GetCollection(db);

        var genres = col.Query()
          .OrderBy(g => g.Name)
          .ToEnumerable()
          .Select(g => new GenreDto(g))
          .ToList();

        return genres;
      }
    }

    public List<GenreDto> SearchTopGenres(int limit, string search)
    {
      using (var db = new LiteDatabase(connectionString))
      {
        var col = GetCollection(db);

        return col.Query()
          .Where(g => g.Name.Contains(search))
          .OrderBy(g => g.Name)
          .Limit(limit)
          .ToEnumerable()
          .Select(g => new GenreDto(g))
          .ToList();
      }
    }
  }
}