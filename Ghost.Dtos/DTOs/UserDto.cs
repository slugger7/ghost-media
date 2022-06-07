using Ghost.Data;

namespace Ghost.Dtos
{
  public class UserDto
  {
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public List<FavouriteVideoDto> FavouriteVideos { get; set; } = new List<FavouriteVideoDto>();
    public List<FavouriteActorDto> FavouriteActors { get; set; } = new List<FavouriteActorDto>();

    public UserDto(User user)
    {
      this.Id = user.Id;
      this.Username = user.Username;
      if (user.FavouriteVideos != null)
      {
        this.FavouriteVideos = user.FavouriteVideos
          .Select(f => new FavouriteVideoDto(f))
          .ToList();
      }
      if (user.FavouriteActors != null)
      {
        this.FavouriteActors = user.FavouriteActors
          .Select(f => new FavouriteActorDto(f))
          .ToList();
      }
    }
  }
}