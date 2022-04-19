using Ghost.Data.Entities;
using Ghost.Dtos;
using Ghost.Services.Interfaces;
using LiteDB;

namespace Ghost.Services
{
  public class ActorService : IActorService
  {
    private static string connectionString = $"..{Path.DirectorySeparatorChar}Ghost.Data{Path.DirectorySeparatorChar}Ghost.db";
    private static string collectionName = "actors";
    internal static ILiteCollection<Actor> GetCollection(LiteDatabase db)
    {
      var col = db.GetCollection<Actor>(collectionName);
      col.EnsureIndex(a => a.Name);

      return col;
    }

    internal static Actor UpsertActorByNameEntity(LiteDatabase db, string actor, Video video)
    {
      var col = ActorService.GetCollection(db);

      var actorEntity = col.FindOne(a => a.Name.ToUpper().Equals(actor.ToUpper()));

      if (actorEntity == null)
      {
        actorEntity = new Actor
        {
          Name = actor
        };

        col.Insert(actorEntity);
      }

      return actorEntity;
    }

    public List<ActorDto> GetActors()
    {
      using (var db = new LiteDatabase(connectionString))
      {
        var col = GetCollection(db);

        var actors = col.Query()
          .OrderBy(a => a.Name)
          .ToEnumerable()
          .Select(a => new ActorDto(a))
          .ToList();

        return actors;
      }
    }

    public ActorDto GetGenreByName(string name)
    {
      using (var db = new LiteDatabase(connectionString))
      {
        var col = GetCollection(db);

        var actor = col.FindOne(a => a.Name.ToUpper().Equals(name.ToUpper()));

        if (actor == null) throw new NullReferenceException("Actor not found");

        return new ActorDto(actor);
      }
    }
  }
}