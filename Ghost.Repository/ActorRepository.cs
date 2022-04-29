using Ghost.Data;
using Microsoft.EntityFrameworkCore;

namespace Ghost.Repository
{
  public class ActorRepository : IActorRepository
  {
    private readonly GhostContext context;

    public ActorRepository(GhostContext context)
    {
      this.context = context;
    }
    public Actor? FindById(int id)
    {
      return context.Actors
        .Include("VideoActors.Video")
        .FirstOrDefault(a => a.Id == id);
    }

    public Actor? GetActorByName(string name)
    {
      return context.Actors
        .Include("VideoActors.Video")
        .FirstOrDefault(a => a.Name.ToUpper().Equals(name.Trim().ToUpper()));
    }

    public IEnumerable<Actor> GetActors()
    {
      return context.Actors.Include("VideoActors").OrderByDescending(a => a.VideoActors.Count());
    }

    public Actor UpsertActor(string name)
    {
      var actor = context.Actors.FirstOrDefault(a => a.Name.ToUpper().Equals(name.Trim().ToUpper()));

      if (actor == null)
      {
        actor = new Actor
        {
          Name = name.Trim()
        };

        context.Actors.Add(actor);
        context.SaveChanges();
      }

      return actor;
    }
  }
}