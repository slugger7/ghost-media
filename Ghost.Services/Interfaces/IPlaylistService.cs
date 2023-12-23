using Ghost.Dtos;

namespace Ghost.Services;

public interface IPlaylistService
{
  Task<List<PlaylistDto>> GetPlaylists(int userId);
  Task<PlaylistDto?> GetPlaylistById(int userId, int id);
  Task<PlaylistDto> CreatePlaylist(int userId, CreatePlaylistDto playlistDto);
  Task DeletePlaylist(int id);
  Task<PlaylistDto> UpdatePlaylist(int userId, int id, UpdatePlaylistDto playlistDto);
  Task<PlaylistDto> AddVideosToPlaylist(int userId, int id, AddVideosToPlaylistDto addVideosToPlaylistDto);
  Task<PlaylistDto> RemoveVideoFromPlaylist(int userId, int id, int videoId);
  PageResultDto<VideoDto> GetVideos(int playlistId, int userId, PageRequestDto pageRequest, FilterQueryDto filters);
}
