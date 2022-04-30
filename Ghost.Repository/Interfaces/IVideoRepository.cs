using Ghost.Data;

namespace Ghost.Repository
{
  public interface IVideoRepository
  {
    PageResult<Video> SearchVideos(int page = 0, int limit = 10, string search = "");
    PageResult<Video> GetForActor(int actorId, int page = 0, int limit = 10, string search = "");
    PageResult<Video> GetForGenre(string name, int page = 0, int limit = 10, string search = "");
    Video? FindById(int id);
    Video SetActors(int id, IEnumerable<Actor> actors);
    Video SetGenres(int id, IEnumerable<Genre> genres);
    Task Delete(int id);
    Task<Video> UpdateTitle(int id, string title);
  }
}