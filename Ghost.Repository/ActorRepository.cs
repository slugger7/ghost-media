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
        .Include("VideoActors.Video.VideoImages.Image")
        .Include("VideoActors.Video.FavouritedBy.User")
        .FirstOrDefault(a => a.Id == id);
    }

    public Actor? GetActorByName(string name)
    {
      return context.Actors
        .Include("VideoActors.Video")
        .Include("VideoActors.Video.VideoImages.Image")
        .FirstOrDefault(a => a.Name.ToUpper().Equals(name.Trim().ToUpper()));
    }

    public IEnumerable<Actor> GetActors()
    {
      return context.Actors.Include("VideoActors").OrderByDescending(a => a.VideoActors.Count());
    }

    public IEnumerable<Actor> GetActorsForVideo(int videoId)
    {
      return context.VideoActors
        .Include("Actor.VideoActors")
        .Where(va => va.Video.Id == videoId)
        .Select(va => va.Actor)
        .OrderByDescending(a => a.VideoActors.Count());
    }

    public async Task<Actor> UpdateName(int id, string name)
    {
      var actor = this.FindById(id);
      if (actor == null) throw new NullReferenceException("Actor was not found");

      actor.Name = name;

      await context.SaveChangesAsync();

      return actor;
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