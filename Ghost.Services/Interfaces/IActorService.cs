using Ghost.Dtos;

namespace Ghost.Services
{
  public interface IActorService
  {
    List<ActorDto> GetActors(int userId);
    ActorDto GetActorByName(string name, int userId);
    List<ActorDto> GetActorsForVideo(int videoId, int userId);
    Task<ActorDto> UpdateName(int id, string name);
  }
}