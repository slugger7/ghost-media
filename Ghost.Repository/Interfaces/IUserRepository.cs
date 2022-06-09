using Ghost.Data;

namespace Ghost.Repository
{
  public interface IUserRepository
  {
    User? FindById(int id);
    IEnumerable<User> GetUsers();
    Task<User> Create(User user);
    Task<User> Delete(int id);
    Task<bool> ToggleFavouriteVideo(int id, int videoId);
    Task LogProgress(int id, int userId, double progress);
  }
}