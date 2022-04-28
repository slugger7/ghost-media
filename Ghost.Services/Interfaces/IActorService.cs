using Ghost.Dtos;

namespace Ghost.Services
{
  public interface IActorService
  {
    List<ActorDto> GetActors();
    ActorDto GetActorByName(string name);
  }
}