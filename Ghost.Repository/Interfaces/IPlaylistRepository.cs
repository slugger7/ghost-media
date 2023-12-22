using Ghost.Data;

namespace Ghost.Repository;

public interface IPlaylistRepository
{
  Task<List<Playlist>> GetPlaylists(int userId);
  Task<Playlist?> GetPlaylistById(int userId, int id);
  Task<Playlist> CreatePlaylist(Playlist playlist);
  Task DeletePlaylist(int id);
  Task<Playlist> UpdatePlaylist(int userId, int id, Playlist playlistDto);
}