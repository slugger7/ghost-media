using Ghost.Dtos;
using Ghost.Data;
using Ghost.Repository;

namespace Ghost.Services
{
  public class UserService : IUserService
  {
    private readonly IUserRepository userRepository;
    public UserService(IUserRepository userRepository)
    {
      this.userRepository = userRepository;
    }

    public async Task<UserDto> Create(CreateUserDto createUser)
    {
      var user = new User
      {
        Username = createUser.Username,
        Password = createUser.Password // this needs to be hashed
      };

      user = await this.userRepository.Create(user);

      return new UserDto(user);
    }

    public UserDto FindById(int id)
    {
      var user = userRepository.FindById(id);
      if (user == null) throw new NullReferenceException("User not found");

      return new UserDto(user);
    }

    public PageResultDto<UserDto> GetUsers()
    {
      var users = userRepository.GetUsers();

      return new PageResultDto<UserDto>
      {
        Content = users.Select(u => new UserDto(u)).ToList()
      };
    }
  }
}