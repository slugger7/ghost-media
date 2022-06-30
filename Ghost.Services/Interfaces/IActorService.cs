using Ghost.Dtos;

namespace Ghost.Services
{
  public interface IActorService
  {
    List<ActorDto> GetActors();
    ActorDto GetActorByName(string name, int userId);
    List<ActorDto> GetActorsForVideo(int videoId);
    Task<ActorDto> UpdateName(int id, string name);
  }
}