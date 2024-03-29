using Ghost.Dtos;
using Ghost.Repository;

namespace Ghost.Services;
public class ActorService : IActorService
{
  private readonly IActorRepository actorRepository;

  public ActorService(IActorRepository actorRepository)
  {
    this.actorRepository = actorRepository;
  }

  public List<ActorDto> GetActors(int userId)
  {
    return actorRepository.GetActors()
      .Select(a => new ActorDto(a, userId))
      .ToList();
  }

  public List<ActorDto> GetFavouriteActors(int userId)
  {
    return actorRepository.GetFavouriteActors(userId)
      .Select(a => new ActorDto(a, userId))
      .ToList();
  }

  public ActorDto GetActorByName(string name, int userId)
  {
    var actor = actorRepository.GetActorByName(name);

    if (actor == null) throw new NullReferenceException("Actor not found");

    return new ActorDto(actor, userId);
  }

  public List<ActorDto> GetActorsForVideo(int videoId, int userId)
  {
    var actors = actorRepository.GetActorsForVideo(videoId);

    return actors.Select(a => new ActorDto(a, userId)).ToList();
  }

  public async Task<ActorDto> UpdateName(int id, string name)
  {
    var actor = await actorRepository.UpdateName(id, name);

    return new ActorDto(actor);
  }
}