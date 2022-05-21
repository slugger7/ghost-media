using Ghost.Data;

namespace Ghost.Dtos
{
  public class ActorDto
  {
    public string _id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int VideoCount { get; set; }

    public ActorDto(Actor actor)
    {
      this._id = actor.Id.ToString();
      this.Name = actor.Name;
      if (actor.VideoActors != null)
      {
        this.VideoCount = actor.VideoActors.Count();
      }
    }
  }

}