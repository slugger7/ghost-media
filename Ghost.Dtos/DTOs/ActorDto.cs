using Ghost.Data;

namespace Ghost.Dtos
{
  public class ActorDto
  {
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int VideoCount { get; set; }

    public ActorDto(Actor actor)
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