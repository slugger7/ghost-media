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

    internal static Genre UpsertGenreByNameEntity(LiteDatabase db, string name)
    {
      var col = GenreService.GetCollection(db);

      var genre = col.FindOne(g => g.Name.ToUpper().Equals(name.ToUpper()));

      if (genre == null)
      {
        genre = new Genre
        {
          Name = name
        };

        col.Insert(genre);
      }

      return genre;
    }
  }
}