using Ghost.Data.Entities;
using Ghost.Services.Interfaces;
using LiteDB;

namespace Ghost.Services
{
  public class ActorService : IActorService
  {
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
  }
}