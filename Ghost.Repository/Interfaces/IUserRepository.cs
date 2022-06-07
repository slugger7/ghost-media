using Ghost.Data;

namespace Ghost.Repository
{
  public interface IUserRepository
  {
    User? FindById(int id);
    IEnumerable<User> GetUsers();
    Task<User> Create(User user);
  }
}