using Ghost.Data;

namespace Ghost.Repository
{
  public interface IVideoRepository
  {
    PageResult<Video> GetVideos(int page = 0, int limit = 10);
    PageResult<Video> GetForActor(int actorId, int page = 0, int limit = 10);
    PageResult<Video> GetForGenre(string name, int page = 0, int limit = 10);
    Video? FindById(int id);
    Video AddActors(int id, IEnumerable<Actor> actors);
    Video AddGenres(int id, IEnumerable<Genre> genres);
    Task Delete(int id);
  }
}