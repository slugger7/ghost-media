using Ghost.Data;

namespace Ghost.Dtos
{
  public class ActorDto
  {
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int VideoCount { get; set; }
    public bool Favourite { get; set; }

    public ActorDto(Actor actor)
    {
      initialize(actor);
    }

    public ActorDto(Actor actor, int userId)
    {
      initialize(actor);
      this.Favourite = actor.FavouritedBy.Any(f => f.User.Id == userId);
    }

    public void initialize(Actor actor)
    {
      this.Id = actor.Id.ToString();
      this.Name = actor.Name;
      if (actor.VideoActors != null)
      {
        this.VideoCount = actor.VideoActors.Count();
      }
    }
  }

}