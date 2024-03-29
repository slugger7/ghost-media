using Ghost.Data;
using Microsoft.EntityFrameworkCore;
using Ghost.Repository.Extensions;

namespace Ghost.Repository;
public class ActorRepository : IActorRepository
{
  private readonly GhostContext context;

  public ActorRepository(GhostContext context)
  {
    this.context = context;
  }

  public Actor? FindById(int id)
  {
    return this.FindById(id, new List<string>
    {
      "VideoActors.Video",
      "VideoActors.Video.VideoImages.Image",
      "VideoActors.Video.FavouritedBy.User",
      "VideoActors.Video.VideoActors.Actor.FavouritedBy.User",
      "VideoActors.Video.WatchedBy.User",
      "FavouritedBy.User"
    });
  }

  public Actor? FindById(int id, List<string>? includes)
  {
    return context.Actors
      .AddIncludes(includes)
      .FirstOrDefault(v => v.Id == id);
  }

  public Actor? GetActorByName(string name)
  {
    return context.Actors
      .AddIncludes(new List<string> {
        "VideoActors.Video",
        "VideoActors.Video.VideoImages.Image",
        "FavouritedBy.User"
      })
      .FirstOrDefault(a => a.Name.ToUpper().Equals(name.Trim().ToUpper()));
  }

  public IEnumerable<Actor> GetActors()
  {
    return context.Actors
      .Include("VideoActors")
      .Include("FavouritedBy.User")
      .OrderByDescending(a => a.VideoActors.Count());
  }

  public IEnumerable<Actor> GetFavouriteActors(int userId)
  {
    return context.Actors
      .Include("VideoActors")
      .Include("FavouritedBy.User")
      .Where(actor => actor.FavouritedBy.Any(fav => fav.User.Id == userId))
      .OrderByDescending(a => a.VideoActors.Count());
  }

  public IEnumerable<Actor> GetActorsForVideo(int videoId)
  {
    return context.VideoActors
      .Include("Actor.VideoActors")
      .Include("Actor.FavouritedBy.User")
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