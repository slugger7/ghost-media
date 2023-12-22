using Ghost.Data;

namespace Ghost.Repository;

public interface IPlaylistRepository
{
  Task<List<Playlist>> GetPlaylists(int userId);
  Task<Playlist?> GetPlaylistById(int userId, int id);
  Task<Playlist> CreatePlaylist(Playlist playlist);
  Task DeletePlaylist(int id);
}