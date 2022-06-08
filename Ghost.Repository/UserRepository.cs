using Ghost.Data;
using Ghost.Exceptions;

namespace Ghost.Repository
{
  public class UserRepository : IUserRepository
  {
    private readonly GhostContext context;
    public UserRepository(GhostContext context)
    {
      this.context = context;
    }

    public async Task<User> Create(User user)
    {
      var userEntity = context.Users
        .FirstOrDefault(u => u.Username.ToLower().Equals(user.Username.Trim().ToLower()));
      if (userEntity != null) throw new UserExisistException();
      context.Users.Add(user);
      await context.SaveChangesAsync();

      return user;
    }

    public async Task<User> Delete(int id)
    {
      var user = context.Users.Find(id);
      if (user == null) throw new NullReferenceException("User was not found");

      context.Users.Remove(user);

      await context.SaveChangesAsync();

      return user;
    }

    public User? FindById(int id)
    {
      return context.Users.Find(id);
    }

    public IEnumerable<User> GetUsers()
    {
      return context.Users;
    }
  }
}