using Ghost.Data.Entities;

namespace Ghost.Dtos
{
  public class ActorDto
  {
    public string _id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public ActorDto(Actor actor)
    {
      this._id = actor._id.ToString();
      this.Name = actor.Name;
    }
  }

}