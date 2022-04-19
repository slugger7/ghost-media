using Ghost.Dtos;

namespace Ghost.Services.Interfaces
{
  public interface IActorService
  {
    List<ActorDto> GetActors();
    ActorDto GetGenreByName(string name);
  }
}