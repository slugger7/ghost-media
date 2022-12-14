using Ghost.Data;
using Ghost.Dtos;

namespace Ghost.Repository
{
    public interface IUserRepository
    {
        User? FindById(int id);
        IEnumerable<User> GetUsers();
        Task<User> Create(User user);
        Task<User> Delete(int id);
        User FindUserByLogin(string username, string password);
        Task<bool> ToggleFavouriteVideo(int id, int videoId);
        Task<bool> ToggleFavouriteActor(int id, int actorId);
        Task LogProgress(int id, int userId, ProgressUpdateDto progress);
        PageResult<Video> Favourites(int userId, string watchState, string[]? genresFilter, int page = 0, int limit = 10, string search = "", string sortBy = "title", bool ascending = true);
    }
}