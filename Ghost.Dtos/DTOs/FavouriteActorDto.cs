using Ghost.Data;

namespace Ghost.Dtos
{
  public class FavouriteActorDto
  {
    public int Id { get; set; }
    public ActorDto Actor { get; set; }

    public FavouriteActorDto(FavouriteActor favouriteActor)
    {
      this.Id = favouriteActor.Id;
      this.Actor = new ActorDto(favouriteActor.Actor);
    }
  }
}