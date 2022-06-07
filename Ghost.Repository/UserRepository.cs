using Ghost.Data;

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
      context.Users.Add(user);
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