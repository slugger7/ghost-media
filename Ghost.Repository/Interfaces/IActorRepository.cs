using Ghost.Data;

namespace Ghost.Repository
{
  public interface IActorRepository
  {
    IEnumerable<Actor> GetActors();
    Actor? FindById(int id);
    Actor? GetActorByName(string name);
    Actor UpsertActor(string name);
  }
}