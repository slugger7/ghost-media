using Ghost.Data;

namespace Ghost.Repository;

public interface IPlaylistRepository
{
  Task<List<Playlist>> GetPlaylists(int userId);
  Task<Playlist?> GetPlaylistById(int userId, int id);
  PageResult<Video> GetVideos(int playlistId, int userId, string watchState, string[]? genres, int page = 0, int limit = 10, string search = "", string sortBy = "title", bool ascending = true);
  Task<Playlist> CreatePlaylist(Playlist playlist);
  Task DeletePlaylist(int id);
  Task<Playlist> UpdatePlaylist(int userId, int id, Playlist playlistDto);
}