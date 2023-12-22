using Ghost.Data;

namespace Ghost.Repository;
public interface IGenreRepository
{
  Genre? FindById(int id);
  IEnumerable<Genre> GetGenres();
  Genre? GetByName(string name);
  Genre? GetByName(string name, List<string> includes);
  IEnumerable<Genre> Search(string search, int limit = 10);
  Genre Upsert(string name);
  IEnumerable<Genre> GetGenresForVideo(int videoId);
  Task<Genre> UpdateGenre(int id, string name);
}