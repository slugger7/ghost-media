using Ghost.Dtos;
using Ghost.Repository;

namespace Ghost.Services
{
  public class ActorService : IActorService
  {
    private readonly IActorRepository actorRepository;

    public ActorService(IActorRepository actorRepository)
    {
      this.actorRepository = actorRepository;
    }

    public List<ActorDto> GetActors()
    {
      return actorRepository.GetActors()
        .Select(a => new ActorDto(a))
        .ToList();
    }

    public ActorDto GetActorByName(string name)
    {
      var actor = actorRepository.GetActorByName(name);

      if (actor == null) throw new NullReferenceException("Actor not found");

      return new ActorDto(actor);
    }
  }
}