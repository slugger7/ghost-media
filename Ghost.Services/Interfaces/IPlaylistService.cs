using Ghost.Dtos;

namespace Ghost.Services;

public interface IPlaylistService
{
  Task<List<PlaylistDto>> GetPlaylists(int userId);
  Task<PlaylistDto?> GetPlaylistById(int userId, int id);
  Task<PlaylistDto> CreatePlaylist(int userId, CreatePlaylistDto playlistDto);
}
