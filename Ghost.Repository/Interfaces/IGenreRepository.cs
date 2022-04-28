using Ghost.Data;

namespace Ghost.Repository
{
  public interface IGenreRepository
  {
    Genre? FindById(int id);
    IEnumerable<Genre> GetGenres();
    Genre? GetByName(string name);
    IEnumerable<Genre> Search(string search, int limit = 10);
    Genre Upsert(string name);
  }
}